﻿using System;
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
using VirtoCommerce.LiquidThemeEngine.Objects;
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
            UnityWebActivator.Start();
            var container = UnityConfig.GetConfiguredContainer();

            //Caching configuration (system runtime memory handle)
            var cacheManager = CacheFactory.FromConfiguration<object>("storefrontCache");
            container.RegisterInstance<ICacheManager<object>>(cacheManager);
            //Because CacheManagerOutputCacheProvider used diff cache manager instance need translate clear region by this way
            //https://github.com/MichaCo/CacheManager/issues/32
            cacheManager.OnClearRegion += (sender, region) =>
            {
                CacheManagerOutputCacheProvider.Cache.ClearRegion(region.Region);
            };
            cacheManager.OnClear += (sender, args) =>
            {
                CacheManagerOutputCacheProvider.Cache.Clear();
            };

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
            container.RegisterType<ISearchModuleApi, SearchModuleApi>();
            container.RegisterType<IMarketingService, MarketingServiceImpl>();

            container.RegisterType<ICartBuilder, CartBuilder>();
            container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory((context) => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IStorefrontUrlBuilder, StorefrontUrlBuilder>(new PerRequestLifetimeManager());
            if (_managerAssembly != null)
            {
                AreaRegistration.RegisterAllAreas();
                CallChildConfigure(app, _managerAssembly, "VirtoCommerce.Platform.Web.Startup", "Configuration", "~/areas/admin", "admin/");
            }

            // Create new work context for each request
            container.RegisterType<WorkContext, WorkContext>(new PerRequestLifetimeManager());

            var shopifyLiquidEngine = new ShopifyLiquidThemeEngine(cacheManager, () => container.Resolve<WorkContext>(), () => container.Resolve<IStorefrontUrlBuilder>(), HostingEnvironment.MapPath("~/App_data/themes"), "~/themes/assets", "~/themes/global/assets");
            container.RegisterInstance(shopifyLiquidEngine);
            //Register liquid engine
            ViewEngines.Engines.Add(new DotLiquidThemedViewEngine(container.Resolve<ShopifyLiquidThemeEngine>()));

            // Shopify model binders convert Shopify form fields with bad names to VirtoCommerce model properties.
            container.RegisterType<IModelBinderProvider, ShopifyModelBinderProvider>("shopify");

            //Static content service
            var staticContentService = new StaticContentServiceImpl(HostingEnvironment.MapPath("~/App_data/Pages"), new Markdown(), shopifyLiquidEngine, cacheManager);
            container.RegisterInstance<IStaticContentService>(staticContentService);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes, () => container.Resolve<WorkContext>(), container.Resolve<ICommerceCoreModuleApi>(), container.Resolve<IStaticContentService>(), cacheManager);
            AuthConfig.ConfigureAuth(app);

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
    }
}
