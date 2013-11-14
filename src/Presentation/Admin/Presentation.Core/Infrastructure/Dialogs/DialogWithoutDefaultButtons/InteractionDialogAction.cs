#region Usings

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    public class InteractionDialogAction : InteractionDialogActionBase
    {
        #region Dependency Properties

        public static readonly DependencyProperty DialogWindowContentTypeProperty =
            DependencyProperty.Register(
                "DialogWindowContentType",
                typeof (Type),
                typeof (InteractionDialogAction),
                new PropertyMetadata(null));

        public static readonly DependencyProperty DialogWindowWidthProperty =
            DependencyProperty.Register(
                "DialogWindowWidth",
                typeof (double),
                typeof (InteractionDialogAction),
                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty DialogWindowHeightProperty =
            DependencyProperty.Register(
                "DialogWindowHeight",
                typeof (double),
                typeof (InteractionDialogAction),
                new PropertyMetadata(default(double)));

        public static readonly DependencyProperty IsResizableProperty =
            DependencyProperty.Register(
                "IsResizable",
                typeof (bool),
                typeof (InteractionDialogAction),
                new PropertyMetadata(true));

        #endregion

        #region Public Properties

        public Type DialogWindowContentType
        {
            get { return (Type) GetValue(DialogWindowContentTypeProperty); }
            set { SetValue(DialogWindowContentTypeProperty, value); }
        }

        public double DialogWindowWidth
        {
            get { return (double) GetValue(DialogWindowWidthProperty); }
            set { SetValue(DialogWindowWidthProperty, value); }
        }

        public double DialogWindowHeight
        {
            get { return (double) GetValue(DialogWindowHeightProperty); }
            set { SetValue(DialogWindowHeightProperty, value); }
        }

        public bool IsResizable
        {
            get { return (bool) GetValue(IsResizableProperty); }
            set { SetValue(IsResizableProperty, value); }
        }

        #endregion

        #region InteractionDialogBase Overrides

        protected override Window GetInteractionDialog(Notification notification)
        {
            var window = GetDefaultWindow(notification);

            window.DataContext = notification;
            window.SizeToContent = SizeToContent.Manual;

            if (DialogWindowWidth > 0) window.Width = DialogWindowWidth;
            if (DialogWindowHeight > 0) window.Height = DialogWindowHeight;

            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            if (!IsResizable)
            {
                window.ResizeMode = ResizeMode.NoResize;
            }

            return window;
        }

        #endregion

        #region Auxiliary Methods

        private Window GetDefaultWindow(Notification notification)
        {
            var window = notification is Confirmation
                             ? (InteractionDialogWindow) Activator.CreateInstance<ConfirmationDialog>()
                             : Activator.CreateInstance<NotificationDialog>();

            window.DialogWindowContent = Activator.CreateInstance(DialogWindowContentType);

            return window;
        }

        private void TitleMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = GetParentWindow(sender as DependencyObject) as Window;

            if (window != null)
                window.DragMove();
        }

        private object GetParentWindow(DependencyObject obj)
        {
            var window = VisualTreeHelper.GetParent(obj) as Window;

            return window ?? GetParentWindow(VisualTreeHelper.GetParent(obj));
        }

        #endregion
    }
}
