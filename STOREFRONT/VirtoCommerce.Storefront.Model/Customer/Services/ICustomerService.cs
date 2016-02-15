using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Customer.Services
{
    public interface ICustomerService
    {
        Task<CustomerInfo> GetCustomerByIdAsync(string customerId);
        Task CreateCustomerAsync(CustomerInfo customer);
        Task UpdateCustomerAsync(CustomerInfo customer);
    }
}
