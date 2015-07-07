using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Unity;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.ImportExport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.CatalogModule.Web
{
	public class Module : ModuleBase, ISupportExportModule
    {
        private const string _connectionStringName = "VirtoCommerce";
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IDatabaseModule Members

        public override void SetupDatabase(SampleDataLevel sampleDataLevel)
        {
            base.SetupDatabase(sampleDataLevel);

            using (var db = new CatalogRepositoryImpl(_connectionStringName))
            {
                IDatabaseInitializer<CatalogRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                        initializer = new SqlCatalogSampleDatabaseInitializer();
                        break;
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCatalogReducedSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SetupDatabaseInitializer<CatalogRepositoryImpl, Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(db);
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            #region Catalog dependencies

            Func<ICatalogRepository> catalogRepFactory = () =>
                new CatalogRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(),
                    new ChangeLogInterceptor(_container.Resolve<Func<IPlatformRepository>>(), ChangeLogPolicy.Cumulative, new[] { typeof(Product).Name }));

            _container.RegisterInstance(catalogRepFactory);

            _container.RegisterType<IItemService, ItemServiceImpl>();
            _container.RegisterType<ICategoryService, CategoryServiceImpl>();
            _container.RegisterType<ICatalogService, CatalogServiceImpl>();
            _container.RegisterType<IPropertyService, PropertyServiceImpl>();
            _container.RegisterType<ICatalogSearchService, CatalogSearchServiceImpl>();
			_container.RegisterType<ISkuGenerator, DefaultSkuGenerator>();

            #endregion

      
        }

        #endregion

		#region ISupportExport Members

		public void DoExport(System.IO.Stream outStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var exportedTypes = new string[] { "categories", "products", "images", "assets", "seo", "reviews" };
			var progressInfo = new ExportImportProgressInfo
			{
				TotalCount = exportedTypes.Count()
			};

			using (var streamWriter = new StreamWriter(outStream))
			{
				foreach (var exportedType in exportedTypes)
				{
					progressInfo.Status = exportedType;
					progressInfo.ProcessedCount++;
					progressCallback(progressInfo);
					Thread.Sleep(1000);
					streamWriter.Write("Hello Catalog module - " + exportedType);
				}
			}

		}

		#endregion
	}
}
