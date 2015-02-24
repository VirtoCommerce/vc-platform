using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Fulfillment.Model;

namespace VirtoCommerce.Domain.Fulfillment.Services
{
	public interface IFulfillmentService
	{
		IEnumerable<FulfillmentCenter> GetAllFulfillmentCenters();
		FulfillmentCenter UpsertFulfillmentCenter(FulfillmentCenter fullfilmentCenter);
		void DeleteFulfillmentCenter(string[] ids);

	}
}
