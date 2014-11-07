using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;

namespace VirtoCommerce.SecurityModule.Web.Data
{
    public interface IFoundationCustomerRepository : ICustomerRepository
    {
        Contact GetContact(string id);
    }
}
