using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public class PushNotificationSearchResult : ValueObject
    {
        public PushNotificationSearchResult()
        {
            NotifyEvents = new List<PushNotification>();
        }
        public int TotalCount { get; set; }
        public int NewCount { get; set; }
        public List<PushNotification> NotifyEvents { get; set; }
    }
}
