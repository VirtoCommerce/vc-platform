using System;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Notifications;
using VirtoCommerce.OrderModule.Data.Observers;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Web.ExportImport;
using VirtoCommerce.OrderModule.Web.Resources;
using VirtoCommerce.OrderModule.Web.Security;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.OrderModule.Web
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
            using (var context = new OrderRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>()))
            {
                var initializer = new SetupDatabaseInitializer<OrderRepositoryImpl, Data.Migrations.Configuration>();
                initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IEventPublisher<OrderChangeEvent>, EventPublisher<OrderChangeEvent>>();
            //Adjust inventory activity
            _container.RegisterType<IObserver<OrderChangeEvent>, AdjustInventoryObserver>("AdjustInventoryObserver");
            //Create order observer. Send notification
            _container.RegisterType<IObserver<OrderChangeEvent>, OrderNotificationObserver>("OrderNotificationObserver");
            //Cancel payment observer. Payment method cancel operations
            _container.RegisterType<IObserver<OrderChangeEvent>, CancelPaymentObserver>("CancelPaymentObserver");

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderRepositoryImpl(_connectionStringName, _container.Resolve<AuditableInterceptor>(), new EntityPrimaryKeyGeneratorInterceptor())));
            //_container.RegisterInstance<IInventoryService>(new Mock<IInventoryService>().Object);
            _container.RegisterType<IUniqueNumberGenerator, SequenceUniqueNumberGeneratorServiceImpl>();

            _container.RegisterType<ICustomerOrderService, CustomerOrderServiceImpl>();
            _container.RegisterType<ICustomerOrderSearchService, CustomerOrderSearchServiceImpl>();
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            //Add order numbers formats settings  to store module allows to use individual number formats in each store
            var settingManager = _container.Resolve<ISettingsManager>();
            var numberFormatSettings = settingManager.GetModuleSettings("VirtoCommerce.Orders").Where(x=> x.Name.EndsWith("NewNumberTemplate")).ToArray();
            settingManager.RegisterModuleSettings("VirtoCommerce.Store", numberFormatSettings);

            var notificationManager = _container.Resolve<INotificationManager>();
            notificationManager.RegisterNotificationType(() => new OrderCreateEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Create order notification",
                Description = "This notification sends by email to client when he create order",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = OrderNotificationResource.CreateOrderNotificationBody,
                    Subject = OrderNotificationResource.CreateOrderNotificationSubject,
                    Language = "en-US"
                }
            });

            notificationManager.RegisterNotificationType(() => new OrderPaidEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Order paid notification",
                Description = "This notification sends by email to client when all payments of order has status paid",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = OrderNotificationResource.OrderPaidNotificationBody,
                    Subject = OrderNotificationResource.OrderPaidNotificationSubject,
                    Language = "en-US"
                }
            });

            notificationManager.RegisterNotificationType(() => new OrderSentEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Order sent notification",
                Description = "This notification sends by email to client when all shipments gets status sent",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = OrderNotificationResource.OrderSentNotificationBody,
                    Subject = OrderNotificationResource.OrderSentNotificationSubject,
                    Language = "en-US"
                }
            });

            notificationManager.RegisterNotificationType(() => new NewOrderStatusEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "New order status notification",
                Description = "This notification sends by email to client when status of orders has been changed",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = OrderNotificationResource.NewOrderStatusNotificationBody,
                    Subject = OrderNotificationResource.NewOrderStatusNotificatonSubject,
                    Language = "en-US"
                }
            });

            notificationManager.RegisterNotificationType(() => new CancelOrderEmailNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Cancel order notification",
                Description = "This notification sends by email to client when order canceled",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = OrderNotificationResource.CancelOrderNotificationBody,
                    Subject = OrderNotificationResource.CancelOrderNotificationSubject,
                    Language = "en-US"
                }
            });

            var securityScopeService = _container.Resolve<IPermissionScopeService>();
            securityScopeService.RegisterSope(() => new OrderStoreScope());
            securityScopeService.RegisterSope(() => new OrderResponsibleScope());
        }

        #endregion

        #region ISupportExportImportModule Members

        public void DoExport(System.IO.Stream outStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<OrderExportImport>();
            job.DoExport(outStream, progressCallback);
        }

        public void DoImport(System.IO.Stream inputStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var job = _container.Resolve<OrderExportImport>();
            job.DoImport(inputStream, progressCallback);
        }

        public string ExportDescription
        {
            get
            {
                var settingManager = _container.Resolve<ISettingsManager>();
                return settingManager.GetValue("Order.ExportImport.Description", String.Empty);
            }
        }
        #endregion

    }
}
