using System.Threading;
using GoogleShopping.MerchantModule.Web.Model.Notifications;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace GoogleShopping.MerchantModule.Web.Model
{
    public class ProductsSyncJob
    {
        public string Id { get; set; }
        public string Name { get; set; }

		[JsonIgnore]
		public ProductSyncNotifyEvent NotifyEvent { get; set; }
		[JsonIgnore]
		public IPushNotificationManager PushNotifier { get; set; }
		
		[JsonIgnore]
		public CancellationTokenSource CancellationToken;
		
    }
}