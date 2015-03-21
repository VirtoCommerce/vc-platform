using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public interface IFoundationCustomerRepository : ICustomerRepository
    {
        Contact GetContact(string id);
    }
}
