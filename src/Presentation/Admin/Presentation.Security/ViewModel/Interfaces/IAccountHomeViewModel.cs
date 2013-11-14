using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
    public interface IAccountHomeViewModel : IViewModel
    {
        string SearchKeyword { get; }
    }
}
