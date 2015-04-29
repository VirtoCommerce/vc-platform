using System.Web;
using Hangfire.SqlServer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Security.Data;
using VirtoCommerce.CoreModule.Web.Services;
using VirtoCommerce.Domain.Fulfillment.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notification;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;


namespace VirtoCommerce.CoreModule.Web
{
    public class Module : IModule, IPostInitialize
    {
        private readonly IUnityContainer _container;
        private readonly IAppBuilder _appBuilder;

        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
            _appBuilder = appBuilder;
        }

        #region IModule Members

        public void Initialize()
        {
               
			 #region Payment gateways manager

            _container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            #endregion

         
            #region Fulfillment

            _container.RegisterType<IFoundationFulfillmentRepository>(new InjectionFactory(c => new FoundationFulfillmentRepositoryImpl(_connectionStringName)));
            _container.RegisterType<IFulfillmentService, FulfillmentServiceImpl>();

            #endregion
        }

        #endregion

        #region IPostInitialize Members

        public void PostInitialize()
        {
        }

        #endregion
    }
}
