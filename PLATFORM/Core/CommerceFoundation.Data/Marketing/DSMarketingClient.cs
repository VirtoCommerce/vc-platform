using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Repositories;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Marketing
{
	public class DSMarketingClient : DSClientBase, IMarketingRepository
	{
		[InjectionConstructor]
        public DSMarketingClient(IMarketingEntityFactory entityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(MarketingConfiguration.Instance.Connection.DataServiceUri), entityFactory, tokenInjector)
		{
		}

        public DSMarketingClient(Uri serviceUri, IMarketingEntityFactory entityFactory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, entityFactory, tokenInjector)
		{
		}

		#region IMarketingRepository Members

		public IQueryable<Promotion> Promotions
		{
			get { return GetAsQueryable<Promotion>(); }
		}

		public IQueryable<PromotionReward> PromotionRewards
		{
			get { return GetAsQueryable<PromotionReward>(); }
		}

		public IQueryable<CouponSet> CouponSets
		{
			get { return GetAsQueryable<CouponSet>(); }
		}

		public IQueryable<Coupon> Coupons
		{
			get { return GetAsQueryable<Coupon>(); }
		}

        public IQueryable<PromotionUsage> PromotionUsages
        {
            get { return GetAsQueryable<PromotionUsage>(); }
        }

		#endregion
	}
}
