using System;
using System.IO;
using System.Windows.Forms;

namespace RefreshReports
{
    public partial class Results : Form
    {
        public Results(string resultTxt)
        {
            InitializeComponent();
            txtResults.Text = resultTxt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Filter = "Text file (*.txt)|*.txt";
                    dialog.DefaultExt = "txt";
                    dialog.AddExtension = true;
                    dialog.FileName = $"RefreshReports_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(dialog.FileName, txtResults.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
