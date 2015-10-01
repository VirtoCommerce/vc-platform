using System;
using Microsoft.Practices.Unity;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.MerchandisingModule.Web.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.MerchandisingModule.Web.Model.Notificaitons;

namespace VirtoCommerce.MerchandisingModule.Web
{
    public class Module : ModuleBase
    {
        private readonly IUnityContainer _container;

        public Module(IUnityContainer container)
        {
            _container = container;
        }

        #region IModule Members

        public override void Initialize()
        {
            _container.RegisterType<IBrowseFilterService, FilterService>();
            _container.RegisterType<IItemBrowsingService, ItemBrowsingService>();
        }

        public override void PostInitialize()
        {
            var settingsManager = _container.Resolve<ISettingsManager>();
            var cacheManager = _container.Resolve<CacheManager>();
            var cacheSettings = new[] 
			{
				new CacheSettings("MP", TimeSpan.FromMinutes(settingsManager.GetValue("MerchandisingModule.Caching.Timeout", 5)))
			};
            cacheManager.AddCacheSettings(cacheSettings);

            var notificationManager = _container.Resolve<INotificationManager>();

            notificationManager.RegisterNotificationType(() => new DynamicMerchandisingNotification(_container.Resolve<IEmailNotificationSendingGateway>())
            {
                DisplayName = "Sending custom form from storefront",
                Description = "This notification sends by email to client when he complite some form on storefront, for example contact us form",
                NotificationTemplate = new NotificationTemplate
                {
                    Body = "",
                    Subject = "",
                    Language = "en-US"
                }
            });

        }

        #endregion
    }
}
