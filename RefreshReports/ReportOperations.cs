using System;
using System.Threading;

namespace RefreshReports
{
    internal class ReportOperations
    {
        public static string VerifyDatabase(IntPtr hwndMain)
        {
            var ret = Win32ApiFunctions.PostMessage(hwndMain, Win32ApiFunctions.WM_COMMAND, (IntPtr)Win32ApiFunctions.VERIFY_DATABASE, IntPtr.Zero);
            Thread.Sleep(500);

            bool wasDialogFound = false;
            IntPtr hwndDialog;
            do
            {
                //Maybe increase timeout if your computer is slow. But 2 seconds should be enough for most of the cases.
                hwndDialog = Win32ApiFunctions.WaitForFindWindow("Verify Database", TimeSpan.FromSeconds(2));

                if (hwndDialog != IntPtr.Zero)
                {
                    wasDialogFound = true;
                    Win32ApiFunctions.ClickOnFirstDialogButton(hwndDialog, "Ok");
                }
            }
            while (hwndDialog != IntPtr.Zero);

            if (!wasDialogFound)
                return "NOT_VERIFY";//throw new Exception("Dialog not found. Probably 'Verify Database' was not pressed. Try manually.");
            else
                return "VERIFY";
        }
    }
}
