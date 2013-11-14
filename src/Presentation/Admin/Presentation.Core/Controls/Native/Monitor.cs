using System;
using System.Security;

namespace VirtoCommerce.ManagementClient.Core.Controls.Native
{
    internal class Monitor
    {
        [SecurityCritical]
        private static bool _isCacheValid;

        [SecuritySafeCritical]
        internal Monitor(IntPtr hwnd)
        {
            _handle = Interop.MonitorFromWindow(hwnd, Interop.MONITOR_DEFAULTTONEAREST);
        }

        [SecuritySafeCritical]
        internal void Invalidate()
        {
            _isCacheValid = false;
        }

        [SecurityCritical]
        private void InvalidateInternal()
        {
            Interop.MONITORINFO monitorInfo;
            Interop.GetMonitorInfo(_handle, out monitorInfo);

            _bounds = monitorInfo.rcMonitor;
            _workArea = monitorInfo.rcWork;

            _isCacheValid = true;
        }

        internal IntPtr Handle
        {
            [SecurityCritical]
            get { return _handle; }
        }

        private readonly IntPtr _handle;

        internal Interop.RECT Bounds
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _bounds;
            }
        }

        private Interop.RECT _bounds;

        internal Interop.RECT WorkArea
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _workArea;
            }
        }

        private Interop.RECT _workArea;
    }
}