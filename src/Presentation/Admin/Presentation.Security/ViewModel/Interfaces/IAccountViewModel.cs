using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
    public interface IAccountViewModel: IViewModel
    {
        Account InnerItem { get; }
    }
}
