using System;
using System.Web.Http;
using Microsoft.Practices.Unity;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Services;
using VirtoCommerce.CustomerModule.Web.ExportImport;
using VirtoCommerce.CustomerModule.Web.JsonConverters;
using VirtoCommerce.Domain.Customer.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CustomerModule.Web
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
            using (var db = new CustomerRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<CustomerRepositoryImpl, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(db);
            }

        }

        public override void Initialize()
        {
            //Member changing event publisher.
            _container.RegisterType<IEventPublisher<MemberChangingEvent>, EventPublisher<MemberChangingEvent>>();

            var memberServiceDecorator = new MemberServiceDecorator();
            _container.RegisterInstance(memberServiceDecorator);
            _container.RegisterInstance<IMemberService>(memberServiceDecorator);
            _container.RegisterInstance<IMemberFactory>(memberServiceDecorator);
            _container.RegisterInstance<IMemberSearchService>(memberServiceDecorator);
        }

        public override void PostInitialize()
        {
            var memberServiceDecorator = _container.Resolve<MemberServiceDecorator>();
        
            Func<CustomerRepositoryImpl> customerRepositoryFactory = () => new CustomerRepositoryImpl(_connectionStringName, new EntityPrimaryKeyGeneratorInterceptor(), _container.Resolve<AuditableInterceptor>());
            var commerceMembersService = new CommerceMembersServiceImpl(customerRepositoryFactory, _container.Resolve<IDynamicPropertyService>(), _container.Resolve<ISecurityService>(), memberServiceDecorator, _container.Resolve<IEventPublisher<MemberChangingEvent>>());

            memberServiceDecorator.RegisterMemberTypes(typeof(Organization), typeof(Contact), typeof(Vendor), typeof(Employee))
                                  .WithService(commerceMembersService)
                                  .WithSearchService(commerceMembersService);          


            //Next lines allow to use polymorph types in API controller methods
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.JsonFormatter.SerializerSettings.Converters.Add(new PolymorphicMemberJsonConverter(memberServiceDecorator));

            base.PostInitialize();
        }
        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CustomerExportImport>();
            exportJob.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var exportJob = _container.Resolve<CustomerExportImport>();
            exportJob.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Customer.ExportImport.Description", String.Empty);
            }
        }
        #endregion
    }

}
