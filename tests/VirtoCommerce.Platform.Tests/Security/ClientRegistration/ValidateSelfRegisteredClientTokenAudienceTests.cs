using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Validation.OpenIddictValidationEvents;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class ValidateSelfRegisteredClientTokenAudienceTests
{
    private const string ClientId = "https://claude.ai/oauth/claude-code-client-metadata";
    private const string ResourcePath = "/admin/mcp";

    private readonly SelfRegisteredClientTestContext _context = new();
    private readonly VirtoOpenIddictApplicationManager _manager;

    public ValidateSelfRegisteredClientTokenAudienceTests()
    {
        _manager = _context.CreateManager();
    }

    [Theory]
    [InlineData(ResourcePath)]
    [InlineData(ResourcePath + "/")]
    [InlineData(ResourcePath + "/x")]
    public async Task Handle_SelfRegisteredClientTokenAtItsResource_NotRejected(string path)
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        var context = CreateContext(ClientId, SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler(path).HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_SelfRegisteredClientTokenUnderPathBase_NotRejected()
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        var context = CreateContext(ClientId, SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler("/mcp", pathBase: "/admin").HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Theory]
    [InlineData("/api/platform/security/users")]
    [InlineData(ResourcePath + "x")]
    public async Task Handle_SelfRegisteredClientTokenAtAnotherPath_Rejected(string path)
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        var context = CreateContext(ClientId, SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler(path).HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeTrue();
        context.Error.Should().Be(Errors.InvalidToken);
    }

    [Fact]
    public async Task Handle_AdminRegisteredClientTokenAtAnotherPath_NotRejected()
    {
        // Arrange
        _context.Stored["admin-agent"] = new VirtoOpenIddictEntityFrameworkCoreApplication { ClientId = "admin-agent" };
        var context = CreateContext("admin-agent", SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler("/api/platform/security/users").HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TokenWithoutResourceUrl_NotRejected()
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        var context = CreateContext(ClientId, "resource_server");

        // Act
        await CreateHandler("/api/platform/security/users").HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_TokenWithoutClientId_NotRejected()
    {
        // Arrange
        var context = CreateContext(null, SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler("/api/platform/security/users").HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_UnknownClientWithResourceUrl_Rejected()
    {
        // Arrange
        var context = CreateContext("unknown-client", SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler(ResourcePath).HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeTrue();
        context.Error.Should().Be(Errors.InvalidToken);
        context.ErrorDescription.Should().Contain("not available");
    }

    [Fact]
    public async Task Handle_SelfRegisteredClientWhoseDocumentIsNoLongerAvailable_NotRejectedWithoutResolvingIt()
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        _context.Documents.Clear();
        _context.Resolver.Invocations.Clear();
        var context = CreateContext(ClientId, SelfRegisteredClientTestContext.McpResource);

        // Act
        await CreateHandler(ResourcePath).HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeFalse();
        _context.Resolver.Verify(x => x.ResolveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("https://platform.example.com")]
    [InlineData("https://platform.example.com/")]
    public async Task Handle_SelfRegisteredClientTokenForRootResource_Rejected(string resource)
    {
        // Arrange
        await StoreSelfRegisteredClientAsync();
        var context = CreateContext(ClientId, resource);

        // Act
        await CreateHandler("/api/platform/security/users").HandleAsync(context);

        // Assert
        context.IsRejected.Should().BeTrue();
        context.Error.Should().Be(Errors.InvalidToken);
    }

    private async Task StoreSelfRegisteredClientAsync()
    {
        _context.SetDocument(ClientId, "Claude Code", "https://claude.ai/api/mcp/auth_callback");
        var application = await _manager.FindByClientIdAsync(ClientId, TestContext.Current.CancellationToken);
        await _manager.EnsureCreatedAsync(application, TestContext.Current.CancellationToken);
    }

    private ValidateSelfRegisteredClientTokenAudience CreateHandler(string path, string pathBase = null)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.PathBase = pathBase;
        httpContext.Request.Path = path;

        return new ValidateSelfRegisteredClientTokenAudience(_context.CreateManager(), new HttpContextAccessor { HttpContext = httpContext });
    }

    private static ProcessAuthenticationContext CreateContext(string clientId, params string[] audiences)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        principal.SetAudiences(audiences);

        if (clientId != null)
        {
            principal.SetClaim(Claims.ClientId, clientId);
        }

        return new ProcessAuthenticationContext(new OpenIddictValidationTransaction())
        {
            AccessTokenPrincipal = principal,
        };
    }
}
