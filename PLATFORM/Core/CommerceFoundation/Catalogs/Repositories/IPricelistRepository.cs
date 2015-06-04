using System.Collections.Generic;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Catalogs.Model;
using System.ServiceModel;
using VirtoCommerce.Foundation.Frameworks;
using System.Linq;

namespace VirtoCommerce.Foundation.Catalogs.Repositories
{
	public interface IPricelistRepository : IRepository
	{
		IQueryable<Pricelist> Pricelists { get; }
		IQueryable<Price> Prices { get; }
		IQueryable<PricelistAssignment> PricelistAssignments { get; }
    
    }
}
