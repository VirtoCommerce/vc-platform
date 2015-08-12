using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.Model.Packaging
{
    public class ModulePushNotification : PushNotification
    {
        public ModulePushNotification(string creator)
            : base(creator)
        {
            ProgressLog = new List<ProgressMessage>();
        }

        [JsonProperty("started")]
        public DateTime? Started { get; set; }

        [JsonProperty("finished")]
        public DateTime? Finished { get; set; }

        [JsonProperty("progressLog")]
        public ICollection<ProgressMessage> ProgressLog;
    }
}
