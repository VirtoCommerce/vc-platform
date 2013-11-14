using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.ViewModel.Helpers;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
	public interface IRoleViewModel : IViewModelDetailBase
    {
        Role InnerItem { get; }
		PermissionGroupViewModel[] AllAvailablePermissionGroupViewModels { get; }
	    PermissionGroupViewModel[] CurrentPermissionGroupViewModels { get; }
		
		DelegateCommand<object> SelectItemCommand { get; }
		DelegateCommand SelectAllItemsCommand { get; }
		DelegateCommand<object> UnSelectItemCommand { get; }
		DelegateCommand UnSelectAllItemsCommand { get; }

    }
}
