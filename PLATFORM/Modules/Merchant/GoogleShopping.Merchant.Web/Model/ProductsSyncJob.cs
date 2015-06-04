using System.Threading;
using GoogleShopping.MerchantModule.Web.Model.Notifications;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Notification;

namespace GoogleShopping.MerchantModule.Web.Model
{
    public class ProductsSyncJob
    {
        public string Id { get; set; }
        public string Name { get; set; }

		[JsonIgnore]
		public ProductSyncNotifyEvent NotifyEvent { get; set; }
		[JsonIgnore]
		public INotifier Notifier { get; set; }
		
		[JsonIgnore]
		public CancellationTokenSource CancellationToken;
		
    }
}