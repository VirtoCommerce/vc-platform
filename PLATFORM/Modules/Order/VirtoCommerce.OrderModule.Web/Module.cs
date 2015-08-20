using System;
using System.Data.Entity;
using System.Dynamic;
using Microsoft.Practices.Unity;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.OrderModule.Data.Observers;
using VirtoCommerce.OrderModule.Data.Repositories;
using VirtoCommerce.OrderModule.Data.Services;
using VirtoCommerce.OrderModule.Web.ExportImport;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.OrderModule.Data.Notifications;
using VirtoCommerce.OrderModule.Web.Resources;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.OrderModule.Web
{
    public class Module : ModuleBase, ISupportExportImportModule
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void SetupDatabase()
        {
            using (var context = new OrderRepositoryImpl())
            {
				var initializer = new SetupDatabaseInitializer<OrderRepositoryImpl, Data.Migrations.Configuration>();
				initializer.InitializeDatabase(context);
            }
        }

        public override void Initialize()
        {
            _container.RegisterType<IEventPublisher<OrderChangeEvent>, EventPublisher<OrderChangeEvent>>();
            //Subscribe to order changes. Calculate totals   
            _container.RegisterType<IObserver<OrderChangeEvent>, CalculateTotalsObserver>("CalculateTotalsObserver");
            //Adjust inventory activity
            _container.RegisterType<IObserver<OrderChangeEvent>, AdjustInventoryObserver>("AdjustInventoryObserver");
            //Create order observer. Send notification
            _container.RegisterType<IObserver<OrderChangeEvent>, CreateOrderObserver>("CreateOrderObserver");
            //Cancel payment observer. Payment method cancel operations
            _container.RegisterType<IObserver<OrderChangeEvent>, CancelPaymentObserver>("CancelPaymentObserver");

            _container.RegisterType<IOrderRepository>(new InjectionFactory(c => new OrderRepositoryImpl("VirtoCommerce", new AuditableInterceptor(), new EntityPrimaryKeyGeneratorInterceptor())));
            //_container.RegisterInstance<IInventoryService>(new Mock<IInventoryService>().Object);
            _container.RegisterType<IOperationNumberGenerator, TimeBasedNumberGeneratorImpl>();

            _container.RegisterType<ICustomerOrderService, CustomerOrderServiceImpl>();
            _container.RegisterType<ICustomerOrderSearchService, CustomerOrderSearchServiceImpl>();
        }

        public override void PostInitialize()
        {
            var cacheManager = _container.Resolve<CacheManager>();
            var cacheSettings = new[] 
			{
				new CacheSettings("Statistic", TimeSpan.FromHours(1))
			};
            cacheManager.AddCacheSettings(cacheSettings);

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
