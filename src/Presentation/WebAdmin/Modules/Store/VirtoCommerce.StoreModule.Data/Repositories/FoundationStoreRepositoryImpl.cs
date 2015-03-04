using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;

namespace VirtoCommerce.StoreModule.Data.Repositories
{
	public class FoundationStoreRepositoryImpl : EFStoreRepository, IFoundationStoreRepository
	{
		public FoundationStoreRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationStoreRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		#region IFoundationStoreRepository Members

		public Foundation.Stores.Model.Store GetStoreById(string id)
		{
			var retVal = Stores.Where(x => x.StoreId == id).Include(x => x.Settings)
														 .Include(x => x.ReturnsFulfillmentCenter)
														 .Include(x => x.Languages)
														 .Include(x => x.Currencies)
														 .Include(x=> x.PaymentGateways)
														 .Include(x => x.FulfillmentCenter);
			return retVal.FirstOrDefault();
		}

		#endregion

		
	}

}
