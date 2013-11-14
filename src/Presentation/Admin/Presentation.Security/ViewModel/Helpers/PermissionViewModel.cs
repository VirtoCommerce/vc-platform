using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Helpers
{
    public class PermissionViewModel:ViewModelBase
    {

        #region Fields

        private Permission _innerItem;
        private PermissionGroupViewModel _parentViewModel;

        #endregion

        public PermissionViewModel(Permission innerItem, PermissionGroupViewModel parentViewModel)
        {
            InnerItem = innerItem;
            Parent = parentViewModel;
        }

        #region Properties

        public Permission InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        public PermissionGroupViewModel Parent
        {
            get { return _parentViewModel; }
            set
            {
                _parentViewModel = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
                Parent.RefreshIsVisibleProperty();
            }
        }

        #endregion
    }
}
