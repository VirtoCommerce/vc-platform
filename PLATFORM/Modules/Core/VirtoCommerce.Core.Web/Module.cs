using System;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using Owin;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.CoreModule.Web.Controllers.Api;
using VirtoCommerce.CoreModule.Web.Notification;
using VirtoCommerce.CoreModule.Web.Repositories;
using VirtoCommerce.CoreModule.Web.Security;
using VirtoCommerce.CoreModule.Web.Services;
using VirtoCommerce.CoreModule.Web.Settings;
using VirtoCommerce.Domain.Fulfillment.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.AppConfig;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Data.Security;
using VirtoCommerce.Foundation.Data.Security.Identity;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Notification;
using VirtoCommerce.Framework.Web.Security;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.SecurityModule.Web.Controllers;
using Microsoft.Owin.Security;
using VirtoCommerce.Domain.Mailing.Services;

namespace VirtoCommerce.CoreModule.Web
{
    [Module(ModuleName = "CoreModule", OnDemand = true)]
    public class Module : IModule, IDatabaseModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;
        private readonly IAppBuilder _appBuilder;

        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
            _appBuilder = appBuilder;
        }

        #region IDatabaseModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var db = new SecurityDbContext(_connectionStringName))
            {
                IdentityDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new IdentitySampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new IdentityDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFSecurityRepository(_connectionStringName))
            {
                SqlSecurityDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlSecuritySampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlSecurityDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFCustomerRepository(_connectionStringName))
            {
                SqlCustomerDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCustomerSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlCustomerDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFAppConfigRepository(_connectionStringName))
            {
                SqlAppConfigDatabaseInitializer initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                        initializer = new SqlAppConfigSampleDatabaseInitializer();
                        break;
                    case SampleDataLevel.Reduced:
                        initializer = new SqlAppConfigReducedSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SqlAppConfigDatabaseInitializer();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

            using (var db = new EFSearchRepository(_connectionStringName))
            {
                new SearchDatabaseInitializer().InitializeDatabase(db);
            }
        }

        #endregion

        #region IModule Members

        public void Initialize()
        {
            _container.RegisterType<ICacheRepository, HttpCacheRepository>(new ContainerControlledLifetimeManager());

            #region Settings

            _container.RegisterType<Func<IAppConfigRepository>>(
                new InjectionFactory(x => new Func<IAppConfigRepository>(() => new EFAppConfigRepository(_connectionStringName))));

            _container.RegisterType<ISettingsManager, SettingsManager>();

            #endregion

            #region Security
			var foundationSecurityRepositoryFactory = new Func<IFoundationSecurityRepository>(() => new FoundationSecurityRepositoryImpl(_connectionStringName));
			_container.RegisterType<Func<IFoundationSecurityRepository>>(
				new InjectionFactory(x => foundationSecurityRepositoryFactory));

			var securityRepositoryFactory = new Func<ISecurityRepository>(() => new EFSecurityRepository(_connectionStringName));
			_container.RegisterType<Func<ISecurityRepository>>(new InjectionFactory(x => securityRepositoryFactory));

			_container.RegisterType<IPermissionService, PermissionService>(new ContainerControlledLifetimeManager());

			_container.RegisterType<IApiAccountProvider, ApiAccountProvider>(new ContainerControlledLifetimeManager());
			_container.RegisterType<IClaimsIdentityProvider, ApplicationClaimsIdentityProvider>(new ContainerControlledLifetimeManager());

			Func<ApplicationSignInManager> signInApplication = () => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
			Func<ApplicationUserManager> userManager = () => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			Func<IAuthenticationManager> auth = () => HttpContext.Current.GetOwinContext().Authentication;
            var apiAccountProvider = _container.Resolve<IApiAccountProvider>();
            _container.RegisterType<SecurityController>(new InjectionConstructor(foundationSecurityRepositoryFactory, signInApplication, userManager, auth, apiAccountProvider));

			#endregion

            #region Payment gateways manager
            _container.RegisterInstance<IPaymentGatewayManager>(new InMemoryPaymentGatewayManagerImpl());
            #endregion

            #region Mailing manager
            _container.RegisterInstance<IMailingManager>(new InMemoryMailingManagerImpl());
            #endregion

            #region Notification
            _container.RegisterInstance<INotifier>(new InMemoryNotifierImpl());
            #endregion

            #region Fulfillment
            _container.RegisterType<Func<IFoundationFulfillmentRepository>>(
              new InjectionFactory(x => new Func<IFoundationFulfillmentRepository>(() =>
                  new FoundationFulfillmentRepositoryImpl(_connectionStringName))));
            _container.RegisterType<IFulfillmentService, FulfillmentServiceImpl>();
            #endregion

            OwinConfig.Configure(_appBuilder, _container);
        }

        #endregion
    }
}
