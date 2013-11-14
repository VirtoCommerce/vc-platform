#region Usings

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    /// <summary>
    /// A base class for view-models that are used in pair with the default 
    /// <see cref="NotificationDialog"/>'s <see cref="InteractionDialogAction"/>.
    /// </summary>
    public class NotificationDialogViewModelBase : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of the <see cref="NotificationDialogViewModelBase"/>.
        /// </summary>
        public NotificationDialogViewModelBase()
        {
            InitializeCommandsAndRequests();
        }
        #endregion

        #region Initialization Methods
        /// <summary>
        /// Performs initialization of commands and requests.
        /// </summary>
        private void InitializeCommandsAndRequests()
        {
            CloseCommand = new DelegateCommand(DoClose, CloseCanExecute);
            CloseRequest = new InteractionRequest<Notification>();
        }
        #endregion

        #region Commands & Requests
        /// <summary>
        /// Gets the command for close action.
        /// </summary>
        public DelegateCommand CloseCommand { get; private set; }

        /// <summary>
        /// Gets the request that triggers the close logic.
        /// </summary>
        public InteractionRequest<Notification> CloseRequest { get; private set; }

        #endregion

        #region Command Handlers
        /// <summary>
        /// Basic implementation of CloseCommand action. Can be overriden.
        /// </summary>
        protected virtual void DoClose()
        {
            CloseRequest.Raise(null);
        }

        /// <summary>
        /// CloseCommand execution predicate. Can be overriden.
        /// </summary>
        /// <returns>A value that indicates whether the CloseCommand can execute.</returns>
        protected virtual bool CloseCanExecute()
        {
            return true;
        }
        #endregion
    }
}
