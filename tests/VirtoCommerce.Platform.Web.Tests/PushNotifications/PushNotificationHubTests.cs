using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Moq;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Web.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.PushNotifications
{
    public class PushNotificationHubTests
    {
        [Fact]
        public async Task OnConnected_UserMayReadAll_JoinsOnlySystemEventsGroup()
        {
            var joined = await ConnectAsync("alice", canReadAll: true, audience: ["seller-1"]);

            joined.Should().Equal(PushNotificationHub.SystemEventsGroupName);
        }

        [Fact]
        public async Task OnConnected_UserMayNotReadAll_JoinsOwnCreatorGroupOnly()
        {
            var joined = await ConnectAsync("alice", canReadAll: false);

            joined.Should().Equal(PushNotificationHub.GetCreatorGroupName("alice"));
        }

        [Fact]
        public async Task OnConnected_AudienceProviders_AddTheirCreatorsOnce()
        {
            var joined = await ConnectAsync("alice", canReadAll: false, audience: ["seller-1", "SELLER-1", null, "Alice"]);

            joined.Should().BeEquivalentTo(PushNotificationHub.GetCreatorGroupName("alice"), PushNotificationHub.GetCreatorGroupName("seller-1"));
        }

        [Fact]
        public void GetCreatorGroupName_IgnoresCase()
        {
            PushNotificationHub.GetCreatorGroupName("Alice").Should().Be(PushNotificationHub.GetCreatorGroupName("aLICE"));
        }

        private static async Task<List<string>> ConnectAsync(string userName, bool canReadAll, string[] audience = null)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, userName)], "test"));

            var authorizationService = new Mock<IAuthorizationService>();
            authorizationService
                .Setup(x => x.AuthorizeAsync(user, null, It.Is<IEnumerable<IAuthorizationRequirement>>(r =>
                    r.OfType<PermissionAuthorizationRequirement>().Single().Permission == PlatformConstants.Security.Permissions.PushNotificationsReadAll)))
                .ReturnsAsync(canReadAll ? AuthorizationResult.Success() : AuthorizationResult.Failed());

            var providers = new List<IPushNotificationAudienceProvider>();
            if (audience != null)
            {
                var provider = new Mock<IPushNotificationAudienceProvider>();
                provider.Setup(x => x.GetCreatorsAsync(user)).ReturnsAsync(audience);
                providers.Add(provider.Object);
            }

            var context = new Mock<HubCallerContext>();
            context.SetupGet(x => x.ConnectionId).Returns("connection-1");
            context.SetupGet(x => x.UserIdentifier).Returns(userName);
            context.SetupGet(x => x.User).Returns(user);

            var joined = new List<string>();
            var groups = new Mock<IGroupManager>();
            groups
                .Setup(x => x.AddToGroupAsync("connection-1", It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<string, string, CancellationToken>((_, group, _) => joined.Add(group))
                .Returns(Task.CompletedTask);

            var hub = new PushNotificationHub(authorizationService.Object, providers)
            {
                Context = context.Object,
                Groups = groups.Object,
            };

            await hub.OnConnectedAsync();

            return joined;
        }
    }
}
