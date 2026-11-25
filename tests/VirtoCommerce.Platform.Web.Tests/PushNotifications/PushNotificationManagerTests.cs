using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.PushNotifications
{
    public class PushNotificationManagerTests
    {
        [Fact]
        public async Task SendAsync_BroadcastsOnlyToSystemEventsAndCreatorGroups()
        {
            var userProxy = new Mock<IClientProxy>();
            var groupsProxy = new Mock<IClientProxy>();
            var clients = new Mock<IHubClients>();
            clients.Setup(x => x.User("alice")).Returns(userProxy.Object);
            clients.Setup(x => x.Groups(It.IsAny<IReadOnlyList<string>>())).Returns(groupsProxy.Object);

            var hubContext = new Mock<IHubContext<PushNotificationHub>>();
            hubContext.SetupGet(x => x.Clients).Returns(clients.Object);

            var manager = new PushNotificationManager(Mock.Of<IPushNotificationStorage>(), hubContext.Object);
            var notification = new TestPushNotification("alice");

            await manager.SendAsync(notification);

            userProxy.Verify(x => x.SendCoreAsync("Send", It.Is<object[]>(a => ReferenceEquals(a[0], notification)), It.IsAny<CancellationToken>()), Times.Once);
            clients.Verify(x => x.Groups(It.Is<IReadOnlyList<string>>(g =>
                g.Count == 2 &&
                g[0] == PushNotificationHub.SystemEventsGroupName &&
                g[1] == PushNotificationHub.GetCreatorGroupName("alice"))), Times.Once);
            groupsProxy.Verify(x => x.SendCoreAsync("SendSystemEvents", It.Is<object[]>(a => ReferenceEquals(a[0], notification)), It.IsAny<CancellationToken>()), Times.Once);
            clients.VerifyGet(x => x.All, Times.Never);
        }

        [Fact]
        public async Task SendAsync_NoCreator_SendsNothing()
        {
            var hubContext = new Mock<IHubContext<PushNotificationHub>>(MockBehavior.Strict);
            var manager = new PushNotificationManager(Mock.Of<IPushNotificationStorage>(), hubContext.Object);

            await manager.SendAsync(new TestPushNotification(null));

            hubContext.VerifyNoOtherCalls();
        }

        private sealed class TestPushNotification(string creator) : PushNotification(creator);
    }
}
