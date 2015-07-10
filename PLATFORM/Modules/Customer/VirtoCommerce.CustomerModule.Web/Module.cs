using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.CustomerModule.Web.ExportImport;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CustomerModule.Web
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
            var customerSearchService = _container.Resolve<ICustomerSearchService>();
            var contactService = _container.Resolve<IContactService>();
            var organizationService = _container.Resolve<IOrganizationService>();
            var responce = customerSearchService.Search(new SearchCriteria());

            
            var backupObject = new BackupObject
            {
                Contacts = responce.Contacts.Select(x => x.Id).Select(contactService.GetById).ToArray(),
                Organizations = responce.Organizations.Select(x => x.Id).Select(organizationService.GetById).ToArray()
            };
            exportJob.DoExport(outStream, backupObject, progressCallback);
        }

        #endregion

    }

}
