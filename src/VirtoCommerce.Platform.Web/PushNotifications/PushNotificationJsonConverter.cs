using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// PushNotification json converter for System.Text.Json
    /// it needs to support polymorfic desiarilization of derived classes from PushNotification
    /// https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-migrate-from-newtonsoft-how-to#scenarios-that-jsonserializer-currently-doesnt-support
    /// https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-converters-how-to#support-polymorphic-deserialization
    /// </summary>
    public class PushNotificationJsonConverter : JsonConverter<PushNotification>
    {
        const string PUSH_NOTIFICATION_TYPE_PROPERTY_NAME = "notifyType";

        public override bool CanConvert(Type type)
        {
            var result = typeof(PushNotification).IsAssignableFrom(type);
            return result;
        }

        public override PushNotification Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {            
            while ( reader.Read() )
            {
                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == PUSH_NOTIFICATION_TYPE_PROPERTY_NAME)
                {
                    break;
                }
            }

            reader.Read();
            var typeName = reader.GetString();

            var notificationType = PushNotificationTypeFactory.GetType(typeName);

            Debug.WriteLine(notificationType.FullName);

            // TODO: make real Json deserialization to required type
            // for polymorphic support we need to change dto in such way. It cause frontend code changing. 
            // https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-converters-how-to#support-polymorphic-deserialization
            // or use newtonsoft enstead of System.Text.Json
            // https://docs.microsoft.com/ru-ru/aspnet/core/migration/22-to-30?view=aspnetcore-3.1&tabs=visual-studio#use-newtonsoftjson-in-an-aspnet-core-30-signalr-project
            var notification = Activator.CreateInstance(notificationType) as PushNotification;
            JsonSerializer.Deserialize(ref reader, notificationType);

            return notification;

        }

        public override void Write(Utf8JsonWriter writer, PushNotification value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value);
        }
    }
}
