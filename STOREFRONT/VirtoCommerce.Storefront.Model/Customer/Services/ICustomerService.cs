using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Customer.Services
{
    public interface ICustomerService
    {
        Task<CustomerInfo> GetCustomerByIdAsync(string customerId);
        Task CreateCustomerAsync(CustomerInfo customer);
        Task UpdateCustomerAsync(CustomerInfo customer);
        Task<bool> CanLoginOnBehalfAsync(string storeId, string customerId);
    }
}
