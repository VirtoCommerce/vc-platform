using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public sealed class DistributedPushNotification: PushNotification
    {
        public DistributedPushNotification() : base(null)
        {
            AdditionalProperties = new Dictionary<string, object>();
        }

        public string ServerId { get; set; }

        public Dictionary<string, object> AdditionalProperties { get; set; }
    }
}
