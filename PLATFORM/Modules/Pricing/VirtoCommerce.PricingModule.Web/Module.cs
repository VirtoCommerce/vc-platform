using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Pricing.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Repositories;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.PricingModule.Web.ExportImport;
using dataModel = VirtoCommerce.PricingModule.Data.Model;

namespace VirtoCommerce.PricingModule.Web
{
    public class Module : ModuleBase, ISupportExportModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new PricingRepositoryImpl("VirtoCommerce"))
            {
                IDatabaseInitializer<PricingRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new PricingSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SetupDatabaseInitializer<PricingRepositoryImpl, Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            var extensionManager = new DefaultPricingExtensionManagerImpl();
            _container.RegisterInstance<IPricingExtensionManager>(extensionManager);

            _container.RegisterType<IPricingRepository>(new InjectionFactory(c => new PricingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(), new ChangeLogInterceptor(_container.Resolve<Func<IPlatformRepository>>(), ChangeLogPolicy.Cumulative, new[] { typeof(dataModel.Price).Name }))));
            _container.RegisterType<IPricingService, PricingServiceImpl>();
        }

        #endregion

        #region ISupportExport Members

        public void DoExport(System.IO.Stream outStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<PricingExportImport>();
            var pricingService = _container.Resolve<IPricingService>();

            var backupObject = new BackupObject { Pricelists = pricingService.GetPriceLists().ToArray() };
            exportJob.DoExport(outStream, backupObject, progressCallback);
        }

        #endregion

    }
}
