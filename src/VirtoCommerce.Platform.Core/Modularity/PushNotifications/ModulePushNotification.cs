using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Core.Modularity.PushNotifications
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
        public ICollection<ProgressMessage> ProgressLog { get; set; }

        /// <summary>
        /// Gets the count of errors during processing.
        /// </summary>
        /// <value>
        /// The error count.
        /// </value>
        [JsonProperty("errorCount")]
        public int ErrorCount => ProgressLog?.Count(x => x.Level == ProgressMessageLevel.Error) ?? 0;
    }
}
