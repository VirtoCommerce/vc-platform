using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class OAuthAppsControllerTests
{
    [Fact]
    public void New_GrantsRefreshTokenPermission()
    {
        var controller = new OAuthAppsController(null!);

        var descriptor = controller.New().Value;

        Assert.Contains(
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            descriptor.Permissions);
    }
}
