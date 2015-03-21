using VirtoCommerce.Foundation.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Customers.Repositories;

namespace VirtoCommerce.Foundation.Data.Customers
{
    [JsonSupportBehavior]
    public class DSCustomerService : DServiceBase<EFCustomerRepository>
	{
	}
}
