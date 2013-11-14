#region Usings

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    /// <summary>
    /// Interaction logic for ConfirmationDialog.xaml.
    /// This dialog is used as a default interaction dialog for <see cref="Confirmation"/> notification
    /// when used in conjunction with <see cref="InteractionDialogAction"/>.
    /// </summary>
    public partial class ConfirmationDialog
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of the <see cref="ConfirmationDialog"/>.
        /// </summary>
        public ConfirmationDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Dependency Properties
        /// <summary>
        /// This property is used to indiciate whether the default window closing behaviour should be
        /// overriden.
        /// </summary>
        public static readonly DependencyProperty WindowShouldCloseProperty =
            DependencyProperty.Register(
                "WindowShouldClose", 
                typeof (bool), 
                typeof (ConfirmationDialog), 
                new PropertyMetadata(default(bool)));

        /// <summary>
        /// This property is used to trigger the additional actions to perform upon window close request.
        /// </summary>
        public static readonly DependencyProperty IsWindowCloseRequestedProperty =
            DependencyProperty.Register(
                "IsWindowCloseRequested", 
                typeof(bool), 
                typeof(ConfirmationDialog),
                new PropertyMetadata(default(bool)));
        #endregion

        #region Window Overrides
        protected override void OnClosing(CancelEventArgs e)
        {
            // Default window closing behaviour is used when WindowShouldClose == true.
            if (WindowShouldClose) return;

            // Prevent window from closing.
            e.Cancel = true;

            // The close window property trigger is handled in XAML. Perform all actions in the UI thread.
            Task.Factory.StartNew(
                () =>
                {
                    IsWindowCloseRequested = false; // Ensure that property changed trigger will fire.
                    IsWindowCloseRequested = true;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets value that indicates whether the window should close normally or
        /// perform some additional actions otherwise.
        /// </summary>
        public bool WindowShouldClose
        {
            get { return (bool) GetValue(WindowShouldCloseProperty); }
            set { SetValue(WindowShouldCloseProperty, value); }
        }

        /// <summary>
        /// Gets or sets value that indicates whether a window close logic is requested.
        /// </summary>
        public bool IsWindowCloseRequested
        {
            get { return (bool)GetValue(IsWindowCloseRequestedProperty); }
            set { SetValue(IsWindowCloseRequestedProperty, value); }
        }
        #endregion
    }
}
