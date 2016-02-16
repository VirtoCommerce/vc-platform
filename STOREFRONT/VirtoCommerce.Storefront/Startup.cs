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
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Owin;
using VirtoCommerce.Client;
using VirtoCommerce.Client.Api;
using VirtoCommerce.Client.Client;
using VirtoCommerce.LiquidThemeEngine;
using VirtoCommerce.LiquidThemeEngine.Binders;
using VirtoCommerce.Storefront;
using VirtoCommerce.Storefront.App_Start;
using VirtoCommerce.Storefront.Builders;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Owin;
using VirtoCommerce.Storefront.Model.Services;
using VirtoCommerce.Storefront.Services;
using VirtoCommerce.Storefront.Common;
using CacheManager.Core;
using CacheManager.Web;
using MarkdownDeep;
using VirtoCommerce.Storefront.Model.Marketing.Services;
using VirtoCommerce.Storefront.Model.Cart.Services;
using VirtoCommerce.Storefront.Model.Pricing.Services;
using VirtoCommerce.Storefront.Model.Quote.Services;
using VirtoCommerce.Storefront.Model.Customer.Services;
using VirtoCommerce.Storefront.Model.Common.Events;
using VirtoCommerce.Storefront.Model.Order.Events;
using NLog;
using VirtoCommerce.Storefront.Model.LinkList.Services;

[assembly: OwinStartup(typeof(Startup))]
[assembly: PreApplicationStartMethod(typeof(Startup), "PreApplicationStart")]
namespace VirtoCommerce.Storefront
{
    public partial class Startup
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

            //Caching configuration (system runtime memory handle)
            var cacheManager = CacheFactory.FromConfiguration<object>("storefrontCache");
            container.RegisterInstance<ICacheManager<object>>(cacheManager);
            //Because CacheManagerOutputCacheProvider used diff cache manager instance need translate clear region by this way
            //https://github.com/MichaCo/CacheManager/issues/32
            cacheManager.OnClearRegion += (sender, region) =>
            {
                try
                {
                    CacheManagerOutputCacheProvider.Cache.ClearRegion(region.Region);
                }
                catch (Exception)
                {

                }
            };
            cacheManager.OnClear += (sender, args) =>
            {
                try
                {
                    CacheManagerOutputCacheProvider.Cache.Clear();
                }
                catch (Exception)
                {

                }
            };

            var logger = LogManager.GetLogger("default");
            container.RegisterInstance<ILogger>(logger);

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

            var apiClient = new HmacApiClient(baseUrl, ConfigurationManager.AppSettings["vc-public-ApiAppId"], ConfigurationManager.AppSettings["vc-public-ApiSecretKey"]);
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
            container.RegisterType<IAuthenticationManager>(new InjectionFactory((context) => HttpContext.Current.GetOwinContext().Authentication));


            container.RegisterType<IStorefrontUrlBuilder, StorefrontUrlBuilder>(new PerRequestLifetimeManager());

            //Register domain events
            container.RegisterType<IEventPublisher<OrderPlacedEvent>, EventPublisher<OrderPlacedEvent>>();
            container.RegisterType<IEventPublisher<UserLoginEvent>, EventPublisher<UserLoginEvent>>();
            //Register event handlers (observers)
            container.RegisterType<IAsyncObserver<OrderPlacedEvent>, CustomerServiceImpl>("Invalidate customer cache when user placed new order");
            container.RegisterType<IAsyncObserver<UserLoginEvent>, CartBuilder>("Merge anonymous cart with loggined user cart");
            container.RegisterType<IAsyncObserver<UserLoginEvent>, QuoteRequestBuilder>("Merge anonymous quote request with loggined user quote");

            // Create new work context for each request
            container.RegisterType<WorkContext, WorkContext>(new PerRequestLifetimeManager());
            Func<WorkContext> workContextFactory = () => container.Resolve<WorkContext>();
            container.RegisterInstance(workContextFactory);

            var themesPath = ConfigurationManager.AppSettings["vc-public-themes"] ?? "~/App_data/Themes";
            var shopifyLiquidEngine = new ShopifyLiquidThemeEngine(cacheManager, workContextFactory, () => container.Resolve<IStorefrontUrlBuilder>(), ResolveLocalPath(themesPath), "~/themes/assets", "~/themes/global/assets");
            container.RegisterInstance<ILiquidThemeEngine>(shopifyLiquidEngine);
            //Register liquid engine
            ViewEngines.Engines.Add(new DotLiquidThemedViewEngine(shopifyLiquidEngine));

            // Shopify model binders convert Shopify form fields with bad names to VirtoCommerce model properties.
            container.RegisterType<IModelBinderProvider, ShopifyModelBinderProvider>("shopify");

            var staticContentPath = ConfigurationManager.AppSettings["vc-public-pages"] ?? "~/App_data/Pages";
            //Static content service
            var staticContentService = new StaticContentServiceImpl(ResolveLocalPath(staticContentPath), new Markdown(), shopifyLiquidEngine, cacheManager, workContextFactory, () => container.Resolve<IStorefrontUrlBuilder>());
            container.RegisterInstance<IStaticContentService>(staticContentService);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, () => container.Resolve<WorkContext>());
            RouteConfig.RegisterRoutes(RouteTable.Routes, () => container.Resolve<WorkContext>(), container.Resolve<ICommerceCoreModuleApi>(), container.Resolve<IStaticContentService>(), cacheManager);
            AuthConfig.ConfigureAuth(app, () => container.Resolve<IStorefrontUrlBuilder>());

            app.Use<WorkContextOwinMiddleware>(container);
            app.UseStageMarker(PipelineStage.ResolveCache);
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
