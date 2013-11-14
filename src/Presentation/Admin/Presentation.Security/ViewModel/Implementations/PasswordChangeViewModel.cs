using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Implementations
{
    public class PasswordChangeViewModel : ViewModelBase, IPasswordChangeViewModel
    {
        #region Public Properties

        private string _Password1;
        public string Password1
        {
            get { return _Password1; }
            set { _Password1 = value; OnPropertyChanged(); OnPropertyChanged("IsValid"); }
        }

        public bool IsPasswordChanging { get; set; }

        #endregion

        public PasswordChangeViewModel(bool isPasswordChanging)
        {
            IsPasswordChanging = isPasswordChanging;
        }

        #region IPasswordChangeViewModel

        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(Password) && Password == Password1; }
        }

        private string _OldPassword;
        public string OldPassword
        {
            get { return _OldPassword; }
            set { _OldPassword = value; OnPropertyChanged(); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); OnPropertyChanged("IsValid"); }
        }
        #endregion
    }
}
