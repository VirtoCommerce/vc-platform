using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class OAuthAppsSelfRegisteredAppTests
{
    private const string ClientId = "https://agent.example.com/client.json";

    private readonly SelfRegisteredClientTestContext _context = new();
    private readonly VirtoOpenIddictApplicationManager _manager;

    public OAuthAppsSelfRegisteredAppTests()
    {
        _manager = _context.CreateManager();
    }

    [Fact]
    public async Task Get_SelfRegisteredApp_IsStoredOneWithoutFetchingItsDocument()
    {
        // Arrange
        await StoreSelfRegisteredAppAsync();
        _context.Options.ClientRegistration.ClientIdMetadataDocuments.Enabled = false;
        var controller = CreateController();

        // Act
        var result = await controller.GetAsync(ClientId);

        // Assert
        result.Value.Should().NotBeNull("the administrator opens the stored application even when documents are turned off");
        result.Value!.DisplayName.Should().Be("Agent");
        _context.Resolver.Verify(x => x.ResolveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Save_SelfRegisteredApp_ReturnsBadRequestAndKeepsIt()
    {
        // Arrange
        await StoreSelfRegisteredAppAsync();
        var controller = CreateController();
        var descriptor = (await controller.GetAsync(ClientId)).Value!;
        descriptor.DisplayName = "Renamed";

        // Act
        var result = await controller.SaveAsync(descriptor);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeOfType<SecurityResult>()
            .Which.Errors.Should().ContainSingle().Which.Should().Contain("registered itself");
        _context.Updates.Should().Be(0);
    }

    [Fact]
    public async Task Save_NewAppWithRegistrationProperty_IsNotMarkedSelfRegistered()
    {
        // Arrange
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "admin-agent",
            DisplayName = "Admin agent",
            ClientType = ClientTypes.Public,
            ApplicationType = ApplicationTypes.Native,
            ConsentType = ConsentTypes.Explicit,
            RedirectUris = { new Uri("http://localhost/callback") },
            Permissions = { Permissions.GrantTypes.AuthorizationCode },
        };
        descriptor.Properties[SelfRegisteredClientConstants.RegistrationProperty] = JsonSerializer.SerializeToElement(SelfRegisteredClientConstants.DynamicRegistration);
        var controller = CreateController();

        // Act
        await controller.SaveAsync(descriptor);

        // Assert
        (await _manager.IsSelfRegisteredAsync(_context.Stored["admin-agent"], TestContext.Current.CancellationToken)).Should().BeFalse();
    }

    private async Task StoreSelfRegisteredAppAsync()
    {
        var descriptor = await _context.Factory.CreateDescriptorAsync(SelfRegisteredClientConstants.ClientIdMetadataDocument, ClientId, "Agent",
            ["http://127.0.0.1/callback"], TestContext.Current.CancellationToken);
        await _manager.CreateAsync(descriptor, TestContext.Current.CancellationToken);
    }

    private OAuthAppsController CreateController()
    {
        return new OAuthAppsController(_manager, [], new OAuthAppRules(),
            Options.Create(new AuthorizationOptions()), NullLogger<OAuthAppsController>.Instance)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }
}
