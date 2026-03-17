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

        #region Job Object API — kills child processes when parent exits

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll")]
        private static extern bool SetInformationJobObject(IntPtr hJob, int jobObjectInfoClass,
            IntPtr lpJobObjectInfo, uint cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AssignProcessToJobObject(IntPtr hJob, IntPtr hProcess);

        private const int JobObjectExtendedLimitInformation = 9;
        private const uint JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE = 0x00002000;

        [StructLayout(LayoutKind.Sequential)]
        private struct JOBOBJECT_BASIC_LIMIT_INFORMATION
        {
            public long PerProcessUserTimeLimit;
            public long PerJobUserTimeLimit;
            public uint LimitFlags;
            public UIntPtr MinimumWorkingSetSize;
            public UIntPtr MaximumWorkingSetSize;
            public uint ActiveProcessLimit;
            public long Affinity;
            public uint PriorityClass;
            public uint SchedulingClass;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct IO_COUNTERS
        {
            public ulong ReadOperationCount;
            public ulong WriteOperationCount;
            public ulong OtherOperationCount;
            public ulong ReadTransferCount;
            public ulong WriteTransferCount;
            public ulong OtherTransferCount;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct JOBOBJECT_EXTENDED_LIMIT_INFORMATION
        {
            public JOBOBJECT_BASIC_LIMIT_INFORMATION BasicLimitInformation;
            public IO_COUNTERS IoInfo;
            public UIntPtr ProcessMemoryLimit;
            public UIntPtr JobMemoryLimit;
            public UIntPtr PeakProcessMemoryUsed;
            public UIntPtr PeakJobMemoryUsed;
        }

        private static IntPtr _jobHandle = IntPtr.Zero;

        /// <summary>
        /// Creates a Job Object that kills all assigned child processes
        /// when the parent process exits (even if killed via Task Manager).
        /// Call once at application startup.
        /// </summary>
        public static void CreateKillOnCloseJob()
        {
            _jobHandle = CreateJobObject(IntPtr.Zero, null);
            if (_jobHandle == IntPtr.Zero)
                return;

            var info = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION();
            info.BasicLimitInformation.LimitFlags = JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE;

            int length = Marshal.SizeOf(typeof(JOBOBJECT_EXTENDED_LIMIT_INFORMATION));
            IntPtr infoPtr = Marshal.AllocHGlobal(length);
            try
            {
                Marshal.StructureToPtr(info, infoPtr, false);
                SetInformationJobObject(_jobHandle, JobObjectExtendedLimitInformation, infoPtr, (uint)length);
            }
            finally
            {
                Marshal.FreeHGlobal(infoPtr);
            }
        }

        /// <summary>
        /// Assigns a process to the kill-on-close job so it is terminated
        /// automatically when this application exits.
        /// </summary>
        public static void AssignToJob(Process process)
        {
            if (_jobHandle != IntPtr.Zero && process != null && !process.HasExited)
                AssignProcessToJobObject(_jobHandle, process.Handle);
        }

        #endregion

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
