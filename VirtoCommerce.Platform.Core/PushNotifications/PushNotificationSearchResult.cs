using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public class PushNotificationSearchResult
    {
        public PushNotificationSearchResult()
        {
            NotifyEvents = new List<PushNotification>();
        }

        public int TotalCount { get; set; }
        public int NewCount { get; set; }
        public IList<PushNotification> NotifyEvents { get; set; }
    }
}
