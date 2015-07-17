using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Shipping;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CoreModule.Data.Payment;


namespace VirtoCommerce.CoreModule.Web
{
    public class Module : ModuleBase
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var db = new CommerceRepositoryImpl("VirtoCommerce"))
            {
                IDatabaseInitializer<CommerceRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCommerceSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SetupDatabaseInitializer<CommerceRepositoryImpl, VirtoCommerce.CoreModule.Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            //#region Payment gateways manager

            //_container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            //#endregion

            #region Fulfillment

            _container.RegisterType<IСommerceRepository>(new InjectionFactory(c => new CommerceRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
            _container.RegisterType<ICommerceService, CommerceServiceImpl>();

            #endregion

            #region Shipping service
            var shippingService = new ShippingServiceImpl();
            _container.RegisterInstance<IShippingService>(shippingService);
            #endregion

            #region Payment service
            var paymentService = new PaymentMethodsServiceImpl();
            _container.RegisterInstance<IPaymentMethodsService>(paymentService);
            #endregion
        }

        public override void PostInitialize()
        {
            var settingManager = _container.Resolve<ISettingsManager>();
            var shippingService = _container.Resolve<IShippingService>();
			var paymentService = _container.Resolve<IPaymentMethodsService>();

            shippingService.RegisterShippingMethod(() => new FixedRateShippingMethod(new SettingEntry[] { new SettingEntry { Name = "Rate", ValueType = SettingValueType.Decimal, DefaultValue = "0" } })
                {
                    Name = "fixed rate",
                    Description = "Fixed rate shipping method",
                    LogoUrl = "http://somelogo.com/logo.png"

                });

			paymentService.RegisterPaymentMethod(() => new DefaultManualPaymentMethod()
				{
					IsActive = true,
					Name = "Manual test payment method",
					Description = "Manual test, don't use on production",
					LogoUrl = "http://virtocommerce.com/Content/images/logo.jpg",
				});
        }

        #endregion
    }
}
