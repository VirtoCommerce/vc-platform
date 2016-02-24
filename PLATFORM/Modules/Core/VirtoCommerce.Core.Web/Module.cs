using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Shipping;
using VirtoCommerce.CoreModule.Web.ExportImport;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CoreModule.Data.Payment;
using VirtoCommerce.Domain.Tax.Services;
using VirtoCommerce.CoreModule.Data.Tax;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CoreModule.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase()
        {
            using (var db = new CommerceRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<CommerceRepositoryImpl, VirtoCommerce.CoreModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            //#region Payment gateways manager

            //_container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            //#endregion

            #region Fulfillment

            _container.RegisterType<IСommerceRepository>(new InjectionFactory(c => new CommerceRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>())));
            _container.RegisterType<ICommerceService, CommerceServiceImpl>();

            #endregion

            #region Tax service
            var taxService = new TaxServiceImpl();
            _container.RegisterInstance<ITaxService>(taxService);
            #endregion
            #region Shipping service
            var shippingService = new ShippingMethodsServiceImpl();
            _container.RegisterInstance<IShippingMethodsService>(shippingService);
            #endregion

            #region Payment service
            var paymentService = new PaymentMethodsServiceImpl();
            _container.RegisterInstance<IPaymentMethodsService>(paymentService);
            #endregion
        }

        public override void PostInitialize()
        {
            var settingManager = _container.Resolve<ISettingsManager>();
            var commerceService = _container.Resolve<ICommerceService>();
            var shippingService = _container.Resolve<IShippingMethodsService>();
            var taxService = _container.Resolve<ITaxService>();
            var paymentService = _container.Resolve<IPaymentMethodsService>();
            var moduleSettings = settingManager.GetModuleSettings("VirtoCommerce.Core");
            taxService.RegisterTaxProvider(() => new FixedTaxRateProvider(moduleSettings.First(x => x.Name == "VirtoCommerce.Core.FixedTaxRateProvider.Rate"))
            {
                Name = "fixed tax rate",
                Description = "Fixed percent tax rate",
                LogoUrl = "http://virtocommerce.com/Content/images/logo.jpg"
            });

            shippingService.RegisterShippingMethod(() => new FixedRateShippingMethod(moduleSettings.First(x => x.Name == "VirtoCommerce.Core.FixedRateShippingMethod.Rate"))
            {
                Name = "fixed rate",
                Description = "Fixed rate shipping method",
                LogoUrl = "http://virtocommerce.com/Content/images/logo.jpg"

            });

            paymentService.RegisterPaymentMethod(() => new DefaultManualPaymentMethod()
            {
                IsActive = true,
                Name = "Manual test payment method",
                Description = "Manual test, don't use on production",
                LogoUrl = "http://virtocommerce.com/Content/images/logo.jpg",
            });

            var currencies = commerceService.GetAllCurrencies();
            if (!currencies.Any())
            {
                var defaultCurrency = new Currency
                {
                    Code = "USD",
                    IsPrimary = true,
                    ExchangeRate = 1,
                    Symbol = "$",
                    Name = "US dollar"
                };
                commerceService.UpsertCurrencies(new[] { defaultCurrency });
            }
        }

        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<CoreExportImport>();
            job.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<CoreExportImport>();
            job.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("VirtoCommerce.Core.ExportImport.Description", String.Empty);
            }
        }

        #endregion
    }
}
