using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;
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

        [Fact]
        public async System.Threading.Tasks.Task MarkAllAsRead_UpdatesStorageWithoutSendingToHub()
        {
            var storage = new Mock<IPushNotificationStorage>();
            storage.Setup(x => x.MarkAllAsReadAsync("alice", It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync([new PushNotificationStub("alice") { IsNew = false }]);
            var hubContext = new Mock<IHubContext<PushNotificationHub>>(MockBehavior.Strict);
            var manager = new PushNotificationManager(storage.Object, hubContext.Object);

            var marked = await manager.MarkAllAsReadAsync("alice", TestContext.Current.CancellationToken);

            Assert.Single(marked);
            hubContext.VerifyNoOtherCalls();
        }

        [Fact]
        public async System.Threading.Tasks.Task SendAsync_HistoryWriteFails_StillSendsToHub()
        {
            var storage = new Mock<IPushNotificationStorage>();
            storage.Setup(x => x.SavePushNotificationAsync(It.IsAny<PushNotification>())).ThrowsAsync(new System.InvalidOperationException("Redis timeout"));
            var userProxy = new Mock<IClientProxy>();
            var clients = new Mock<IHubClients>();
            clients.Setup(x => x.User("alice")).Returns(userProxy.Object);
            clients.Setup(x => x.All).Returns(new Mock<IClientProxy>().Object);
            var hubContext = new Mock<IHubContext<PushNotificationHub>>();
            hubContext.Setup(x => x.Clients).Returns(clients.Object);
            var manager = new PushNotificationManager(storage.Object, hubContext.Object, GetLogger<PushNotificationManager>());
            var notification = new PushNotificationStub("alice");

            await manager.SendAsync(notification);

            userProxy.Verify(x => x.SendCoreAsync("Send", It.Is<object[]>(a => ReferenceEquals(a[0], notification)), It.IsAny<System.Threading.CancellationToken>()), Times.Once);
        }

        public ILogger<T> GetLogger<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        public PushNotificationManager GetPushNotificationsManager()
        {
            return new PushNotificationManager(new PushNotificationInMemoryStorage(Options.Create(new PushNotificationOptions())), _hubContextMock.Object);
        }

        private class PushNotificationStub: PushNotification
        {
            public PushNotificationStub(string creator) : base(creator)
            {
            }
        }
    }
}
