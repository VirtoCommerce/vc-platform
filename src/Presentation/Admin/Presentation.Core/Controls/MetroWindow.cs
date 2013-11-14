using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Shell;
using System.Windows.Threading;

using VirtoCommerce.ManagementClient.Core.Controls.Native;

using Monitor = VirtoCommerce.ManagementClient.Core.Controls.Native.Monitor;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
    [TemplatePart(Name = LayoutRootName, Type = typeof(Panel))]
    [TemplatePart(Name = CaptionName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TitleBarHostName, Type = typeof(Decorator))]
    public class MetroWindow : System.Windows.Window
    {
        private const string LayoutRootName = "PART_LayoutRoot";
        private const string CaptionName = "PART_Caption";
        private const string TitleBarHostName = "PART_TitleBarHost";

        private Panel _layoutRoot;
        private FrameworkElement _caption;
        private Decorator _titleBarHost;

        private Monitor _monitor;

        private Native.Window _window;

        [SecurityCritical]
        private WindowChrome _chrome;

        static MetroWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(typeof(MetroWindow)));
            WindowStateProperty.OverrideMetadata(typeof(MetroWindow), new FrameworkPropertyMetadata(OnWindowStateChanged));
        }

        [SecuritySafeCritical]
        public MetroWindow()
        {
            Initialize();
        }

        [SecurityCritical]
        private void Initialize()
        {
            _chrome = new WindowChrome
                          {
                              //CaptionHeight = SystemParameters.WindowCaptionHeight,
                              CaptionHeight = 30,
                              CornerRadius = new CornerRadius(0d),
                              GlassFrameThickness = new Thickness(0d),
                              NonClientFrameEdges = NonClientFrameEdges.None,
                              ResizeBorderThickness = new Thickness(3),
                              UseAeroCaptionButtons = false
                          };
            if (_chrome.CanFreeze)
            {
                _chrome.Freeze();
            }
            if (WindowChrome.GetWindowChrome(this) == null)
            {
                WindowChrome.SetWindowChrome(this, _chrome);
            }

            Initialized += OnInitializedInternal;
            Loaded += OnLoadedInternal;
        }

        private SizeToContent _previousSizeToContent = SizeToContent.Manual;

        [SecurityCritical]
        private void OnInitializedInternal(object sender, EventArgs e)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && SizeToContent != SizeToContent.Manual)
            {
                _previousSizeToContent = SizeToContent;
                SizeToContent = SizeToContent.Manual;
            }
        }

        [SecurityCritical]
        private void OnLoadedInternal(object sender, RoutedEventArgs e)
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && _previousSizeToContent != SizeToContent.Manual)
            {
                SizeToContent = _previousSizeToContent;
                _previousSizeToContent = SizeToContent.Manual;

                if (WindowStartupLocation == WindowStartupLocation.CenterScreen)
                {
                    Left = SystemParameters.VirtualScreenLeft + SystemParameters.PrimaryScreenWidth / 2 - ActualWidth / 2;
                    Top = SystemParameters.VirtualScreenTop + SystemParameters.PrimaryScreenHeight / 2 - ActualHeight / 2;
                }
                if (WindowStartupLocation == WindowStartupLocation.CenterOwner)
                {
                    if (Owner != null)
                    {
                        if (Owner.WindowState == WindowState.Maximized)
                        {
                            var source = PresentationSource.FromVisual(Owner);
                            if (source != null && source.CompositionTarget != null)
                            {
                                var ownerHandle = new WindowInteropHelper(Owner).EnsureHandle();
                                var ownerWindow = new Native.Window(ownerHandle);
                                ownerWindow.Invalidate();
                                Left = -ownerWindow.NonClientBorderWidth * source.CompositionTarget.TransformFromDevice.M11;
                                Top = -ownerWindow.NonClientBorderHeight * source.CompositionTarget.TransformFromDevice.M22;
                            }
                            else
                            {
                                Left = 0;
                                Top = 0;
                            }
                        }
                        else
                        {
                            Left = Owner.Left;
                            Top = Owner.Top;
                        }
                        Left += Owner.ActualWidth / 2 - ActualWidth / 2;
                        Top += Owner.ActualHeight / 2 - ActualHeight / 2;
                    }
                }

                UpdateNonClientBorder();

                if (_dispatcherFrame != null)
                {
                    _dispatcherFrame.Continue = false;
                }
            }
        }

        [SecuritySafeCritical]
        protected override void OnSourceInitialized(EventArgs e)
        {
            Hook();
            base.OnSourceInitialized(e);
        }

        [SecurityCritical]
        private void Hook()
        {
            var handle = new WindowInteropHelper(this).EnsureHandle();
            _monitor = new Monitor(handle);
            _window = new Native.Window(handle);
            UpdateNonClientBorder();

            var source = HwndSource.FromHwnd(handle);
            if (source != null)
            {
                source.AddHook(WndProc);
            }
        }

        [SecurityCritical]
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Interop.WM_GETMINMAXINFO:
                    if (Equals(WindowChrome.GetWindowChrome(this), _chrome))
                    {
                        GetMinMaxInfo(lParam);
                        handled = true;
                    }
                    break;
            }

            return (IntPtr)0;
        }

        [SecurityCritical]
        private void GetMinMaxInfo(IntPtr lParam)
        {
            var info = (Interop.MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(Interop.MINMAXINFO));

            _monitor.Invalidate();
            Taskbar.Invalidate();

            var bounds = _monitor.Bounds;
            var workArea = _monitor.WorkArea;
            info.ptMaxPosition.x = Math.Abs(bounds.left) + Taskbar.Position == TaskbarPosition.Left && Taskbar.AutoHide ? 1 : 0;
            info.ptMaxPosition.y = Math.Abs(bounds.top) + Taskbar.Position == TaskbarPosition.Top && Taskbar.AutoHide ? 1 : 0;
            info.ptMaxSize.x = info.ptMaxTrackSize.x = Math.Abs(workArea.right - workArea.left) - (Taskbar.Position == TaskbarPosition.Right && Taskbar.AutoHide ? 1 : 0);
            info.ptMaxSize.y = info.ptMaxTrackSize.y = Math.Abs(workArea.bottom - workArea.top) - (Taskbar.Position == TaskbarPosition.Bottom && Taskbar.AutoHide ? 1 : 0);

            var source = PresentationSource.FromVisual(this);
            if (source != null && source.CompositionTarget != null)
            {
                if (IsNonNegative(MinWidth))
                {
                    info.ptMinTrackSize.x = (int)Math.Ceiling(MinWidth * source.CompositionTarget.TransformFromDevice.M11);
                }
                if (IsNonNegative(MinHeight))
                {
                    info.ptMinTrackSize.y = (int)Math.Ceiling(MinHeight * source.CompositionTarget.TransformFromDevice.M22);
                }
                if (IsNonNegative(MaxWidth))
                {
                    info.ptMaxSize.x = info.ptMaxTrackSize.x = Math.Min(info.ptMaxSize.x, (int)Math.Ceiling(MaxWidth * source.CompositionTarget.TransformFromDevice.M11));
                }
                if (IsNonNegative(MaxHeight))
                {
                    info.ptMaxSize.y = info.ptMaxTrackSize.y = Math.Min(info.ptMaxSize.y, (int)Math.Ceiling(MaxHeight * source.CompositionTarget.TransformFromDevice.M22));
                }
            }

            Marshal.StructureToPtr(info, lParam, true);
        }

        public static readonly DependencyProperty TitleBarProperty = DependencyProperty.Register("TitleBar", typeof(UIElement), typeof(MetroWindow), new FrameworkPropertyMetadata(null, OnTitleBarChanged));

        public UIElement TitleBar
        {
            get { return (UIElement)GetValue(TitleBarProperty); }
            set { SetValue(TitleBarProperty, value); }
        }

        private static void OnTitleBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = obj as MetroWindow;
            if (instance != null && instance._titleBarHost != null)
            {
                instance._titleBarHost.Child = null;
                instance._titleBarHost.Child = (UIElement)e.NewValue;
            }
        }

        [SecurityCritical]
        public new bool? ShowDialog()
        {
            if (Equals(WindowChrome.GetWindowChrome(this), _chrome) && SizeToContent != SizeToContent.Manual)
            {
                _dispatcherFrame = new DispatcherFrame();

                Show();

                Dispatcher.PushFrame(_dispatcherFrame);
                _dispatcherFrame = null;
                return base.ShowDialog();
            }
            return base.ShowDialog();
        }

        private DispatcherFrame _dispatcherFrame;

        [SecuritySafeCritical]
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OnApplyTemplateInternal();
        }

        [SecurityCritical]
        private void OnApplyTemplateInternal()
        {
            if (Template != null)
            {
                _layoutRoot = Template.FindName(LayoutRootName, this) as Panel;
                if (_layoutRoot == null)
                {
                    Trace.TraceError(LayoutRootName + " not found.");
                }

                _caption = Template.FindName(CaptionName, this) as FrameworkElement;
                if (_caption == null)
                {
                    Trace.TraceError(CaptionName + " not found.");
                }
                else
                {
                    _caption.SizeChanged += OnCaptionSizeChanged;
                }

                if (_titleBarHost != null)
                {
                    _titleBarHost.Child = null;
                }
                _titleBarHost = Template.FindName(TitleBarHostName, this) as Decorator;
                if (_titleBarHost == null)
                {
                    Trace.TraceError(TitleBarHostName + " not found.");
                }
                else
                {
                    _titleBarHost.Child = TitleBar;
                }
            }
        }

        [SecurityCritical]
        private void OnCaptionSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged && Equals(WindowChrome.GetWindowChrome(this), _chrome))
            {
                _chrome = new WindowChrome
                              {
                                  CaptionHeight = e.NewSize.Height,
                                  CornerRadius = _chrome.CornerRadius,
                                  GlassFrameThickness = _chrome.GlassFrameThickness,
                                  NonClientFrameEdges = _chrome.NonClientFrameEdges,
                                  ResizeBorderThickness = _chrome.ResizeBorderThickness,
                                  UseAeroCaptionButtons = _chrome.UseAeroCaptionButtons
                              };
                if (_chrome.CanFreeze)
                {
                    _chrome.Freeze();
                }
                WindowChrome.SetWindowChrome(this, _chrome);
            }
        }

        [SecuritySafeCritical]
        private static void OnWindowStateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var instance = (MetroWindow)obj;
            instance.OnWindowStateChanged();
        }

        [SecurityCritical]
        private void OnWindowStateChanged()
        {
            UpdateNonClientBorder();
        }

        [SecurityCritical]
        private void UpdateNonClientBorder()
        {
            if (!Equals(WindowChrome.GetWindowChrome(this), _chrome) || _layoutRoot == null || _window == null)
            {
                return;
            }

            Taskbar.Invalidate();
            if (WindowState == WindowState.Maximized && !Taskbar.AutoHide && SizeToContent == SizeToContent.Manual)
            {
                _window.Invalidate();
                _layoutRoot.Margin = new Thickness(_window.NonClientBorderWidth, _window.NonClientBorderHeight, _window.NonClientBorderWidth, _window.NonClientBorderHeight);
            }
            else
            {
                _layoutRoot.Margin = new Thickness();
            }
        }

        internal static bool IsNonNegative(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
        }
    }
}