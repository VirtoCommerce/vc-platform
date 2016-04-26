using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using CacheManager.Core;
using CacheManager.Web;
using MarkdownSharp;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Newtonsoft.Json;
using NLog;
using Owin;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.LiquidThemeEngine.Binders;
using VirtoCommerce.Storefront;
using VirtoCommerce.Storefront.App_Start;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Controllers;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.LinkList.Services;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Order.Events;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Model.Quote.Events;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Owin;
using VirtoCommerce.Storefront.Services;

[assembly: OwinStartup(typeof(Startup))]
[assembly: PreApplicationStartMethod(typeof(Startup), "PreApplicationStart")]
namespace VirtoCommerce.Storefront
{
    public class Startup
    {
        private static readonly List<string> _directories = new List<string>(new[] { Path.Combine(HostingEnvironment.MapPath("~/App_Data"), Environment.Is64BitProcess ? "x64" : "x86") });
        private static Assembly _managerAssembly;

        public static void PreApplicationStart()
        {
            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));

            AppDomain.CurrentDomain.AssemblyResolve += Resolve;

            var managerAssemblyPath = HostingEnvironment.MapPath("~/Areas/Admin/bin/VirtoCommerce.Platform.Web.dll");
            if (managerAssemblyPath != null && File.Exists(managerAssemblyPath))
            {
                _directories.Add(HostingEnvironment.MapPath("~/Areas/Admin/bin"));

                _managerAssembly = Assembly.LoadFrom(managerAssemblyPath);
                BuildManager.AddReferencedAssembly(_managerAssembly);
            }
        }

        public void Configuration(IAppBuilder app)
        {
            if (_managerAssembly != null)
            {
                AreaRegistration.RegisterAllAreas();
                CallChildConfigure(app, _managerAssembly, "VirtoCommerce.Platform.Web.Startup", "Configuration", "~/areas/admin", "admin/");
            }

            UnityWebActivator.Start();
            var container = UnityConfig.GetConfiguredContainer();

            // Caching configuration
            // Be cautious with SystemWebCacheHandle because it does not work in native threads (Hangfire jobs).
            var localCache = CacheFactory.FromConfiguration<object>("storefrontCache");
            var localCacheManager = new LocalCacheManager(localCache);
            container.RegisterInstance<ILocalCacheManager>(localCacheManager);

            //Because CacheManagerOutputCacheProvider used diff cache manager instance need translate clear region by this way
            //https://github.com/MichaCo/CacheManager/issues/32
            localCacheManager.OnClearRegion += (sender, region) =>
            {
                try
                {
                    CacheManagerOutputCacheProvider.Cache.ClearRegion(region.Region);
                }
                catch { }
            };
            localCacheManager.OnClear += (sender, args) =>
            {
                try
                {
                    CacheManagerOutputCacheProvider.Cache.Clear();
                }
                catch { }
            };

            var distributedCache = CacheFactory.Build("distributedCache", settings =>
      {
          var jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
          var redisCacheEnabled = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Storefront:RedisCache:Enabled", false);

          var memoryHandlePart = settings
              .WithJsonSerializer(jsonSerializerSettings, jsonSerializerSettings)
              .WithUpdateMode(CacheUpdateMode.Up)
              .WithSystemRuntimeCacheHandle("memory")
              .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromHours(1));

          if (redisCacheEnabled)
          {
              var redisCacheConnectionStringName = ConfigurationManager.AppSettings.GetValue("VirtoCommerce:Storefront:RedisCache:ConnectionStringName", "RedisCache");
              var redisConnectionString = ConfigurationManager.ConnectionStrings[redisCacheConnectionStringName].ConnectionString;

              memoryHandlePart
                  .And
                  .WithRedisConfiguration("redis", redisConnectionString)
                  .WithRetryTimeout(100)
                  .WithMaxRetries(1000)
                  .WithRedisBackplane("redis")
                  .WithRedisCacheHandle("redis", true)
                  .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromHours(1));
          }
      });
            var distributedCacheManager = new DistributedCacheManager(distributedCache);
            container.RegisterInstance<IDistributedCacheManager>(distributedCacheManager);

            var logger = LogManager.GetLogger("default");
            container.RegisterInstance<ILogger>(logger);

            // Create new work context for each request
            container.RegisterType<WorkContext, WorkContext>(new PerRequestLifetimeManager());
            Func<WorkContext> workContextFactory = () => container.Resolve<WorkContext>();
            container.RegisterInstance(workContextFactory);

            // Workaround for old storefront base URL: remove /api/ suffix since it is already included in every resource address in VirtoCommerce.Client library.
            var baseUrl = ConfigurationManager.ConnectionStrings["VirtoCommerceBaseUrl"].ConnectionString;
            if (baseUrl != null && baseUrl.EndsWith("/api/", StringComparison.OrdinalIgnoreCase))
            {
                var apiPosition = baseUrl.LastIndexOf("/api/", StringComparison.OrdinalIgnoreCase);
                if (apiPosition >= 0)
                {
                    baseUrl = baseUrl.Remove(apiPosition);
                }
            }

            var apiAppId = ConfigurationManager.AppSettings["vc-public-ApiAppId"];
            var apiSecretKey = ConfigurationManager.AppSettings["vc-public-ApiSecretKey"];
            var apiClient = new StorefrontHmacApiClient(baseUrl, apiAppId, apiSecretKey, workContextFactory);
            container.RegisterInstance<ApiClient>(apiClient);
            container.RegisterInstance(new VirtoCommerce.Client.Client.Configuration(apiClient));

            container.RegisterType<IStoreModuleApi, StoreModuleApi>();
            container.RegisterType<IVirtoCommercePlatformApi, VirtoCommercePlatformApi>();
            container.RegisterType<ICustomerManagementModuleApi, CustomerManagementModuleApi>();
            container.RegisterType<ICommerceCoreModuleApi, CommerceCoreModuleApi>();
            container.RegisterType<ICustomerManagementModuleApi, CustomerManagementModuleApi>();
            container.RegisterType<ICatalogModuleApi, CatalogModuleApi>();
            container.RegisterType<IPricingModuleApi, PricingModuleApi>();
            container.RegisterType<IInventoryModuleApi, InventoryModuleApi>();
            container.RegisterType<IShoppingCartModuleApi, ShoppingCartModuleApi>();
            container.RegisterType<IOrderModuleApi, OrderModuleApi>();
            container.RegisterType<IMarketingModuleApi, MarketingModuleApi>();
            container.RegisterType<ICMSContentModuleApi, CMSContentModuleApi>();
            container.RegisterType<IQuoteModuleApi, QuoteModuleApi>();
            container.RegisterType<ISearchModuleApi, SearchModuleApi>();
            container.RegisterType<IMarketingService, MarketingServiceImpl>();
            container.RegisterType<IPromotionEvaluator, PromotionEvaluator>();
            container.RegisterType<ICartValidator, CartValidator>();
            container.RegisterType<IPricingService, PricingServiceImpl>();
            container.RegisterType<ICustomerService, CustomerServiceImpl>();
            container.RegisterType<IMenuLinkListService, MenuLinkListServiceImpl>();

            container.RegisterType<ICartBuilder, CartBuilder>();
            container.RegisterType<IQuoteRequestBuilder, QuoteRequestBuilder>();
            container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(context => HttpContext.Current.GetOwinContext().Authentication));


            container.RegisterType<IStorefrontUrlBuilder, StorefrontUrlBuilder>(new PerRequestLifetimeManager());

            //Register domain events
            container.RegisterType<IEventPublisher<OrderPlacedEvent>, EventPublisher<OrderPlacedEvent>>();
            container.RegisterType<IEventPublisher<UserLoginEvent>, EventPublisher<UserLoginEvent>>();
            container.RegisterType<IEventPublisher<QuoteRequestUpdatedEvent>, EventPublisher<QuoteRequestUpdatedEvent>>();
            //Register event handlers (observers)
            container.RegisterType<IAsyncObserver<OrderPlacedEvent>, CustomerServiceImpl>("Invalidate customer cache when user placed new order");
            container.RegisterType<IAsyncObserver<QuoteRequestUpdatedEvent>, CustomerServiceImpl>("Invalidate customer cache when quote request was updated");
            container.RegisterType<IAsyncObserver<UserLoginEvent>, CartBuilder>("Merge anonymous cart with loggined user cart");
            container.RegisterType<IAsyncObserver<UserLoginEvent>, QuoteRequestBuilder>("Merge anonymous quote request with loggined user quote");


            var cmsContentConnectionString = BlobConnectionString.Parse(ConfigurationManager.ConnectionStrings["ContentConnectionString"].ConnectionString);
            var themesBasePath = cmsContentConnectionString.RootPath.TrimEnd('/') + "/" + "Themes";
            var staticContentBasePath = cmsContentConnectionString.RootPath.TrimEnd('/') + "/" + "Pages";
            //Use always file system provider for global theme
            var globalThemesBlobProvider = new FileSystemContentBlobProvider(ResolveLocalPath("~/App_Data/Themes/default"));
            IContentBlobProvider themesBlobProvider;
            IContentBlobProvider staticContentBlobProvider;
            if ("AzureBlobStorage".Equals(cmsContentConnectionString.Provider, StringComparison.OrdinalIgnoreCase))
            {
                themesBlobProvider = new AzureBlobContentProvider(cmsContentConnectionString.ConnectionString, themesBasePath, localCacheManager);
                staticContentBlobProvider = new AzureBlobContentProvider(cmsContentConnectionString.ConnectionString, staticContentBasePath, localCacheManager);
            }
            else
            {
                themesBlobProvider = new FileSystemContentBlobProvider(ResolveLocalPath(themesBasePath));
                staticContentBlobProvider = new FileSystemContentBlobProvider(ResolveLocalPath(staticContentBasePath));
            }
            var shopifyLiquidEngine = new ShopifyLiquidThemeEngine(localCacheManager, workContextFactory, () => container.Resolve<IStorefrontUrlBuilder>(), themesBlobProvider, globalThemesBlobProvider, "~/themes/assets", "~/themes/global/assets");
            container.RegisterInstance<ILiquidThemeEngine>(shopifyLiquidEngine);

            //Register liquid engine
            ViewEngines.Engines.Add(new DotLiquidThemedViewEngine(shopifyLiquidEngine));

            // Shopify model binders convert Shopify form fields with bad names to VirtoCommerce model properties.
            container.RegisterType<IModelBinderProvider, ShopifyModelBinderProvider>("shopify");

            var markdownOptions = new MarkdownOptions
            {
                LinkEmails = false // Render mailto: links as is without markup transformations
            };
            //Static content service
            var staticContentService = new StaticContentServiceImpl(new Markdown(markdownOptions), shopifyLiquidEngine, localCacheManager, workContextFactory, () => container.Resolve<IStorefrontUrlBuilder>(), StaticContentItemFactory.GetContentItemFromPath, staticContentBlobProvider);
            container.RegisterInstance<IStaticContentService>(staticContentService);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, workContextFactory, () => container.Resolve<CommonController>());
            RouteConfig.RegisterRoutes(RouteTable.Routes, workContextFactory, container.Resolve<ICommerceCoreModuleApi>(), container.Resolve<IStaticContentService>(), localCacheManager);
            AuthConfig.ConfigureAuth(app, () => container.Resolve<IStorefrontUrlBuilder>());

            app.Use<WorkContextOwinMiddleware>(container);
            app.UseStageMarker(PipelineStage.PostAuthorize);
            app.Use<StorefrontUrlRewriterOwinMiddleware>(container);
            app.UseStageMarker(PipelineStage.PostAuthorize);
        }


        private static void CallChildConfigure(IAppBuilder app, Assembly assembly, string typeName, string methodName, string virtualRoot, string routPrefix)
        {
            var type = assembly.GetType(typeName);
            if (type != null)
            {
                var methodInfo = type.GetMethod(methodName, new[] { typeof(IAppBuilder), typeof(string), typeof(string) });
                if (methodInfo != null)
                {
                    var classInstance = Activator.CreateInstance(type, null);
                    var parameters = new object[] { app, virtualRoot, routPrefix };
                    var result = methodInfo.Invoke(classInstance, parameters);
                }
            }
        }

        private static Assembly Resolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            var assemblyName = new AssemblyName(args.Name);
            var fileName = assemblyName.Name + ".dll";

            foreach (var directoryPath in _directories)
            {
                var filePath = Path.Combine(directoryPath, fileName);
                if (File.Exists(filePath))
                {
                    assembly = Assembly.LoadFrom(filePath);
                    break;
                }
            }

            return assembly;
        }


        private static string ResolveLocalPath(string path)
        {
            var retVal = path;
            if (path.StartsWith("~"))
            {
                retVal = HostingEnvironment.MapPath(path);
            }
            else if (Path.IsPathRooted(path))
            {
                retVal = path;
            }
            else
            {
                retVal = HostingEnvironment.MapPath("~/");
                retVal += path;
            }
            return retVal;
        }
    }
}
