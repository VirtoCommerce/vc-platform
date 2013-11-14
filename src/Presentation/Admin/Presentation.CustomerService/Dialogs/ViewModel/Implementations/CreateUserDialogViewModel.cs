using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.Model.Enumerations;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
    public interface ICreateUserDialogViewModel:IViewModel 
    {
        string StoreId { get; }
        string UserName { get; }
        string Password { get; }
    }


    public class CreateUserDialogViewModel : ViewModelBase, ICreateUserDialogViewModel
    {

        #region Fields

        private readonly IStoreRepository _storeRepository;

        #endregion

        #region Constructor

		public CreateUserDialogViewModel(CreateUserDialogOperationType operationType, IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
            OperationType = operationType;

            InitStoreList();
            OnPropertyChanged("IsValid");
        }

        private async void InitStoreList()
        {
            StoreList = await Task.Run(() =>
                {
                    if (_storeRepository != null)
                    {
                        return _storeRepository.Stores.ToList();
                    }
                    else
                    {
                        return new List<Store>();
                    }
                });

        }

        #endregion

        #region PublicProperties

        private List<Store> _storeList;
        public List<Store> StoreList
        {
            get { return _storeList; }
            set
            {
                _storeList = value;
                OnPropertyChanged();
            }
        }

        private CreateUserDialogOperationType _operationType;
        public CreateUserDialogOperationType OperationType
        {
            get { return _operationType; }
            set { _operationType = value;
            OnPropertyChanged();}
        }


        public bool IsValid
        {
            get
            {
                bool result = false;
                if (OperationType == CreateUserDialogOperationType.ResetPassword)
                {
                    result = !string.IsNullOrEmpty(Password);
                }
                else
                {
                    result =!string.IsNullOrEmpty(StoreId) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
                }
                return result;
            }
        }

        #endregion

        #region ICreateUserDialogViewModel

        private string _storeId;
        public string StoreId
        {
            get { return _storeId; }
            set { _storeId = value;
            OnPropertyChanged();
            OnPropertyChanged("IsValid");}
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }

        #endregion

    }
}
