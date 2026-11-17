using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class ClientRegistrationControllerTests
{
    private readonly SelfRegisteredClientTestContext _context = new();

    public ClientRegistrationControllerTests()
    {
        _context.Options.ClientRegistration.DynamicRegistration.Enabled = true;
    }

    [Fact]
    public async Task Register_Disabled_NotFound()
    {
        // Arrange
        _context.Options.ClientRegistration.DynamicRegistration.Enabled = false;

        // Act
        var result = await RegisterAsync("""{ "redirect_uris": ["https://agent.example.com/callback"] }""");

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _context.Stored.Should().BeEmpty();
    }

    [Fact]
    public async Task Register_PublicClient_StoresTheLeastItNeedsAndTellsTheClient()
    {
        // Act
        var result = await RegisterAsync("""
            {
              "client_name": "Agent",
              "redirect_uris": ["http://127.0.0.1:33418/callback"],
              "grant_types": ["authorization_code", "refresh_token", "client_credentials"],
              "token_endpoint_auth_method": "none",
              "scope": "openid profile email"
            }
            """);

        // Assert
        var created = result.Should().BeOfType<ObjectResult>().Subject;
        created.StatusCode.Should().Be(StatusCodes.Status201Created);
        var body = created.Value.Should().BeAssignableTo<IDictionary<string, object>>().Subject;
        var clientId = (string)body[Parameters.ClientId];
        body["grant_types"].Should().BeEquivalentTo(new[] { GrantTypes.AuthorizationCode, GrantTypes.RefreshToken });
        body["token_endpoint_auth_method"].Should().Be(ClientAuthenticationMethods.None);
        var manager = _context.CreateManager();
        var application = await manager.FindByClientIdAsync(clientId, TestContext.Current.CancellationToken);
        (await manager.GetRegistrationTypeAsync(application, TestContext.Current.CancellationToken)).Should().Be(SelfRegisteredClientConstants.DynamicRegistration);
        (await manager.GetDisplayNameAsync(application, TestContext.Current.CancellationToken)).Should().Be("Agent");
        (await manager.HasClientTypeAsync(application, ClientTypes.Public, TestContext.Current.CancellationToken)).Should().BeTrue();
        (await manager.HasPermissionAsync(application, Permissions.GrantTypes.ClientCredentials, TestContext.Current.CancellationToken)).Should().BeFalse();
        (await manager.HasPermissionAsync(application, Permissions.Scopes.Email, TestContext.Current.CancellationToken)).Should().BeFalse();
        (await manager.HasRequirementAsync(application, Requirements.Features.ProofKeyForCodeExchange, TestContext.Current.CancellationToken)).Should().BeTrue();
    }

    [Fact]
    public async Task Register_NoAuthMethod_RegisteredAsAPublicClient()
    {
        // Act
        var result = await RegisterAsync("""{ "redirect_uris": ["https://agent.example.com/callback"] }""");

        // Assert
        var body = (IDictionary<string, object>)((ObjectResult)result).Value;
        body["token_endpoint_auth_method"].Should().Be(ClientAuthenticationMethods.None);
        body["client_name"].Should().Be("Unnamed client");
    }

    [Theory]
    [InlineData("""{ "redirect_uris": ["https://agent.example.com/callback"], "token_endpoint_auth_method": "client_secret_basic" }""", "invalid_client_metadata")]
    [InlineData("""{ "redirect_uris": ["https://agent.example.com/callback"], "grant_types": ["client_credentials"] }""", "invalid_client_metadata")]
    [InlineData("""{ "redirect_uris": ["https://agent.example.com/callback"], "response_types": ["token"] }""", "invalid_client_metadata")]
    [InlineData("""{ "redirect_uris": ["https://agent.example.com/callback"], "grant_types": "authorization_code" }""", "invalid_client_metadata")]
    [InlineData("""{ "client_name": "Agent" }""", "invalid_redirect_uri")]
    [InlineData("""{ "redirect_uris": ["http://agent.example.com/callback"] }""", "invalid_redirect_uri")]
    [InlineData("""{ "redirect_uris": ["javascript:alert(1)"] }""", "invalid_redirect_uri")]
    [InlineData("""[]""", "invalid_client_metadata")]
    [InlineData("""not json""", "invalid_client_metadata")]
    public async Task Register_InvalidMetadata_RefusedWithTheError(string json, string error)
    {
        // Act
        var result = await RegisterAsync(json);

        // Assert
        var body = result.Should().BeOfType<BadRequestObjectResult>().Subject.Value.Should().BeAssignableTo<IDictionary<string, string>>().Subject;
        body[Parameters.Error].Should().Be(error);
        _context.Stored.Should().BeEmpty();
    }

    private async Task<IActionResult> RegisterAsync(string json)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.ContentType = "application/json";
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));

        var registrationService = new DynamicClientRegistrationService(
            new ClientMetadataValidator(new OAuthAppRules()),
            _context.Factory,
            _context.CreateManager(),
            NullLogger<DynamicClientRegistrationService>.Instance);
        var controller = new ClientRegistrationController(
            registrationService,
            Mock.Of<IOptionsMonitor<AuthorizationOptions>>(x => x.CurrentValue == _context.Options))
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext },
        };

        return await controller.Register();
    }
}
