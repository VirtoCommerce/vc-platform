using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.MarketingModule.Web.Model;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.MarketingModule.Web
{
    public class Module : ModuleBase, ISupportExportModule, ISupportImportModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            using (var context = new MarketingRepositoryImpl())
            {
                var initializer = new SetupDatabaseInitializer<MarketingRepositoryImpl, VirtoCommerce.MarketingModule.Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IMarketingRepository>(new InjectionFactory(c => new MarketingRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));

            var promotionExtensionManager = new DefaultMarketingExtensionManagerImpl();

            _container.RegisterInstance<IMarketingExtensionManager>(promotionExtensionManager);
            _container.RegisterType<IPromotionService, PromotionServiceImpl>();
            _container.RegisterType<IMarketingDynamicContentEvaluator, DefaultDynamicContentEvaluatorImpl>();
            _container.RegisterType<IDynamicContentService, DynamicContentServiceImpl>();
            _container.RegisterType<IMarketingSearchService, MarketingSearchServiceImpl>();
            _container.RegisterType<IMarketingPromoEvaluator, DefaultPromotionEvaluatorImpl>();
        }

        public override void PostInitialize()
        {
            var promotionExtensionManager = _container.Resolve<IMarketingExtensionManager>();
            EnsureRootFoldersExist(new[] { MarketingConstants.ContentPlacesRootFolderId, MarketingConstants.CotentItemRootFolderId });
        }

        #endregion

        #region ISupportExportModule Members

		public void DoExport(System.IO.Stream outStream, PlatformExportImportOptions exportOptions, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<MarketingExportImport>();
            exportJob.DoExport(outStream, progressCallback);
        }

        #endregion

        #region ISupportImportModule Members

		public void DoImport(System.IO.Stream inputStream, PlatformExportImportOptions importOptions, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<MarketingExportImport>();
            exportJob.DoImport(inputStream, progressCallback);
        }

        #endregion


        private void EnsureRootFoldersExist(string[] ids)
        {
            var dynamicContentService = _container.Resolve<IDynamicContentService>();
            foreach (var id in ids)
            {
                var rootFolder = dynamicContentService.GetFolderById(id);
                if (rootFolder == null)
                {
                    rootFolder = new Domain.Marketing.Model.DynamicContentFolder
                    {
                        Id = id,
                        Name = id
                    };
                    dynamicContentService.CreateFolder(rootFolder);
                }
            }

        }
    }
}
