using VirtoCommerce.Foundation.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Data.Orders
{
	[JsonSupportBehavior]
	public class DSOrderService : DServiceBase<EFOrderRepository>
	{
		protected override EFOrderRepository CreateDataSource()
		{
            return ServiceLocator.Current.GetInstance<IOrderRepository>() as EFOrderRepository;
		}
	}
}
