using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public sealed class DistributedPushNotification: PushNotification
    {
        public DistributedPushNotification() : base(null)
        {
        }

        public string ServerId { get; set; }

        public Dictionary<string, object> AdditionalProperties { get; set; }
    }
}
