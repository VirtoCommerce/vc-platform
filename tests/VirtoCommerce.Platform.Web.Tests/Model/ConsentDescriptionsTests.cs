using System.Linq;
using VirtoCommerce.Platform.Web.Model;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Model;

/// <summary>
/// The consent page says in the user's words what the application asks for and where the user goes after they
/// allow it, so a user can tell an application that is not what it claims.
/// </summary>
public class ConsentDescriptionsTests
{
    [Fact]
    public void GetPermissions_AgentOfAnMcpEndpoint_DescribesTheResourceAndTheStandardScopes()
    {
        var permissions = ConsentDescriptions.GetPermissions(
            ["openid", "profile", "offline_access"], ["https://platform.example.com/admin/mcp"], "operator@example.com");

        Assert.Equal(["Work with your permissions", "Know who you are", "Stay signed in"], permissions.Select(x => x.Title));
        Assert.Equal("Everything your account is allowed to do at platform.example.com/admin/mcp", permissions[0].Details);
        Assert.Equal("Your name and user name", permissions[1].Details);
    }

    [Fact]
    public void GetPermissions_OpenIdWithoutProfile_TellsOnlyTheUserId()
    {
        var permissions = ConsentDescriptions.GetPermissions(["openid"], [], null);

        Assert.Equal("Your user ID", Assert.Single(permissions).Details);
    }

    [Fact]
    public void GetPermissions_Email_ShowsTheAddress()
    {
        var permissions = ConsentDescriptions.GetPermissions(["email"], [], "buyer@example.com");

        var email = Assert.Single(permissions);
        Assert.Equal("See your email address", email.Title);
        Assert.Equal("buyer@example.com", email.Details);
    }

    [Fact]
    public void GetPermissions_NoResource_SaysNothingAboutWork()
    {
        var permissions = ConsentDescriptions.GetPermissions(["openid"], [], null);

        Assert.DoesNotContain(permissions, x => x.Icon == "work");
    }

    [Fact]
    public void GetPermissions_ScopesOfTheirOwn_AreLeftToTheScopeManager()
    {
        var permissions = ConsentDescriptions.GetPermissions(["scp:catalog:read"], [], null);

        Assert.Empty(permissions);
        Assert.DoesNotContain("scp:catalog:read", ConsentDescriptions.StandardScopes);
    }

    [Theory]
    [InlineData("http://localhost:65111/callback", "localhost:65111", true, false)]
    [InlineData("http://127.0.0.1:33418/", "127.0.0.1:33418", true, false)]
    [InlineData("https://claude.ai/api/mcp/auth_callback", "claude.ai", false, true)]
    [InlineData("https://platform.example.com/signin", "platform.example.com", false, false)]
    [InlineData("https://Platform.Example.com/signin", "Platform.Example.com", false, false)]
    public void GetRedirect_TellsAnAppOnThisComputerAndAnotherSite(string redirectUri, string host, bool isLocal, bool isElsewhere)
    {
        var redirect = ConsentDescriptions.GetRedirect(redirectUri, "platform.example.com");

        Assert.Equal(host.ToLowerInvariant(), redirect.Host.ToLowerInvariant());
        Assert.Equal(isLocal, redirect.IsLocal);
        Assert.Equal(isElsewhere, redirect.IsElsewhere);
    }

    [Fact]
    public void GetResourceName_NotAUri_IsShownAsItIs()
    {
        Assert.Equal("resource_server", ConsentDescriptions.GetResourceName("resource_server"));
    }
}
