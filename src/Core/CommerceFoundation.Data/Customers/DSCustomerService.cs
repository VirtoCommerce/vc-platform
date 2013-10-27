using VirtoCommerce.Foundation.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Customers.Repositories;

namespace VirtoCommerce.Foundation.Data.Customers
{
    [JsonSupportBehavior]
    public class DSCustomerService : DServiceBase<ICustomerRepository>
	{
        protected override ICustomerRepository CreateDataSource()
		{
            return ServiceLocator.Current.GetInstance<ICustomerRepository>() as ICustomerRepository;
			//return new EFCustomerRepository(new CustomerEntityFactory());
		}
	}
}
