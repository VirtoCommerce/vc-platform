using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Microsoft.Owin;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Newtonsoft.Json;
using Owin;
using VirtoCommerce.ApiClient.Utilities;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CatalogModule.Data;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.MerchandisingModule.Web;
using VirtoCommerce.MerchandisingModule.Web.Controllers;
using VirtoCommerce.Search.Providers.Elastic;

namespace VirtoCommerce.ApiClient.Tests
{
    public class OwinStartup
    {
        Type valuesControllerType = typeof(StoresController);
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use(typeof(LogMiddleware));
            var config = new HttpConfiguration();
            var container = new UnityContainer();

            #region Search

            container.RegisterType<ISearchProvider, ElasticSearchProvider>();
            container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            container.RegisterInstance<ISearchConnection>(searchConnection);

            #endregion

            container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ICatalogRepository, EFCatalogRepositoryWrapper>();
            container.RegisterType<ICacheRepository, HttpCacheRepository>();
            container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();

            #region Interceptors
            container.RegisterType<IInterceptor, AuditChangeInterceptor>("audit");

            #endregion

            container.RegisterType<CatalogClient>();

            var merchantModule = new MerchModule(container);
            merchantModule.Initialize();


            config.MapHttpAttributeRoutes();
            config.DependencyResolver = new UnityResolver(container);
            var formatter = config.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new DeltaContractResolver(formatter);
            formatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatter.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            formatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            appBuilder.UseWebApi(config);
        }
    }

    public class CustomAssemblyResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var baseAssemblies = base.GetAssemblies();
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
   
            var assemblies = new List<Assembly>(baseAssemblies);
            assemblies.AddRange(domainAssemblies);

            return assemblies;
        }

    }

    public class LogMiddleware : OwinMiddleware
    {
        public LogMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public async override Task Invoke(IOwinContext context)
        {

            Debug.WriteLine("Request begins: {0} {1} \n\r {2}", context.Request.Method, context.Request.Uri, GetString(context.Request.Body));
            await Next.Invoke(context);
            Debug.WriteLine("Request ends : {0} {1} \n\r {2}", context.Request.Method, context.Request.Uri, GetString(context.Request.Body));
        }

        private string GetString(Stream stream)
        {
            var reader = new StreamReader(stream);
            var text = reader.ReadToEnd();
            return text;
        }
    }
}
