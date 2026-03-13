using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace RefreshReports
{
    public partial class RefreshReports : Form
    {
        private const string CRW32_PATH = @"C:\Program Files (x86)\Business Objects\Crystal Reports 11.5\crw32.exe";
        private const string INFO_TEXT = "Začnite stlačením tlačidla 'Štart refresh'";

        public RefreshReports()
        {
            InitializeComponent();
            lblInfo.Text = INFO_TEXT;
        }

        private void btnStartRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnStartRefresh.Enabled) return;
                if (!ReadyForRunning()) return;

                this.Cursor = Cursors.WaitCursor;
                btnStartRefresh.Enabled = false;

                //TODO : For testing purposes, you can set the path to the folder with reports here and comment FolderBrowserDialog part.
                string reportsPath = @"c:\reporty";//@"c:\_Projects\OlympGit\Olymp\Reporty\";//string.Empty;// @"c:\reporty";//
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.Description = "Vyberte priečinok s reportami (*.rpt)";

                    if (dialog.ShowDialog() == DialogResult.OK)                    
                        reportsPath = dialog.SelectedPath;                    
                    else
                        return;
                }

                var files = Directory.GetFiles(reportsPath, "*.rpt");
                if (files.Length == 0)
                {
                    MessageBox.Show("V priečinoku neboli nájdené žiadne reporty (*.rpt).", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Main processing of reports
                var results = ProcesReports(files);

                new Results(results).ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblInfo.Text = INFO_TEXT;
                btnStartRefresh.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private bool ReadyForRunning()
        {
            //Check if Default printer is FinePrint
            var ps = new PrinterSettings();
            if (string.IsNullOrEmpty(ps.PrinterName) 
                || !ps.IsValid 
                || ps.PrinterName.ToLower() != "fineprint")
            {
                MessageBox.Show(
                    "Nie je nastavená predvolená tlačiareň na 'FinePrint',\r\nalebo tlačiareň 'FinePrint' je nedostupná.",
                    "Pozor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            //Check if some Crystal is already running.
            var existingProcesses = Process.GetProcessesByName("crw32");
            if (existingProcesses.Length > 0)
            {
                MessageBox.Show(
                    "Crystal Reports (crw32.exe) už beží. Najprv ho zatvorte.",
                    "Pozor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            //Check if Crystal v11.5 is instaled.
            if (!File.Exists(CRW32_PATH))
            {
                Console.WriteLine();
                MessageBox.Show(
                    $"Súbor crw32.exe neexistuje.\r\nPravdepodobne nemáte nainštalovaný Crystal Reports ver.11.5.\r\n\r\n{CRW32_PATH}",
                    "Pozor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private string ProcesReports(string[] reportFiles)
        {
            var sbInfos = new System.Text.StringBuilder();
            var sbErrors = new System.Text.StringBuilder();
            int errorsCount = 0;
            int updatedCount = 0;

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < reportFiles.Length; i++)
            {            
                var file = reportFiles[i];
                var fileName = Path.GetFileName(file);
                lblInfo.Text = $"Spracovávám ({i + 1}/{reportFiles.Length}):\r\n\t{fileName}";
                Application.DoEvents();
                sbInfos.Append(fileName);
                try
                {
                    Process process;
                    IntPtr hwndMain;
                    OpenReport(out process, out hwndMain, file);

                    var retInfo = ReportOperations.VerifyDatabase(hwndMain);
                    sbInfos.Append($"\t{retInfo}");

                    if (CloseAndSaveReport(process, hwndMain))
                    {
                        sbInfos.Append("\tUPDATED");
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    sbInfos.Append("\tERROR");
                    sbErrors.AppendLine(ex.Message);
                    errorsCount++;
                }
                sbInfos.AppendLine();
            }

            sw.Stop();
            string timeElapsed = sw.Elapsed.ToString(@"hh\:mm\:ss");

            var summary = $"Počet spracovaný reportov:\t\t\t{reportFiles.Length}\r\n" +
                            $"Počet aktualizovaných(uložených) reportov:\t{updatedCount}\r\n" +
                            $"Počet chýb:\t{errorsCount}\r\n" +
                            $"Čas spracovania:\t{timeElapsed}" +
                            "\r\n\r\n-----------------Spracované reporty-------------------\r\n\r\n";
            summary += sbInfos.ToString();

            if (errorsCount > 0)
            {
                summary += "\r\n\r\n-----------------Chyby-------------------\r\n\r\n";
                summary += sbErrors.ToString();
            }

            return summary;
        }

        private static void OpenReport(out Process process, out IntPtr hwndMain, string reportFile)
        {
            var existingProcesses = Process.GetProcessesByName("crw32");
            if (existingProcesses.Length > 0)
                throw new Exception("Crystal Reports (crw32.exe) is running");

            var psi = new ProcessStartInfo();
            psi.FileName = CRW32_PATH;
            psi.Arguments = $"\"{reportFile}\"";
            psi.WorkingDirectory = Path.GetDirectoryName(reportFile);
            psi.UseShellExecute = true;

            process = Process.Start(psi);
            Thread.Sleep(500);

            //Find main window of Crystal and set it to foreground.
            hwndMain = Win32ApiFunctions.WaitForMainWindow(process, TimeSpan.FromSeconds(10));
            if (hwndMain == IntPtr.Zero)
                throw new Exception("Crystal Reports main window not found.");

            Win32ApiFunctions.SetForegroundWindow(hwndMain);
            Thread.Sleep(200);
        }

        private static bool CloseAndSaveReport(Process process, IntPtr hwndMain)
        {
            bool wasSaved = false;

            // Closing main window of Crystal
            Win32ApiFunctions.PostMessage(hwndMain, Win32ApiFunctions.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(500);

            //If there are unsaved changes, Crystal should ask if you want to save. Let's click "Yes" on that dialog.
            IntPtr hwndSaveDialog = Win32ApiFunctions.WaitForFindWindow("Crystal Reports", TimeSpan.FromSeconds(3));
            if (hwndSaveDialog != IntPtr.Zero && hwndSaveDialog != hwndMain)
            {
                Win32ApiFunctions.ClickOnFirstDialogButton(hwndSaveDialog, "Áno");
                wasSaved = true;
            }

            //Wait for close Crystal process
            bool exited = process.WaitForExit(15000);
            if (!exited)
                throw new Exception("Crystal Reports didn't close in expected time.");

            return wasSaved;
        }
    }
}
