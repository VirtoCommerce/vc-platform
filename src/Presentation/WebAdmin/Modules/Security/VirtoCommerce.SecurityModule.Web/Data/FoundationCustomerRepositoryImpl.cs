using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Security.Repositories;

namespace VirtoCommerce.SecurityModule.Web.Data
{
    public class FoundationCustomerRepositoryImpl : EFCustomerRepository, IFoundationCustomerRepository
    {
        public FoundationCustomerRepositoryImpl()
            : this("VirtoCommerce")
        {
        }
        public FoundationCustomerRepositoryImpl(string nameOrConnectionString)
            : this(nameOrConnectionString, null)
        {
        }
        public FoundationCustomerRepositoryImpl(string nameOrConnectionString, IInterceptor[] interceptors = null)
            : base(nameOrConnectionString, null, interceptors)
        {
        }
        public Contact GetContact(string id)
        {
            return Members.OfType<Contact>().FirstOrDefault(x => x.MemberId == id);
        }
    }
}
