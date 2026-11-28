using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;
using PlatformPermissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Tests.Security;

public class OAuthAppsControllerTests
{
    private const string StoredClientId = "integration";

    private readonly Mock<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>> _manager;

    public OAuthAppsControllerTests()
    {
        // The cache and store are never reached - they exist because the base constructor rejects nulls.
        _manager = new Mock<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>(
            Mock.Of<IOpenIddictApplicationCache<VirtoOpenIddictEntityFrameworkCoreApplication>>(),
            NullLogger<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>.Instance,
            Mock.Of<IOptionsMonitor<OpenIddictCoreOptions>>(x => x.CurrentValue == new OpenIddictCoreOptions()),
            Mock.Of<IOpenIddictApplicationStore<VirtoOpenIddictEntityFrameworkCoreApplication>>());

        var stored = new VirtoOpenIddictEntityFrameworkCoreApplication { Id = "app-1", ClientId = StoredClientId, ClientSecret = "hashed" };

        _manager
            .Setup(x => x.FindByClientIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((string clientId, CancellationToken _) => ValueTask.FromResult(clientId == StoredClientId ? stored : null));
        _manager
            .Setup(x => x.PopulateAsync(It.IsAny<OpenIddictApplicationDescriptor>(), It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .Returns((OpenIddictApplicationDescriptor descriptor, VirtoOpenIddictEntityFrameworkCoreApplication app, CancellationToken _) =>
            {
                descriptor.ClientId = app.ClientId;
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                return ValueTask.CompletedTask;
            });
        _manager
            .Setup(x => x.CreateAsync(It.IsAny<OpenIddictApplicationDescriptor>(), It.IsAny<CancellationToken>()))
            .Returns((OpenIddictApplicationDescriptor descriptor, CancellationToken _) =>
                ValueTask.FromResult(new VirtoOpenIddictEntityFrameworkCoreApplication { ClientId = descriptor.ClientId, ClientSecret = "hashed" }));
    }

    [Fact]
    public void New_GrantsRefreshTokenPermission()
    {
        var controller = new OAuthAppsController(null!);

        var descriptor = controller.New().Value;

        Assert.Contains(
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            descriptor.Permissions);
    }

    [Fact]
    public async Task Save_NewApp_ByNonAdministrator_IsForbidden()
    {
        // Arrange
        // PLATFORM-07: every application gets the client credentials grant, and its tokens act as an administrator
        var controller = CreateController(CreateUser(PlatformPermissions.SecurityOAuthApplicationsCreate, PlatformPermissions.SecurityOAuthApplicationsUpdate));

        // Act
        var result = await controller.SaveAsync(CreateDescriptor("new-app"));

        // Assert
        GetForbiddenErrors(result).Should().ContainSingle().Which.Should().Contain("administrator");
        _manager.Verify(x => x.CreateAsync(It.IsAny<OpenIddictApplicationDescriptor>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Save_ExistingApp_ByNonAdministrator_IsForbidden()
    {
        // Arrange
        // a redirect URI added to the application would let the caller act as the client
        var controller = CreateController(CreateUser(PlatformPermissions.SecurityOAuthApplicationsUpdate));
        var descriptor = CreateDescriptor(StoredClientId);
        descriptor.RedirectUris.Add(new Uri("https://attacker.example.com/callback"));

        // Act
        var result = await controller.SaveAsync(descriptor);

        // Assert
        GetForbiddenErrors(result).Should().ContainSingle();
        _manager.Verify(x => x.UpdateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Save_NewApp_WithoutCreatePermission_IsForbidden()
    {
        // Arrange
        var controller = CreateController(CreateUser(PlatformPermissions.SecurityOAuthApplicationsUpdate));

        // Act
        var result = await controller.SaveAsync(CreateDescriptor("new-app"));

        // Assert
        result.Result.Should().BeOfType<ForbidResult>();
        _manager.Verify(x => x.CreateAsync(It.IsAny<OpenIddictApplicationDescriptor>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Save_NewApp_ByAdministrator_IsCreated()
    {
        // Arrange
        var controller = CreateController(CreateAdministrator());

        // Act
        var result = await controller.SaveAsync(CreateDescriptor("new-app"));

        // Assert
        result.Value.Permissions.Should().Contain(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
        _manager.Verify(x => x.CreateAsync(It.IsAny<OpenIddictApplicationDescriptor>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Save_ExistingApp_ByAdministrator_IsUpdated()
    {
        // Arrange
        var controller = CreateController(CreateAdministrator());

        // Act
        var result = await controller.SaveAsync(CreateDescriptor(StoredClientId));

        // Assert
        result.Value.Should().NotBeNull();
        _manager.Verify(x => x.UpdateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private OAuthAppsController CreateController(ClaimsPrincipal user)
    {
        return new OAuthAppsController(_manager.Object)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } },
        };
    }

    private static ClaimsPrincipal CreateAdministrator()
    {
        return new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.Role, PlatformConstants.Security.SystemRoles.Administrator)], "test"));
    }

    private static ClaimsPrincipal CreateUser(params string[] permissions)
    {
        var claims = permissions.Select(x => new Claim(PlatformConstants.Security.Claims.PermissionClaimType, x));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
    }

    private static OpenIddictApplicationDescriptor CreateDescriptor(string clientId)
    {
        return new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = "secret",
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            DisplayName = "Integration",
        };
    }

    private static IEnumerable<string> GetForbiddenErrors<T>(ActionResult<T> result)
    {
        var forbidden = result.Result.Should().BeOfType<ObjectResult>().Subject;
        forbidden.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        return forbidden.Value.Should().BeOfType<SecurityResult>().Subject.Errors;
    }
}
