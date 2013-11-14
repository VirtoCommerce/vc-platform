using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.Fulfillment;

namespace VirtoCommerce.Foundation.Orders.Repositories
{
	public interface IFulfillmentRepository : IRepository
	{
		IQueryable<Picklist> Picklists { get; }
    }
}
