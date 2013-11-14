using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
    public class CreateCustomerDialogViewModel : ViewModelBase, ICreateCustomerDialogViewModel
    {
        #region Constructor

        public CreateCustomerDialogViewModel(Contact item)
        {
            InnerItem = item;
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;

            EmailForUserInput=new Email();
            PhoneForUserInput = new Phone();
        }


        #endregion

        #region Public Properties

        private Email _emailForUserInput;
        public Email EmailForUserInput
        {
            get { return _emailForUserInput; }
            set { _emailForUserInput = value;
            OnPropertyChanged();}
        }

        private Phone _phoneForUserInput;
        public Phone PhoneForUserInput
        {
            get { return _phoneForUserInput; }
            set { _phoneForUserInput = value;
            OnPropertyChanged();}
        }


        public bool IsValid
        {
            get { return IsCustomerValid(); }
        }

        #endregion

        #region Private method

        private bool IsCustomerValid()
        {
            return InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.FullName);
        }

        #endregion

        #region ICustomerChoiceDialogViewModel

        private Contact _innerItem;
        public Contact InnerItem
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

        private void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            OnPropertyChanged("IsValid");
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        #endregion

    }
}
