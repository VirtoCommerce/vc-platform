using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications
{
    [Trait("Category", "Unit")]
    public class PushNotificationsTests
    {
        private readonly Mock<IHubContext<PushNotificationHub>> _hubContextMock;

        public PushNotificationsTests()
        {
            var clientProxyMock = new Mock<IClientProxy>();

            var hubClientsMock = new Mock<IHubClients>();
            hubClientsMock.Setup(x => x.All).Returns(() => clientProxyMock.Object);
            
            _hubContextMock = new Mock<IHubContext<PushNotificationHub>>();
            _hubContextMock.Setup(x => x.Clients).Returns(() => hubClientsMock.Object);
        }

        [Fact]
        public void Should_Save_And_Search_PushNotification()
        {
            var pushNotificationManager = GetPushNotificationsManager();
            var pushNotification = new PushNotificationStub(null);
            pushNotificationManager.Send(pushNotification);
            Assert.Equal(pushNotification, pushNotificationManager.SearchNotifies(null, new PushNotificationSearchCriteria()).NotifyEvents.Single());
        }

        public ILogger<T> GetLogger<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        public PushNotificationManager GetPushNotificationsManager()
        {
            return new PushNotificationManager(new PushNotificationInMemoryStorage(), _hubContextMock.Object);
        }

        private class PushNotificationStub: PushNotification
        {
            public PushNotificationStub(string creator) : base(creator)
            {
            }
        }
    }
}
