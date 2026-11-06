using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model.Security;
using VirtoCommerce.Platform.Web.Security;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Tests.Security;

public class OAuthAppsControllerTests
{
    private const string McpResource = "rsrc:https://platform.example.com/admin/mcp";

    // The applications of the store, by client id: the real OpenIddict manager and EF Core store logic over a dictionary
    private readonly Dictionary<string, VirtoOpenIddictEntityFrameworkCoreApplication> _apps = new(StringComparer.Ordinal);
    private readonly OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication> _manager;

    public OAuthAppsControllerTests()
    {
        var store = new Mock<OpenIddictEntityFrameworkCoreApplicationStore<VirtoOpenIddictEntityFrameworkCoreApplication, VirtoOpenIddictEntityFrameworkCoreAuthorization, VirtoOpenIddictEntityFrameworkCoreToken, string>>(
            new MemoryCache(new MemoryCacheOptions()),
            Mock.Of<IOpenIddictEntityFrameworkCoreContext>(),
            Mock.Of<IOptionsMonitor<OpenIddictEntityFrameworkCoreOptions>>(x => x.CurrentValue == new OpenIddictEntityFrameworkCoreOptions()))
        {
            CallBase = true,
        };

        store
            .Setup(x => x.FindByClientIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((string clientId, CancellationToken _) => ValueTask.FromResult(_apps.GetValueOrDefault(clientId)));
        store
            .Setup(x => x.CreateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .Returns((VirtoOpenIddictEntityFrameworkCoreApplication app, CancellationToken _) =>
            {
                _apps[app.ClientId!] = app;
                return ValueTask.CompletedTask;
            });
        store
            .Setup(x => x.UpdateAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .Returns(() => ValueTask.CompletedTask);

        _manager = new OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>(
            Mock.Of<IOpenIddictApplicationCache<VirtoOpenIddictEntityFrameworkCoreApplication>>(),
            NullLogger<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>.Instance,
            Mock.Of<IOptionsMonitor<OpenIddictCoreOptions>>(x => x.CurrentValue == new OpenIddictCoreOptions { DisableEntityCaching = true }),
            store.Object);
    }

    [Fact]
    public void New_GrantsRefreshTokenPermission()
    {
        var controller = CreateController();

        var descriptor = controller.New().Value;

        Assert.Contains(
            OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
            descriptor.Permissions);
    }

    [Fact]
    public void New_ReturnsConfidentialClientWithExplicitConsentAndPlatformDefaults()
    {
        var descriptor = CreateController().New().Value!;

        descriptor.ClientType.Should().Be(ClientTypes.Confidential);
        descriptor.ApplicationType.Should().Be(ApplicationTypes.Web);
        descriptor.ConsentType.Should().Be(ConsentTypes.Explicit);
        descriptor.ClientSecret.Should().NotBeNullOrEmpty();
        descriptor.Permissions.Should().BeEquivalentTo(
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.GrantTypes.ClientCredentials,
            Permissions.GrantTypes.RefreshToken,
            Permissions.Scopes.Email,
            Permissions.Scopes.Profile,
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.EndSession,
            Permissions.Endpoints.Token,
            Permissions.ResponseTypes.Code);
    }

    [Fact]
    public async Task Save_PublicAgentClient_StoresAndReturnsEveryField()
    {
        var controller = CreateController();
        var descriptor = CreateAgentClient();
        descriptor.PostLogoutRedirectUris.Add(new Uri("https://agent.example.com/signed-out"));

        var saved = GetValue(await controller.SaveAsync(descriptor));
        var loaded = GetValue(await controller.GetAsync("agent"));

        foreach (var actual in new[] { saved, loaded })
        {
            actual.ClientId.Should().Be("agent");
            actual.DisplayName.Should().Be("AI agent");
            actual.ClientType.Should().Be(ClientTypes.Public);
            actual.ApplicationType.Should().Be(ApplicationTypes.Native);
            actual.ConsentType.Should().Be(ConsentTypes.Explicit);
            actual.ClientSecret.Should().BeEmpty();
            actual.Requirements.Should().BeEquivalentTo(Requirements.Features.ProofKeyForCodeExchange);
            actual.RedirectUris.Should().BeEquivalentTo([new Uri("http://localhost/callback")]);
            actual.PostLogoutRedirectUris.Should().BeEquivalentTo([new Uri("https://agent.example.com/signed-out")]);
            actual.Permissions.Should().BeEquivalentTo(
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.Scopes.Profile,
                McpResource,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.EndSession,
                Permissions.Endpoints.Token,
                Permissions.ResponseTypes.Code);
        }
    }

    [Fact]
    public async Task Save_UncheckedDefaults_AreNotAdded()
    {
        var controller = CreateController();
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "integration",
            ClientSecret = "secret",
            ClientType = ClientTypes.Confidential,
            Permissions = { Permissions.GrantTypes.ClientCredentials },
        };

        var saved = GetValue(await controller.SaveAsync(descriptor));

        saved.Permissions.Should().BeEquivalentTo(Permissions.GrantTypes.ClientCredentials, Permissions.Endpoints.Token);
    }

    [Fact]
    public async Task Save_WithoutGrantTypes_GetsPlatformDefaults()
    {
        // the request the MCP README used to document: the resource only
        var controller = CreateController();
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "claude-code",
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Explicit,
            RedirectUris = { new Uri("http://localhost:65111/callback") },
            Requirements = { Requirements.Features.ProofKeyForCodeExchange },
            Permissions = { McpResource },
        };

        var saved = GetValue(await controller.SaveAsync(descriptor));

        saved.Permissions.Should().BeEquivalentTo(
            McpResource,
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.GrantTypes.RefreshToken,
            Permissions.Scopes.Email,
            Permissions.Scopes.Profile,
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.EndSession,
            Permissions.Endpoints.Token,
            Permissions.ResponseTypes.Code);
    }

    [Fact]
    public async Task Save_UncheckedAuthorizationCode_RemovesItsEndpoints()
    {
        var controller = CreateController();
        await controller.SaveAsync(CreateAgentClient());

        var descriptor = GetValue(await controller.GetAsync("agent"));
        descriptor.Permissions.Remove(Permissions.GrantTypes.AuthorizationCode);
        var saved = GetValue(await controller.SaveAsync(descriptor));

        saved.Permissions.Should().BeEquivalentTo(Permissions.GrantTypes.RefreshToken, Permissions.Scopes.Profile, McpResource, Permissions.Endpoints.Token);
    }

    [Fact]
    public async Task Save_ExistingClient_KeepsSecretAndEveryPermission()
    {
        var controller = CreateController();
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "storefront",
            ClientSecret = "s3cret",
            ClientType = ClientTypes.Confidential,
            Permissions =
            {
                Permissions.GrantTypes.Password,
                Permissions.GrantTypes.ClientCredentials,
                Permissions.Endpoints.Introspection,
                Permissions.Prefixes.Scope + "custom",
                McpResource,
            },
        };
        var created = GetValue(await controller.SaveAsync(descriptor));

        // the back office sends the client back with the empty secret the search returns
        var loaded = GetValue(await controller.GetAsync("storefront"));
        loaded.DisplayName = "Storefront";
        var saved = GetValue(await controller.SaveAsync(loaded));

        created.ClientSecret.Should().BeEmpty();
        saved.ClientSecret.Should().BeEmpty();
        saved.DisplayName.Should().Be("Storefront");
        saved.Permissions.Should().BeEquivalentTo(created.Permissions);
        saved.Permissions.Should().Contain([Permissions.GrantTypes.Password, Permissions.Endpoints.Introspection, Permissions.Prefixes.Scope + "custom", McpResource]);
        (await _manager.ValidateClientSecretAsync(_apps["storefront"], "s3cret", TestContext.Current.CancellationToken)).Should().BeTrue();
    }

    [Fact]
    public async Task Save_PublicClientWithSecret_ReturnsBadRequest()
    {
        var descriptor = CreateAgentClient();
        descriptor.ClientSecret = "secret";

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain("public client can't have a client secret");
        _apps.Should().BeEmpty();
    }

    [Fact]
    public async Task Save_ConfidentialClientWithoutSecret_ReturnsBadRequest()
    {
        var result = await CreateController().SaveAsync(new OpenIddictApplicationDescriptor { ClientId = "app", ClientType = ClientTypes.Confidential });

        GetErrors(result).Should().NotBeEmpty();
        _apps.Should().BeEmpty();
    }

    [Fact]
    public async Task Save_TypesInAnyCase_AreStoredInLowerCase()
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = "integration",
            ClientSecret = "secret",
            ClientType = "Confidential",
            ApplicationType = " Native ",
            ConsentType = "Explicit",
            Permissions = { Permissions.GrantTypes.ClientCredentials },
        };

        var saved = GetValue(await CreateController().SaveAsync(descriptor));

        (saved.ClientType, saved.ApplicationType, saved.ConsentType).Should().Be((ClientTypes.Confidential, ApplicationTypes.Native, ConsentTypes.Explicit));
    }

    [Theory]
    [InlineData(null, "secret", ClientTypes.Confidential)]
    [InlineData(null, null, ClientTypes.Public)]
    [InlineData("", null, ClientTypes.Public)]
    public async Task Save_NewClientWithoutClientType_GetsItFromTheSecret(string clientType, string secret, string expected)
    {
        var descriptor = CreateAgentClient();
        descriptor.ClientType = clientType;
        descriptor.ClientSecret = secret;

        var saved = GetValue(await CreateController().SaveAsync(descriptor));

        saved.ClientType.Should().Be(expected);
    }

    [Fact]
    public async Task Save_ExistingClientWithoutClientType_KeepsTheStoredOne()
    {
        var controller = CreateController();
        await controller.SaveAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "integration",
            ClientSecret = "secret",
            ClientType = ClientTypes.Confidential,
            Permissions = { Permissions.GrantTypes.ClientCredentials },
        });

        var saved = GetValue(await controller.SaveAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = "integration",
            DisplayName = "Integration",
            Permissions = { Permissions.GrantTypes.ClientCredentials },
        }));

        saved.ClientType.Should().Be(ClientTypes.Confidential);
        saved.DisplayName.Should().Be("Integration");
    }

    [Fact]
    public async Task Save_ChangedClientType_ReturnsBadRequest()
    {
        var controller = CreateController();
        await controller.SaveAsync(CreateAgentClient());

        var descriptor = GetValue(await controller.GetAsync("agent"));
        descriptor.ClientType = ClientTypes.Confidential;
        var result = await controller.SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain("can't be changed");
    }

    [Theory]
    [InlineData("callback")]
    [InlineData("https://agent.example.com/callback#fragment")]
    [InlineData("http://agent.example.com/callback")]
    public async Task Save_InvalidRedirectUri_ReturnsBadRequest(string uri)
    {
        var descriptor = CreateAgentClient();
        descriptor.RedirectUris.Add(new Uri(uri, UriKind.RelativeOrAbsolute));

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain(uri);
    }

    [Fact]
    public async Task Save_HttpRedirectUriTheClientAlreadyHas_IsKept()
    {
        // a client registered before the HTTPS rule
        var controller = CreateController();
        var legacy = CreateAgentClient();
        legacy.RedirectUris.Add(new Uri("http://intranet.example.com/callback"));
        await _manager.CreateAsync(legacy, TestContext.Current.CancellationToken);

        var descriptor = GetValue(await controller.GetAsync("agent"));
        descriptor.DisplayName = "Renamed";
        var saved = GetValue(await controller.SaveAsync(descriptor));

        saved.RedirectUris.Should().Contain(new Uri("http://intranet.example.com/callback"));
    }

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("http://agent.example.com/signed-out")]
    public async Task Save_InvalidPostLogoutRedirectUri_ReturnsBadRequest(string uri)
    {
        var descriptor = CreateAgentClient();
        descriptor.PostLogoutRedirectUris.Add(new Uri(uri));

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain(uri);
    }

    [Fact]
    public async Task Save_RedirectUriOnlyOpenIddictRefuses_ReturnsBadRequest()
    {
        // the iss parameter is reserved (RFC 9207): OpenIddict refuses it when the application is created
        var descriptor = CreateAgentClient();
        descriptor.RedirectUris.Add(new Uri("https://agent.example.com/callback?iss=https://evil.example.com"));

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().NotBeEmpty();
        _apps.Should().BeEmpty();
    }

    [Fact]
    public async Task Save_InvalidApplicationType_ReturnsBadRequest()
    {
        var descriptor = CreateAgentClient();
        descriptor.ApplicationType = "desktop";

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain("application type");
    }

    [Fact]
    public async Task Save_InvalidConsentType_ReturnsBadRequest()
    {
        var descriptor = CreateAgentClient();
        descriptor.ConsentType = "sometimes";

        var result = await CreateController().SaveAsync(descriptor);

        GetErrors(result).Should().ContainSingle().Which.Should().Contain("consent type");
    }

    [Fact]
    public async Task Get_UnknownClient_ReturnsNotFound()
    {
        var result = await CreateController().GetAsync("unknown");

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetResources_MergesModuleResourcesWithRegisteredOnes()
    {
        var provider = new Mock<IOAuthResourceProvider>();
        provider
            .Setup(x => x.GetResourcesAsync(It.Is<OAuthResourceContext>(c => c.HttpContext != null), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<OAuthResource>
            {
                new() { Uri = "https://platform.example.com/admin/mcp", Title = "Back office", Kind = "mcp" },
                new() { Uri = "https://platform.example.com/ucp/mcp", Title = "Shopping", Kind = "mcp" },
                new() { Uri = "" },
            });
        var controller = CreateController(["https://platform.example.com/admin/mcp", "https://api.example.com"], provider.Object);

        var resources = GetValue(await controller.GetResourcesAsync(TestContext.Current.CancellationToken));

        resources.Select(x => (x.Uri, x.Title, x.IsRegistered)).Should().Equal(
            ("https://platform.example.com/admin/mcp", "Back office", true),
            ("https://platform.example.com/ucp/mcp", "Shopping", false),
            ("https://api.example.com", null, true));
    }

    [Fact]
    public async Task GetResources_ProviderThatFailsOrAnswersNull_IsSkipped()
    {
        var failing = new Mock<IOAuthResourceProvider>();
        failing
            .Setup(x => x.GetResourcesAsync(It.IsAny<OAuthResourceContext>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("The module is not configured."));
        var empty = new Mock<IOAuthResourceProvider>();
        empty
            .Setup(x => x.GetResourcesAsync(It.IsAny<OAuthResourceContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IList<OAuthResource>)null);
        var working = new Mock<IOAuthResourceProvider>();
        working
            .Setup(x => x.GetResourcesAsync(It.IsAny<OAuthResourceContext>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new OAuthResource { Uri = "https://platform.example.com/admin/mcp", Title = "Back office" }]);
        var controller = CreateController(["https://api.example.com"], failing.Object, empty.Object, working.Object);

        var resources = GetValue(await controller.GetResourcesAsync(TestContext.Current.CancellationToken));

        resources.Select(x => x.Uri).Should().Equal("https://platform.example.com/admin/mcp", "https://api.example.com");
    }

    [Theory]
    [InlineData(Permissions.GrantTypes.Implicit, new[] { Permissions.Endpoints.Authorization, Permissions.Endpoints.EndSession })]
    [InlineData(Permissions.Prefixes.GrantType + "impersonate", new[] { Permissions.Endpoints.Token })]
    [InlineData(Permissions.GrantTypes.Password, new[] { Permissions.Endpoints.Token })]
    public void Normalize_DerivesTheEndpointsOfTheGrantType(string grantType, string[] endpoints)
    {
        var descriptor = new OpenIddictApplicationDescriptor { ClientType = ClientTypes.Public, Permissions = { grantType } };

        new OAuthAppRules().Normalize(descriptor, stored: null);

        descriptor.Permissions.Should().BeEquivalentTo([grantType, .. endpoints]);
    }

    [Fact]
    public void New_UsesTheDefaultsOfDerivedRules()
    {
        var controller = new OAuthAppsController(_manager, [], new RolesScopeRules(), Options.Create(new AuthorizationOptions()), NullLogger<OAuthAppsController>.Instance);

        var descriptor = controller.New().Value!;

        descriptor.Permissions.Should().Contain(Permissions.Scopes.Roles).And.NotContain(Permissions.Scopes.Email);
    }

    [Fact]
    public void AddSecurityServices_KeepsRulesRegisteredBefore()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IOAuthAppRules, RolesScopeRules>();

        services.AddSecurityServices();

        services.Where(x => x.ServiceType == typeof(IOAuthAppRules)).Should().ContainSingle()
            .Which.ImplementationType.Should().Be<RolesScopeRules>();
    }

    [Fact]
    public void AddSecurityServices_RegistersTheRules()
    {
        var services = new ServiceCollection();

        services.AddSecurityServices();

        services.Where(x => x.ServiceType == typeof(IOAuthAppRules)).Should().ContainSingle()
            .Which.ImplementationType.Should().Be<OAuthAppRules>();
    }

    [Theory]
    [InlineData("https://claude.ai/api/mcp/auth_callback", true)]
    [InlineData("http://localhost:65111/callback", true)]
    [InlineData("http://127.0.0.1:33418", true)]
    [InlineData("http://[::1]/callback", true)]
    [InlineData("cursor://anysphere.cursor-retrieval/oauth/callback", true)]
    [InlineData("http://agent.example.com/callback", false)]
    [InlineData("/callback", false)]
    [InlineData("javascript:alert(1)", false)]
    [InlineData("data:text/html,x", false)]
    public void GetRedirectUriError_AllowsHttpOnlyForLoopback(string uri, bool isValid)
    {
        (new OAuthAppRules().GetRedirectUriError(uri) == null).Should().Be(isValid);
    }

    private OAuthAppsController CreateController(string[] registeredResources = null, params IOAuthResourceProvider[] providers)
    {
        var options = Options.Create(new AuthorizationOptions { Resources = registeredResources ?? [] });

        return new OAuthAppsController(_manager, providers, new OAuthAppRules(), options, NullLogger<OAuthAppsController>.Instance)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() },
        };
    }

    private static OpenIddictApplicationDescriptor CreateAgentClient()
    {
        return new OpenIddictApplicationDescriptor
        {
            ClientId = "agent",
            DisplayName = "AI agent",
            ClientType = ClientTypes.Public,
            ApplicationType = ApplicationTypes.Native,
            ConsentType = ConsentTypes.Explicit,
            RedirectUris = { new Uri("http://localhost/callback") },
            Requirements = { Requirements.Features.ProofKeyForCodeExchange },
            Permissions =
            {
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.Scopes.Profile,
                McpResource,
            },
        };
    }

    private static T GetValue<T>(ActionResult<T> result)
    {
        return result.Value ?? (T)result.Result.Should().BeOfType<OkObjectResult>().Subject.Value!;
    }

    private static IEnumerable<string> GetErrors<T>(ActionResult<T> result)
    {
        return result.Result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().BeOfType<SecurityResult>().Subject.Errors;
    }

    private sealed class RolesScopeRules : OAuthAppRules
    {
        protected override IEnumerable<string> GetDefaultScopes()
        {
            return [Permissions.Scopes.Profile, Permissions.Scopes.Roles];
        }
    }
}
