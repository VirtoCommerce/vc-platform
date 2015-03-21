using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity.Mvc;

namespace VirtoCommerce.Web.Client
{
    /// <summary>
    /// Class UnityDependencyResolverServiceLocatorProvider.
    /// </summary>
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
