using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.ApiWebClient.Caching;
using VirtoCommerce.ApiWebClient.Caching.Interfaces;
using VirtoCommerce.ApiWebClient.Clients;
using VirtoCommerce.ApiWebClient.Currencies;
using VirtoCommerce.ApiClient.Caching;
using VirtoCommerce.ApiClient.Session;

namespace VirtoCommerce.Web
{

    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {

            container.RegisterType<ICurrencyService, CurrencyService>(new ContainerControlledLifetimeManager());

            #region Customer

            container.RegisterType<ICustomerSessionService, CustomerSessionService>();

            #endregion

            #region MVC Helpers

            container.RegisterType<StoreClient>();

            #endregion


            #region OutputCache

            container.RegisterType<ICacheRepository, HttpCacheRepository>();
            container.RegisterType<IKeyBuilder, KeyBuilder>(new PerRequestLifetimeManager());
            container.RegisterType<IKeyGenerator, KeyGenerator>(new PerRequestLifetimeManager());
            container.RegisterType<IDonutHoleFiller, DonutHoleFiller>(new PerRequestLifetimeManager());
            container.RegisterType<ICacheHeadersHelper, CacheHeadersHelper>(new PerRequestLifetimeManager());
            container.RegisterType<ICacheSettingsManager, CacheSettingsManager>(new PerRequestLifetimeManager());
            container.RegisterType<IReadWriteOutputCacheManager, OutputCacheManager>(new PerRequestLifetimeManager());
            container.RegisterInstance<IActionSettingsSerialiser>(new EncryptingActionSettingsSerialiser(new ActionSettingsSerialiser(), new Encryptor()));

            #endregion
        }
    }
}
