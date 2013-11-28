using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Mvc;
using VirtoCommerce.Foundation.Data.Common;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Web.Client;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(VirtoCommerce.Web.UnityWebActivator), "Start")]

namespace VirtoCommerce.Web
{
    using VirtoCommerce.Foundation.Data.Azure.Common;

    /// <summary>
    /// Provides the bootstrapping for integrating Unity with ASP.NET MVC.
    /// </summary>
    public static class UnityWebActivator
    {
        /// <summary>
        /// Integrates Unity when the application starts.
        /// </summary>
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

			if (!AzureCommonHelper.IsAzureEnvironment() && ConnectionHelper.IsDatabaseInstalled)
			{
				SchedulerHost.CreateScheduler(container);
			}
        }
    }
}