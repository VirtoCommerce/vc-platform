using System;
using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Data.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications
{
    [Trait("Category", "Unit")]
    public class PushNotificationJsonSerializerTests
    {
        [Fact]
        public void SerializeDeserialize_KeepsTypeAndFields()
        {
            var notification = new StorageTestNotification("admin", "Indexation") { ProcessedCount = 42, ServerId = "server-1", RepeatCount = 2 };

            var restored = PushNotificationJsonSerializer.Deserialize(PushNotificationJsonSerializer.Serialize(notification));

            restored.Should().BeOfType<StorageTestNotification>().Which.Should().BeEquivalentTo(notification);
        }

        [Fact]
        public void Deserialize_TypeThatIsNotANotification_Throws()
        {
            var value = JsonConvert.SerializeObject(new { Type = typeof(Uri).FullName, Assembly = typeof(Uri).Assembly.GetName().Name, Notification = "{}" });

            var act = () => PushNotificationJsonSerializer.Deserialize(value);

            act.Should().Throw<JsonSerializationException>();
        }
    }
}
