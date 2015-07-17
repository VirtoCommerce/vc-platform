using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.CustomerModule.Web.ExportImport;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CustomerModule.Web
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
            using (var db = new CustomerRepositoryImpl("VirtoCommerce"))
            {
                IDatabaseInitializer<CustomerRepositoryImpl> initializer;

                switch (sampleDataLevel)
                {
                    case SampleDataLevel.Full:
                    case SampleDataLevel.Reduced:
                        initializer = new SqlCustomerSampleDatabaseInitializer();
                        break;
                    default:
                        initializer = new SetupDatabaseInitializer<CustomerRepositoryImpl, VirtoCommerce.CustomerModule.Data.Migrations.Configuration>();
                        break;
                }

                initializer.InitializeDatabase(db);
            }

        }

        public override void Initialize()
        {
            _container.RegisterType<ICustomerRepository>(new InjectionFactory(c => new CustomerRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor())));

            _container.RegisterType<IOrganizationService, OrganizationServiceImpl>();
            _container.RegisterType<IContactService, ContactServiceImpl>();
            _container.RegisterType<ICustomerSearchService, CustomerSearchServiceImpl>();
        }

        #endregion

        #region ISupportExportModule Members

        public void DoExport(System.IO.Stream outStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CustomerExportImport>();
            exportJob.DoExport(outStream, progressCallback);
        }

        #endregion

        #region ISupportImportModule Members

        public void DoImport(System.IO.Stream inputStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CustomerExportImport>();
            exportJob.DoImport(inputStream, progressCallback);
        }

        #endregion
    }

}
