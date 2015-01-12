using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using VirtoCommerce.Web;
using VirtoCommerce.Web.Helpers.MVC.SiteMap;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(UnityWebActivator), "Shutdown")]

namespace VirtoCommerce.Web
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            //Add Unity extension for mvc sitemap dependency injection
            container.AddNewExtension<MvcSiteMapProviderContainerExtension>();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            var dependencyResolver = new UnityDependencyResolver(container);
            var locatorProvider = new UnityDependencyResolverServiceLocatorProvider(dependencyResolver);
            ServiceLocator.SetLocatorProvider(() => locatorProvider);
            DependencyResolver.SetResolver(dependencyResolver);
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }

    public class UnityDependencyResolverServiceLocatorProvider : ServiceLocatorImplBase
    {
        /// <summary>
        /// The _unity dependency resolver
        /// </summary>
        private readonly UnityDependencyResolver _unityDependencyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyResolverServiceLocatorProvider"/> class.
        /// </summary>
        /// <param name="unityDependencyResolver">The unity dependency resolver.</param>
        public UnityDependencyResolverServiceLocatorProvider(UnityDependencyResolver unityDependencyResolver)
        {
            _unityDependencyResolver = unityDependencyResolver;
        }

        /// <summary>
        /// Resolves the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            return _unityDependencyResolver.GetService(serviceType);
        }

        /// <summary>
        /// Resolves all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Sequence of service instance objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return _unityDependencyResolver.GetServices(serviceType);
        }
    }
}