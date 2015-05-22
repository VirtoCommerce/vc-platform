using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Payment.Services;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Shipping.Services;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;


namespace VirtoCommerce.CoreModule.Web
{
    public class Module : IModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public void SetupDatabase(SampleDataLevel sampleDataLevel)
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

        public void Initialize()
        {
           #region Payment gateways manager

            _container.RegisterType<IPaymentGatewayManager, InMemoryPaymentGatewayManagerImpl>(new ContainerControlledLifetimeManager());

            #endregion

           #region Fulfillment

			_container.RegisterType<IСommerceRepository>(new InjectionFactory(c => new CommerceRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));
			_container.RegisterType<ICommerceService, CommerceServiceImpl>();

            #endregion

			#region Shipping service
			var shippingService = new ShippingServiceImpl();
			_container.RegisterInstance<IShippingService>(shippingService);

			_container.RegisterType<IShippingRateEvaluator, StoreShippingRateEvaluator>();
			#endregion
        }

        public void PostInitialize()
        {
			var settingManager = _container.Resolve<ISettingsManager>();
			var shippingService = _container.Resolve<IShippingService>();
			shippingService.RegisterShippingMethod(() => new FixedRateShippingMethod(settingManager.GetModuleSettings("VirtoCommerce.Core")));
      
        }


		public class FixedRateShippingMethod : ShippingMethod
		{
			public FixedRateShippingMethod(ICollection<SettingEntry> settings)
				: base("FixedRate")
			{
				Settings = settings;
			}
			public decimal Rate { get; set; }

			public override ShippingRate CalculateRate(Domain.Common.IEvaluationContext context)
			{
				return new ShippingRate { Rate = Rate, ShippingMethod = this };
			}
		}

		#endregion
    }
}
