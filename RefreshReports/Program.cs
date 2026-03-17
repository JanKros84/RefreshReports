using System;
using System.Windows.Forms;

namespace RefreshReports
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Win32ApiFunctions.CreateKillOnCloseJob();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RefreshReports());
        }
    }
}
