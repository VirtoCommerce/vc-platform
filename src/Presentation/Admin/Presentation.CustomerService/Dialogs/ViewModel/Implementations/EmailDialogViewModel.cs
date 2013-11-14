using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
    public class EmailDialogViewModel : ViewModelBase, IEmailDialogViewModel
    {
        #region Fields



        #endregion


        #region Constructor

        public EmailDialogViewModel(Email item)
        {
            InnerItem = item;
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

       
        #endregion

        #region Properties

        public bool IsValid
        {
            get { return IsEmailValid(); }
        }

        #endregion

        #region IEmailDialogViewModel members

        private Email _innerItem;
        public Email InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Handlers

        void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsValid");
        }   


        #endregion

        #region Validation

        private bool IsEmailValid()
        {
            bool result = true;
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            result = InnerItem.Validate();
            InnerItem.PropertyChanged+=InnerItem_PropertyChanged;
            return result;
        }

        #endregion

    }
}
