using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Notification
{
    public class NotifyEvent
    {
        public NotifyEvent(string creator)
        {
            Created = DateTime.UtcNow;
            New = true;
            Id = Guid.NewGuid().ToString();
            Creator = creator;
            ExtendedData = new Dictionary<string, string>();
        }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("new")]
        public bool New { get; set; }
        [JsonProperty("notifyType")]
        public string NotifyType { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("repeatCount")]
        public int RepeatCount { get; set; }
        [JsonProperty("extendedData")]
        public IDictionary<string, string> ExtendedData { get; set; }

        public bool ItHasSameContent(NotifyEvent other)
        {
            return other.Title == Title && other.NotifyType == NotifyType && other.Description == Description;
        }

    }
}
