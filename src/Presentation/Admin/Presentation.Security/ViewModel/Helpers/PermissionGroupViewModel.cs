using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.Model;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Helpers
{
    public class PermissionGroupViewModel:ViewModelBase
    {

        #region Fields

        private PermissionGroup _innerItem;

        #endregion

        public PermissionGroupViewModel(PermissionGroup innerItem)
        {
            InnerItem = innerItem;

            foreach (var permission in InnerItem.AllAvailablePermissions)
            {
                Permissions.Add(new PermissionViewModel(permission,this));
            }

            OnPropertyChanged("IsVisible");
        }


        #region Properties

        public PermissionGroup InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        public bool IsVisible
        {
            get
            {
                var result = true;

                if (Permissions != null)
                {
                    result = Permissions.Any(pvm => pvm.IsVisible);
                }

                return result;
            }
        }

        public bool IsVisibleInCurrentList
        {
            get
            {
                var result = true;

                if (Permissions != null)
                {
                    result = Permissions.All(pvm => !pvm.IsVisible);
                }

                return result;
            }
        }

        private List<PermissionViewModel> _permissions=new List<PermissionViewModel>();
        public List<PermissionViewModel> Permissions
        {
            get { return _permissions; }
            set
            {
                _permissions = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        public void RefreshIsVisibleProperty()
        {
            OnPropertyChanged("IsVisible");
        }

        #endregion

    }
}
