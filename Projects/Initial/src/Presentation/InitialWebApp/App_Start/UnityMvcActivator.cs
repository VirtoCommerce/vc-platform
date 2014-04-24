using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Initial.Web;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Mvc;
using VirtoCommerce.Web.Client;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebActivator), "Start")]
namespace Initial.Web
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            var dependencyResolver = new UnityDependencyResolver(container);
            var locatorProvider = new UnityDependencyResolverServiceLocatorProvider(dependencyResolver);
            ServiceLocator.SetLocatorProvider(() => locatorProvider);
            DependencyResolver.SetResolver(dependencyResolver);

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }
    }
}