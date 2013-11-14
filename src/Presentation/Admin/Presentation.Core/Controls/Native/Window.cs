using System;
using System.Security;

namespace VirtoCommerce.ManagementClient.Core.Controls.Native
{
    internal class Window
    {
        [SecurityCritical]
        private static bool _isCacheValid;

        [SecuritySafeCritical]
        internal Window(IntPtr hwnd)
        {
            _handle = hwnd;
        }

        [SecuritySafeCritical]
        internal void Invalidate()
        {
            _isCacheValid = false;
        }

        [SecurityCritical]
        private void InvalidateInternal()
        {
            Interop.WINDOWINFO windowInfo;
            Interop.GetWindowInfo(_handle, out windowInfo);

            _bounds = windowInfo.rcWindow;
            _clientArea = windowInfo.rcClient;
            _nonClientBorderWidth = windowInfo.cxWindowBorders;
            _nonClientBorderHeight = windowInfo.cyWindowBorders;
            _windowStyle = windowInfo.dwStyle;
            _windowExStyle = windowInfo.dwExStyle;

            _isCacheValid = true;
        }

        internal IntPtr Handle
        {
            [SecurityCritical]
            get { return _handle; }
        }

        private readonly IntPtr _handle;

        internal int WindowStyle
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _windowStyle;
            }
        }

        private int _windowStyle;

        internal int WindowExStyle
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _windowExStyle;
            }
        }

        private int _windowExStyle;

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

        internal Interop.RECT ClientArea
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _clientArea;
            }
        }

        private Interop.RECT _clientArea;

        internal int NonClientBorderWidth
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _nonClientBorderWidth;
            }
        }

        private int _nonClientBorderWidth;

        internal int NonClientBorderHeight
        {
            [SecuritySafeCritical]
            get
            {
                if (!_isCacheValid)
                {
                    InvalidateInternal();
                }
                return _nonClientBorderHeight;
            }
        }

        private int _nonClientBorderHeight;
    }
}