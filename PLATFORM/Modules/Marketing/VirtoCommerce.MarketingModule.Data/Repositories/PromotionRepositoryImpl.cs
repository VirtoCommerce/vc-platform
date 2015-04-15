using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Repositories
{
	public class PromotionRepositoryImpl : EFMarketingRepository, IFoundationPromotionRepository
	{
		public PromotionRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null, null)
		{
		}
		public PromotionRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{
		}

		#region IFoundationMarketingRepository Members

		public Foundation.Marketing.Model.Promotion GetPromotionById(string id)
		{
			var retVal = Promotions.Include(x=>x.Coupon).FirstOrDefault(x => x.PromotionId == id);
			return retVal;
		}

		public Foundation.Marketing.Model.Promotion[] GetActivePromotions()
		{
			var now = DateTime.UtcNow;
			var retVal = Promotions.Where(x => x.Status == "Active" && (x.StartDate == null || now >= x.StartDate) && (x.EndDate == null || x.EndDate >= now))
											 .OrderByDescending(x => x.Priority).ToArray();
			return retVal;
		}

		#endregion
	}

}
