using System;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Presentation.Core.Infrastructure;

namespace Presentation.Order.ViewModel
{
    /// <summary>
    /// base view model for dialog based form. Implements ICancellable and ISupportAcceptChanges.
    /// </summary>
    public abstract class AddressBaseViewModel : ViewModelBase, ISupportAcceptChanges
    {
        #region constructor
        public AddressBaseViewModel(IView view)
        {
            this.View = view;
            this.View.Model = this;

            CancelConfirmRequest = new InteractionRequest<Confirmation>();
            CancelCommand = new DelegateCommand<object>((x) => RaiseCancelInteractionRequest());
        }

        #endregion
        
        #region ISupportAcceptChanges Members

		public InteractionRequest<Confirmation> CancelConfirmRequest { get; protected set; }
		public DelegateCommand<object> CancelCommand { get; protected set; }

        public DelegateCommand<object> SaveChangesCommand
        {
            get;
            protected set;
        }

        private bool _isModified;
        public bool IsModified
        {
            get
            {
                return _isModified;
            }

            protected set
            {
                _isModified = value;
                OnPropertyChanged("IsModified");
            }
        }
        #endregion

        #region Private methods

        private void RaiseCancelInteractionRequest()
        {
        }
        #endregion

    }
}
