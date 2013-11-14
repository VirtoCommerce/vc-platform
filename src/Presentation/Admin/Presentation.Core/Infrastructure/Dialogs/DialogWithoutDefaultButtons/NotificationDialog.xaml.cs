#region Usings

using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    /// <summary>
    /// Interaction logic for NotificationDialog.xaml
    /// This dialog is used as a default interaction dialog for <see cref="Notification"/> notification
    /// when used in conjunction with <see cref="InteractionDialogAction"/>.
    /// </summary>
    public partial class NotificationDialog
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of the <see cref="NotificationDialog"/>.
        /// </summary>
        public NotificationDialog()
        {
            InitializeComponent();
        }
        #endregion
    }
}
