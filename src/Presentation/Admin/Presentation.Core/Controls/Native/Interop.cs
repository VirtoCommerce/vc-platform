using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace VirtoCommerce.ManagementClient.Core.Controls.Native
{
    internal static class Interop
    {
        #region Constants

        [MarshalAs(UnmanagedType.U4)]
        internal const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        [MarshalAs(UnmanagedType.SysUInt)]
        internal const int ABS_ALWAYSONTOP = 0x0000002;

        [MarshalAs(UnmanagedType.SysUInt)]
        internal const int ABS_AUTOHIDE = 0x0000001;

        [MarshalAs(UnmanagedType.U4)]
        internal const int ABM_GETSTATE = 0x00000004;

        [MarshalAs(UnmanagedType.U4)]
        internal const int ABM_GETTASKBARPOS = 0x00000005;

        internal const int WM_GETMINMAXINFO = 0x0024;


        #endregion

        #region Enumerations

        internal enum ABE
        {
            [MarshalAs(UnmanagedType.U4)]
            Left = 0,

            [MarshalAs(UnmanagedType.U4)]
            Top,

            [MarshalAs(UnmanagedType.U4)]
            Right,

            [MarshalAs(UnmanagedType.U4)]
            Bottom
        }

        #endregion

        #region Structures

        [StructLayout(LayoutKind.Sequential)]
        internal struct MONITORINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            public RECT rcMonitor;

            public RECT rcWork;

            [MarshalAs(UnmanagedType.U4)]
            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct APPBARDATA
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            public IntPtr hWnd;

            [MarshalAs(UnmanagedType.U4)]
            public int uCallbackMessage;

            [MarshalAs(UnmanagedType.U4)]
            public ABE uEdge;

            public RECT rc;

            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WINDOWINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;

            public RECT rcWindow;

            public RECT rcClient;
            
            [MarshalAs(UnmanagedType.U4)]
            public int dwStyle;

            [MarshalAs(UnmanagedType.U4)]
            public int dwExStyle;

            [MarshalAs(UnmanagedType.U4)]
            public int dwWindowStatus;
            
            [MarshalAs(UnmanagedType.U4)]
            public int cxWindowBorders;

            [MarshalAs(UnmanagedType.U4)]
            public int cyWindowBorders;

            [MarshalAs(UnmanagedType.U2)]
            public short atomWindowType;

            [MarshalAs(UnmanagedType.U2)]
            public short wCreatorVersion;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct POINT
        {
            public int x;
            public int y;
        }

        #endregion

        #region Functions

        [SecurityCritical]
        [DllImport("user32.dll", EntryPoint = "FindWindow", ExactSpelling = false, CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr _FindWindow(string lpClassName, string lpWindowName);

        [SecurityCritical]
        internal static IntPtr FindWindow(string lpClassName, string lpWindowName)
        {
            var handle = _FindWindow(lpClassName, lpWindowName);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return handle;
        }

        [SecurityCritical]
        [DllImport("user32.dll", EntryPoint = "GetMonitorInfo", ExactSpelling = false, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _GetMonitorInfo(IntPtr hMonitor, [In] [Out] ref MONITORINFO lpmi);

        [SecurityCritical]
        internal static void GetMonitorInfo(IntPtr hMonitor, out MONITORINFO lpmi)
        {
            lpmi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
            var result = _GetMonitorInfo(hMonitor, ref lpmi);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        [SecurityCritical]
        [DllImport("user32.dll", EntryPoint = "GetWindowInfo", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool _GetWindowInfo(IntPtr hwnd, [In] [Out] ref WINDOWINFO pwi);

        [SecurityCritical]
        internal static void GetWindowInfo(IntPtr hwnd, out WINDOWINFO pwi)
        {
            pwi = new WINDOWINFO { cbSize = Marshal.SizeOf(typeof(WINDOWINFO)) };
            var result = _GetWindowInfo(hwnd, ref pwi);
            if (!result)
            {
                throw new Win32Exception();
            }
        }

        [SecurityCritical]
        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr _MonitorFromWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.U4)] int dwFlags);

        [SecurityCritical]
        internal static IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags)
        {
            var handle = _MonitorFromWindow(hwnd, dwFlags);
            if (handle == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return handle;
        }

        [SecurityCritical]
        [DllImport("shell32.dll", EntryPoint = "SHAppBarMessage", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = false)]
        [return: MarshalAs(UnmanagedType.SysUInt)]
        private static extern IntPtr _SHAppBarMessage([MarshalAs(UnmanagedType.U4)] int dwMessage, [In] [Out] ref APPBARDATA pData);

        [SecurityCritical]
        internal static IntPtr SHAppBarMessage(int dwMessage, ref APPBARDATA pData)
        {
            var result = _SHAppBarMessage(dwMessage, ref pData);
            if (dwMessage == ABM_GETTASKBARPOS && result == IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }
            return result;
        }

        #endregion
    }
}