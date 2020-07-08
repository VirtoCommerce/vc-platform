using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public class DistributedPushNotificationJsonConverter: JsonConverter
    {
        private static readonly string[] _knownProperties = typeof(DistributedPushNotification).GetProperties().Select(x => x.Name).ToArray();

        public override bool CanWrite => true;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return typeof(DistributedPushNotification).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var notificationProperties = serializer.Deserialize<Dictionary<string, object>>(reader).ToDictionary(x => x.Key.FirstCharToUpper(), x => x.Value);
            var result = notificationProperties == null ? null : new DistributedPushNotification
            {
                Id = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.Id), default(string)),
                ServerId = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.ServerId), default(string)),
                Created = (DateTime)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.Created), DateTime.UtcNow),
                Creator = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.Creator), default(string)),
                IsNew = (bool)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.IsNew), true),
                NotifyType = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.NotifyType), default(string)),
                Description = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.Description), default(string)),
                Title = (string)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.Title), default(string)),
                RepeatCount = (int)notificationProperties.GetValueOrDefault(nameof(DistributedPushNotification.RepeatCount), default(int)),
                AdditionalProperties = notificationProperties.Where(x => !_knownProperties.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value)
            };
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var notification = (DistributedPushNotification)value;
            var notificationProperties = new Dictionary<string, object>(notification.AdditionalProperties)
            {
                {nameof(DistributedPushNotification.Id), notification.Id},
                {nameof(DistributedPushNotification.ServerId), notification.ServerId},
                {nameof(DistributedPushNotification.Created), notification.Created},
                {nameof(DistributedPushNotification.Creator), notification.Creator},
                {nameof(DistributedPushNotification.IsNew), notification.IsNew},
                {nameof(DistributedPushNotification.NotifyType), notification.NotifyType},
                {nameof(DistributedPushNotification.Description), notification.Description},
                {nameof(DistributedPushNotification.Title), notification.Title},
                {nameof(DistributedPushNotification.RepeatCount), notification.RepeatCount}
            }.ToDictionary(x => x.Key.FirstCharToLower(), x => x.Value);
            serializer.Serialize(writer, notificationProperties);
        }
    }
}
