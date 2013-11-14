using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VirtoCommerce.ManagementClient.Core.Controls.Native
{
    internal static class Taskbar
    {
        [SecurityCritical]
        private static bool _isCacheValid;

        [SecuritySafeCritical]
        internal static void Invalidate()
        {
            _isCacheValid = false;
        }

        [SecurityCritical]
        private static void InvalidateInternal()
        {
            _handle = Interop.FindWindow("Shell_TrayWnd", null);

            var data = new Interop.APPBARDATA { hWnd = _handle, cbSize = Marshal.SizeOf(typeof(Interop.APPBARDATA)) };
            Interop.SHAppBarMessage(Interop.ABM_GETTASKBARPOS, ref data);

            _position = (TaskbarPosition)data.uEdge;
            _left = data.rc.left;
            _top = data.rc.top;
            _right = data.rc.right;
            _bottom = data.rc.bottom;

            data = new Interop.APPBARDATA { hWnd = _handle, cbSize = Marshal.SizeOf(typeof(Interop.APPBARDATA)) };

            var state = Interop.SHAppBarMessage(Interop.ABM_GETSTATE, ref data).ToInt32();

            // See http://msdn.microsoft.com/en-us/library/bb787947(v=vs.85).aspx
            _alwaysOnTop = (state & Interop.ABS_ALWAYSONTOP) == Interop.ABS_ALWAYSONTOP || (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1);
            _autoHide = (state & Interop.ABS_AUTOHIDE) == Interop.ABS_AUTOHIDE;

            _isCacheValid = true;
        }

        internal static IntPtr Handle
        {
            [SecurityCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _handle;
            }
        }

        private static IntPtr _handle;

        internal static TaskbarPosition Position
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _position;
            }
        }

        private static TaskbarPosition _position;

        internal static int Left
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _left;
            }
        }

        private static int _left;

        internal static int Top
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _top;
            }
        }

        private static int _top;

        internal static int Right
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _right;
            }
        }

        private static int _right;

        internal static int Bottom
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _bottom;
            }
        }

        private static int _bottom;

        internal static int Width
        {
            [SecuritySafeCritical]
            get { return Right - Left; }
        }

        internal static int Height
        {
            [SecuritySafeCritical]
            get { return Bottom - Top; }
        }

        internal static bool AlwaysOnTop
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _alwaysOnTop;
            }
        }

        private static bool _alwaysOnTop;

        internal static bool AutoHide
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _autoHide;
            }
        }

        private static bool _autoHide;
    }
}