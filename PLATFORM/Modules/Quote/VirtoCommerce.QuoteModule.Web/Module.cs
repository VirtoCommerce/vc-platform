using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using dataModel = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.QuoteModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase()
        {
    //        using (var context = new PricingRepositoryImpl("VirtoCommerce"))
    //        {
				//var initializer = new SetupDatabaseInitializer<PricingRepositoryImpl, Data.Migrations.Configuration>();
    //            initializer.InitializeDatabase(context);
    //        }
        }

        public override void Initialize()
        {
            //var extensionManager = new DefaultPricingExtensionManagerImpl();
            //_container.RegisterInstance<IPricingExtensionManager>(extensionManager);

            //_container.RegisterType<IPricingRepository>(new InjectionFactory(c => new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(), new ChangeLogInterceptor(_container.Resolve<Func<IPlatformRepository>>(), ChangeLogPolicy.Cumulative, new[] { typeof(dataModel.Price).Name }))));
            //_container.RegisterType<IPricingService, PricingServiceImpl>();
        }

        #endregion

	
    }
}
