using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    public interface IRefreshItemList
    {

        DelegateCommand RefreshItemListCommand { get; }

    }
}
