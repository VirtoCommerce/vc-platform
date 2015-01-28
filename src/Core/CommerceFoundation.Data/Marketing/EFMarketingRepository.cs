using System.Linq;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing;
using System.Data.Entity.ModelConfiguration.Conventions;
using VirtoCommerce.Foundation.Marketing.Model;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Marketing
{
    public class EFMarketingRepository : EFRepositoryBase, IMarketingRepository
	{
        public EFMarketingRepository()
        {
        }

        public EFMarketingRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFMarketingRepository>(null);
        }

        [InjectionConstructor]
        public EFMarketingRepository(IMarketingEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(MarketingConfiguration.Instance.Connection.SqlConnectionStringName, factory: entityFactory, interceptors: interceptors)
		{
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(new ValidateDatabaseInitializer<EFMarketingRepository>());
		}

		public EFMarketingRepository(string connectionStringName, IMarketingEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(connectionStringName, entityFactory, interceptors: interceptors)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<EFMarketingRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			TPHMaping(modelBuilder);

			MapEntity<Coupon>(modelBuilder, toTable: "Coupon");
			MapEntity<CouponSet>(modelBuilder, toTable: "CouponSet");
			MapEntity<Segment>(modelBuilder, toTable: "Segment");
			MapEntity<SegmentSet>(modelBuilder, toTable: "SegmentSet");
            MapEntity<PromotionUsage>(modelBuilder, toTable: "PromotionUsage");

			base.OnModelCreating(modelBuilder);
		}

		private void TPHMaping(DbModelBuilder modelBuilder)
		{
			#region Promotion
			MapEntity<Promotion>(modelBuilder, toTable: "Promotion");
			MapEntity<CartPromotion>(modelBuilder, toTable: "Promotion", discriminatorValue: "CartPromotion");
			MapEntity<CatalogPromotion>(modelBuilder, toTable: "Promotion", discriminatorValue: "CatalogPromotion");
			#endregion
			#region Rewards
			MapEntity<PromotionReward>(modelBuilder, toTable: "PromotionReward");
			MapEntity<CartSubtotalReward>(modelBuilder, toTable: "PromotionReward", discriminatorValue: "CartSubtotalReward");
			MapEntity<ShipmentReward>(modelBuilder, toTable: "PromotionReward", discriminatorValue: "ShipmentReward");
			MapEntity<CatalogItemReward>(modelBuilder, toTable: "PromotionReward", discriminatorValue: "CatalogItemReward");
			#endregion
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
