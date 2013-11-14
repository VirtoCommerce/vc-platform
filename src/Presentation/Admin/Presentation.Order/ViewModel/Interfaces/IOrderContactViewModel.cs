using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Interfaces
{
    public interface IOrderContactViewModel : IViewModel
    {
        string CustomerId { get; }
        string FullName { get; }
        bool Validate();
    }
}
