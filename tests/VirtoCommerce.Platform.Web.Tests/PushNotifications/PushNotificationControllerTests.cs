using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.PushNotifications;

public class PushNotificationControllerTests
{
    [Fact]
    public async Task MarkAllAsRead_ReturnsTheMarkedNotificationsOfTheSignedInUser()
    {
        var marked = new List<PushNotification> { new TestNotification("alice"), new TestNotification("alice") };
        var manager = new Mock<IPushNotificationManager>();
        manager.Setup(x => x.MarkAllAsReadAsync("alice", It.IsAny<CancellationToken>())).ReturnsAsync(marked);
        var controller = CreateController(manager.Object, "alice");

        var response = await controller.MarkAllAsRead();

        var result = response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<PushNotificationSearchResult>().Subject;
        result.NotifyEvents.Should().Equal(marked);
        result.TotalCount.Should().Be(2);
        result.NewCount.Should().Be(2);
        manager.Verify(x => x.SendAsync(It.IsAny<PushNotification>()), Times.Never);
    }

    [Fact]
    public async Task Search_SearchesTheSignedInUsersNotifications()
    {
        var criteria = new PushNotificationSearchCriteria { Take = 5 };
        var expected = new PushNotificationSearchResult { TotalCount = 1 };
        var manager = new Mock<IPushNotificationManager>();
        manager.Setup(x => x.SearchNotifiesAsync("alice", criteria, It.IsAny<CancellationToken>())).ReturnsAsync(expected);
        var controller = CreateController(manager.Object, "alice");

        var response = await controller.SearchPushNotification(criteria);

        response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeSameAs(expected);
    }

    private static PushNotificationController CreateController(IPushNotificationManager manager, string userName)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Name, userName)], "test"));

        return new PushNotificationController(manager)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } },
        };
    }

    private sealed class TestNotification(string creator) : PushNotification(creator);
}
