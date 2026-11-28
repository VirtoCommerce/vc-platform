using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// Stores a notification with its concrete type, and restores only <see cref="PushNotification"/> types.
    /// </summary>
    public static class PushNotificationJsonSerializer
    {
        private static readonly JsonSerializerSettings _settings = new() { TypeNameHandling = TypeNameHandling.None };
        private static readonly DefaultSerializationBinder _binder = new DefaultSerializationBinder();

        public static string Serialize(PushNotification notification)
        {
            var type = notification.GetType();

            return JsonConvert.SerializeObject(new Envelope
            {
                Type = type.FullName,
                Assembly = type.Assembly.GetName().Name,
                Notification = JsonConvert.SerializeObject(notification, _settings),
            });
        }

        public static PushNotification Deserialize(string value)
        {
            var envelope = JsonConvert.DeserializeObject<Envelope>(value);
            if (envelope?.Type == null || envelope.Notification == null)
            {
                return null;
            }

            var type = _binder.BindToType(envelope.Assembly, envelope.Type);
            if (!typeof(PushNotification).IsAssignableFrom(type) || type.IsAbstract)
            {
                throw new JsonSerializationException($"Type '{envelope.Type}, {envelope.Assembly}' is not a push notification.");
            }

            return (PushNotification)JsonConvert.DeserializeObject(envelope.Notification, type, _settings);
        }

        private sealed class Envelope
        {
            public string Type { get; set; }
            public string Assembly { get; set; }
            public string Notification { get; set; }
        }
    }
}
