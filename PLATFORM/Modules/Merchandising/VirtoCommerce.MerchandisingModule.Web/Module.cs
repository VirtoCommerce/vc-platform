using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Foundation;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Foundation.Assets.Services;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Marketing;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Caching;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.MerchandisingModule.Web.Controllers;
using VirtoCommerce.MerchandisingModule.Web.Services;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : IModule, IDatabaseModule
    {
        #region Constants

        private const string _connectionStringName = "VirtoCommerce";

        #endregion

        #region Fields

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors and Destructors

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods and Operators

        public void Initialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            var cacheRepository = _container.Resolve<ICacheRepository>();

            var cacheSettings = new[] 
			{
				new CacheSettings(Constants.CatalogCachePrefix + ".GetByIds", TimeSpan.FromMinutes(settingsManager.GetValue("Catalogs.Caching.ItemTimeout", 30)), "", true),
				new CacheSettings(Constants.CatalogCachePrefix + ".GetCatalogById", TimeSpan.FromMinutes(settingsManager.GetValue("Catalogs.Caching.CatalogTimeout", 60)), "", true),
				new CacheSettings(Constants.CatalogCachePrefix + ".Search", TimeSpan.FromMinutes(settingsManager.GetValue("Catalogs.Caching.SearchTimeout", 30)), "", true)
			};

            var cacheManager = new CacheManager(x => cacheRepository, x => cacheSettings.FirstOrDefault(y => y.Group == x));

            Func<IFoundationCatalogRepository> catalogRepFactory =
                () => new FoundationCatalogRepositoryImpl(_connectionStringName);
            Func<IFoundationAppConfigRepository> appConfigRepFactory =
                () => new FoundationAppConfigRepositoryImpl(_connectionStringName);

            var catalogService = new CatalogServiceImpl(catalogRepFactory, cacheManager);
            var propertyService = new PropertyServiceImpl(catalogRepFactory, cacheManager);
            var categoryService = new CategoryServiceImpl(catalogRepFactory, appConfigRepFactory, cacheManager);
            var itemService = new ItemServiceImpl(
                catalogRepFactory,
                appConfigRepFactory,
                cacheManager);
            var itemSearchService = new CatalogSearchServiceImpl(
                catalogRepFactory,
                itemService,
                catalogService,
                categoryService,
                cacheManager);

            #region VCF dependencies

            var searchConnection = _container.Resolve<ISearchConnection>();
            var searchProvider = _container.Resolve<ISearchProvider>();

            Func<IReviewRepository> reviewRepFactory = () => new EFReviewRepository(_connectionStringName);
            Func<IStoreRepository> storeRepFactory = () => new EFStoreRepository(_connectionStringName);

            #endregion

            #region Dynamic content

            Func<IDynamicContentRepository> dynamicRepositoryFactory =
                () => new EFDynamicContentRepository(_connectionStringName);
            Func<IDynamicContentEvaluator> dynamicContentEval =
                () => new DynamicContentEvaluator(dynamicRepositoryFactory(), null, cacheRepository);
            Func<IDynamicContentService> dynamicContentServiceFactory =
                () => new DynamicContentService(dynamicRepositoryFactory(), dynamicContentEval());

            #endregion

            #region Pricelists

            Func<IPricelistRepository> priceListRepositoryFactory = () => new EFCatalogRepository(_connectionStringName);
            Func<IPriceListAssignmentEvaluator> priceListEvalFactory =
                () => new PriceListAssignmentEvaluator(priceListRepositoryFactory(), null, cacheRepository);

            #endregion

            var blobStorageProvider = _container.Resolve<IBlobStorageProvider>();
            var assetUrl = _container.Resolve<IAssetUrl>();

            var itemBrowseService = new ItemBrowsingService(
                itemService,
                catalogRepFactory,
                searchProvider,
                cacheRepository,
                assetUrl,
                searchConnection);
            var filterService = new FilterService(storeRepFactory, catalogRepFactory, new HttpCacheRepository());

            Func<ICatalogOutlineBuilder> catalogOutlineBuilderFactory =
                () => new CatalogOutlineBuilder(catalogRepFactory(), cacheRepository);

            _container.RegisterType<ReviewController>(new InjectionConstructor(reviewRepFactory));
            _container.RegisterType<ProductController>(
                new InjectionConstructor(
                    itemService,
                    itemBrowseService,
                    filterService,
                    catalogRepFactory,
                    appConfigRepFactory,
                    catalogOutlineBuilderFactory,
                    storeRepFactory,
                    blobStorageProvider,
                    settingsManager,
                    cacheRepository));
            _container.RegisterType<ContentController>(new InjectionConstructor(dynamicContentServiceFactory));
            _container.RegisterType<CategoryController>(
                new InjectionConstructor(
                    itemSearchService,
                    categoryService,
                    propertyService,
                    catalogRepFactory,
                    appConfigRepFactory,
                    storeRepFactory,
                    settingsManager,
                    cacheRepository));
            _container.RegisterType<StoreController>(
                new InjectionConstructor(storeRepFactory, appConfigRepFactory, settingsManager, cacheRepository));
            _container.RegisterType<KeywordController>(new InjectionConstructor(appConfigRepFactory));
            _container.RegisterType<PriceController>(
                new InjectionConstructor(
                    storeRepFactory,
                    priceListRepositoryFactory,
                    priceListEvalFactory,
                    new PriceListAssignmentEvaluationContext(),
                    settingsManager,
                    cacheRepository));
            _container.RegisterType<IAssetService, AssetService>();
            _container.RegisterType<IItemBrowsingService, ItemBrowsingService>();

            //Register prmotion evaluation policies
            /*
            _container.RegisterType<IEvaluationPolicy, GlobalExclusivityPolicy>("global");
            _container.RegisterType<IEvaluationPolicy, GroupExclusivityPolicy>("group");
            _container.RegisterType<IEvaluationPolicy, CartSubtotalRewardCombinePolicy>("cart");
            _container.RegisterType<IEvaluationPolicy, ShipmentRewardCombinePolicy>("shipment");
             * */
        }

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var db = new EFReviewRepository(_connectionStringName))
            {
                SqlReviewDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlReviewSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlReviewDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFStoreRepository(_connectionStringName))
            {
                SqlStoreDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlStoreSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlStoreDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFMarketingRepository(_connectionStringName))
            {
                SqlPromotionDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlPromotionSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlPromotionDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFDynamicContentRepository(_connectionStringName))
            {
                SqlDynamicContentDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlDynamicContentSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlDynamicContentDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }
        }

        #endregion
    }
}
