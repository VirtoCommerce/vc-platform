using VirtoCommerce.Foundation.Data.Infrastructure;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Data.Orders
{
	[JsonSupportBehavior]
	public class DSOrderService : DServiceBase<EFOrderRepository>
	{
	}
}
