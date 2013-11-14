using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Stores.Services;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Stores.Repositories
{
	public interface IFulfillmentCenterRepository : IRepository
    {
		IQueryable<FulfillmentCenter> FulfillmentCenters { get; }
    }
}
