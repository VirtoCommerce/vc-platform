using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Data;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Orders;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Stores.Factories;

namespace UI.FunctionalTests.Helpers.Common
{

	[JsonSupportBehavior]
	public class TestDSCatalogService : DSCatalogService
	{
		public const string DatabaseName = "CatalogTest";

        protected override EFCatalogRepository CreateRepository()
		{
			return new EFCatalogRepository(DatabaseName, new CatalogEntityFactory());
		}
	}

	[JsonSupportBehavior]
	public class TestDSAppConfigService : DSAppConfigService
	{
		public const string DatabaseName = "AppConfigTest";

		protected override EFAppConfigRepository CreateRepository()
		{
			return new EFAppConfigRepository(DatabaseName, new AppConfigEntityFactory());
		}
	}
	
	[JsonSupportBehavior]
	public class TestDSOrderService : DSOrderService
	{
		public const string DatabaseName = "OrderTest";

        protected override EFOrderRepository CreateRepository()
		{
			return new EFOrderRepository(DatabaseName, new OrderEntityFactory());
		}
	}

	[JsonSupportBehavior]
	public class TestDSStoreService : DSStoreService
	{
		public const string DatabaseName = "StoreTest";

        protected override EFStoreRepository CreateRepository()
		{
			return new EFStoreRepository(DatabaseName, new StoreEntityFactory());
		}
	}

	[JsonSupportBehavior]
	public class TestDSDynamicContentService : DSDynamicContentService
	{
		public const string DatabaseName = "DynamicContentTest";

        protected override EFDynamicContentRepository CreateRepository()
		{
			return new EFDynamicContentRepository(DatabaseName, new DynamicContentEntityFactory());
		}
	}

	[JsonSupportBehavior]
	public class TestDSMarketingService : DSMarketingService
	{
		public const string DatabaseName = "MarketingTest";

        protected override EFMarketingRepository CreateRepository()
		{
			return new EFMarketingRepository(DatabaseName, new MarketingEntityFactory());
		}
	}

}
