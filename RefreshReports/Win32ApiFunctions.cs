using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace RefreshReports
{
    internal class Win32ApiFunctions
    {
        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parent, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public const int WM_COMMAND = 0x0111;
        public const int VERIFY_DATABASE = 0xB041;//Find by Spy++
        public const int BM_CLICK = 0x00F5;
        public const int WM_CLOSE = 0x0010;

        public static IntPtr WaitForMainWindow(Process process, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                Thread.Sleep(200);
                process.Refresh();
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    return process.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr WaitForFindWindow(string windowName, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                Thread.Sleep(200);
                IntPtr hwnd = FindWindow(null, windowName);
                if (hwnd != IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    return hwnd;
                }
            }
            return IntPtr.Zero;
        }

        private static IntPtr WaitForFindButtom(IntPtr hwndDialogWindow, TimeSpan timeout)
        {
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                Thread.Sleep(200);
                IntPtr hwnd = FindWindowEx(hwndDialogWindow, IntPtr.Zero, "Button", null);
                if (hwnd != IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    return hwnd;
                }
            }
            return IntPtr.Zero;
        }

        public static IntPtr ClickOnFirstDialogButton(IntPtr hwndDialogWindow, string buttonText)
        {
            IntPtr hwndButton = WaitForFindButtom(hwndDialogWindow, TimeSpan.FromSeconds(4));
            if (hwndButton == IntPtr.Zero)
                throw new Exception($"Button '{buttonText}' not found");

            Thread.Sleep(200);
            var ret = SendMessage(hwndButton, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            Thread.Sleep(500);
            return ret;
        }
    }
}
