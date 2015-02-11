using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.PricingModule.Data.Repositories
{
	public class FoundationPricingRepositoryImpl : EFCatalogRepository, IFoundationPricingRepository
	{
		public FoundationPricingRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationPricingRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		#region IFoundationPricingRepository Members

		public Price GetPriceById(string priceId)
		{
			var retVal = Prices.Include(x => x.Pricelist).FirstOrDefault(x => x.PriceId == priceId);
			return retVal;
		}

		public Pricelist GetPriceListById(string priceListId)
		{
			var retVal = Pricelists.Include(x => x.Prices).FirstOrDefault(x => x.PricelistId == priceListId);
			return retVal;
		}

		#endregion
	}

}
