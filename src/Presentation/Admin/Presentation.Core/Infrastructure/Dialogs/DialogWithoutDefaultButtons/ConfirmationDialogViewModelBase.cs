#region Usings

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

#endregion

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs.DialogWithoutDefaultButtons
{
    /// <summary>
    /// A base class for view-models that are used in pair with the default 
    /// <see cref="InteractionDialogAction"/>'s <see cref="ConfirmationDialog"/>.
    /// </summary>
    public class ConfirmationDialogViewModelBase : ViewModelBase
    {
        #region Constructors

        /// <summary>
        /// Creates new instance of the <see cref="ConfirmationDialogViewModelBase"/>.
        /// </summary>
        protected ConfirmationDialogViewModelBase()
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
            ConfirmCommand = new DelegateCommand(DoConfirm, CanConfirm);
            CancelCommand = new DelegateCommand(DoCancel, CanCancel);
            ConfirmationRequest = new InteractionRequest<Notification>();
            CancellationRequest = new InteractionRequest<Notification>();
        }

        #endregion

        #region Commands & Requests

        /// <summary>
        /// Gets the command for confirmation action.
        /// </summary>
        public DelegateCommand ConfirmCommand { get; private set; }

        /// <summary>
        /// Gets the command for cancellation action.
        /// </summary>
        public DelegateCommand CancelCommand { get; private set; }

        /// <summary>
        /// Gets the request that triggers the confirmation logic.
        /// </summary>
        public InteractionRequest<Notification> ConfirmationRequest { get; private set; }

        /// <summary>
        /// Gets the request that triggers the cancellation logic.
        /// </summary>
        public InteractionRequest<Notification> CancellationRequest { get; private set; }

        #endregion

        #region Command Handlers

        #region Confirm

        /// <summary>
        /// Basic implementation of the ConfirmCommand action. Can be overriden.
        /// </summary>
        protected virtual void DoConfirm()
        {
            ConfirmationRequest.Raise(null);
        }

        /// <summary>
        /// ConfirmCommand execution predicate. Can be overriden.
        /// </summary>
        /// <returns>A value that indicates whether the ConfirmCommand can execute.</returns>
        protected virtual bool CanConfirm()
        {
            return true;
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Basic implementation of the CancelCommand action. Can be overriden.
        /// </summary>
        protected virtual void DoCancel()
        {
            CancellationRequest.Raise(null);
        }

        /// <summary>
        /// CancelCommand execution predicate. Can be overriden.
        /// </summary>
        /// <returns>A value that indicates whether the CancelCommand can execute.</returns>
        protected virtual bool CanCancel()
        {
            return true;
        }

        #endregion

        #endregion
    }
}
