using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
    public class PhoneNumberDialogViewModel : ViewModelBase, IPhoneNumberDialogViewModel
    {

        #region Fields



        #endregion

        #region Constructor

        public PhoneNumberDialogViewModel(Phone item)
        {
            InnerItem = item;
            InnerItem.PropertyChanged+=InnerItem_PropertyChanged;
        }


        #endregion

        #region Properties

        public bool IsValid
        {
            get { return IsPhoneValid(); }
        }

        #endregion

        #region IPhoneNumberDialogViewModel Members

        private Phone _innerItem;
        public Phone InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }
    

        #endregion

        #region Validation

        private bool IsPhoneValid()
        {
            bool result = true;
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            result = InnerItem.Validate();
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
            return result;
        }

        #endregion

        #region Handlers

        void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsValid");
        }

        #endregion
    }
}
