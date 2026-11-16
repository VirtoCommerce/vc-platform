using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Model;
using Xunit;
using static OpenIddict.Abstractions.OpenIddictConstants;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace VirtoCommerce.Platform.Tests.Security.ClientRegistration;

public class AuthorizationControllerSelfRegisteredClientTests
{
    private const string ClientId = "https://claude.ai/oauth/claude-code-client-metadata";
    private const string VendorResource = "https://platform.example.com/vendor/mcp";

    private readonly SelfRegisteredClientTestContext _context = new();
    private readonly ApplicationUser _user = new() { Id = "user-1", UserName = "marketer" };
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<IOpenIddictAuthorizationManager> _authorizationManager = new();
    private readonly List<(object Authorization, string ApplicationId, string[] Resources)> _authorizations = [];

    public AuthorizationControllerSelfRegisteredClientTests()
    {
        _context.SetDocument(ClientId, "Claude Code", "http://localhost/callback");
        _context.Resources.Add(new() { Uri = VendorResource, Kind = "mcp", AllowSelfRegisteredClients = true });

        _userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_user);
        _userManager.Setup(x => x.GetUserIdAsync(_user)).ReturnsAsync(_user.Id);

        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
        _signInManager
            .Setup(x => x.CreateUserPrincipalAsync(_user))
            .ReturnsAsync(() => new ClaimsPrincipal(new ClaimsIdentity([new Claim(Claims.Subject, _user.Id)], "test")));

        _authorizationManager
            .Setup(x => x.FindAsync(It.IsAny<string>(), It.IsAny<string>(), Statuses.Valid, AuthorizationTypes.Permanent,
                It.IsAny<ImmutableArray<string>>(), It.IsAny<CancellationToken>()))
            .Returns((string _, string client, string _, string _, ImmutableArray<string> _, CancellationToken _) =>
                _authorizations.Where(x => x.ApplicationId == client).Select(x => x.Authorization).ToAsyncEnumerable());
        _authorizationManager
            .Setup(x => x.GetPropertiesAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((object authorization, CancellationToken _) =>
            {
                var resources = _authorizations.First(x => x.Authorization == authorization).Resources;
                return resources == null
                    ? ImmutableDictionary<string, JsonElement>.Empty
                    : ImmutableDictionary<string, JsonElement>.Empty.Add(SelfRegisteredClientConstants.ResourcesProperty, JsonSerializer.SerializeToElement(resources));
            });
        _authorizationManager
            .Setup(x => x.CreateAsync(It.IsAny<OpenIddictAuthorizationDescriptor>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OpenIddictAuthorizationDescriptor descriptor, CancellationToken _) =>
            {
                var authorization = new object();
                var resources = descriptor.Properties[SelfRegisteredClientConstants.ResourcesProperty].EnumerateArray().Select(x => x.GetString()).ToArray();
                _authorizations.Add((authorization, descriptor.ApplicationId, resources));
                return authorization;
            });
        _authorizationManager.Setup(x => x.GetIdAsync(It.IsAny<object>(), It.IsAny<CancellationToken>())).ReturnsAsync("auth");
    }

    [Fact]
    public async Task Authorize_FirstTime_ShowsTheConsentPageAndStoresNothing()
    {
        // Arrange
        var controller = CreateController(SelfRegisteredClientTestContext.McpResource);

        // Act
        var result = await controller.Authorize();

        // Assert
        var model =result.Should().BeOfType<ViewResult>().Subject.Model.Should().BeOfType<AuthorizeViewModel>().Subject;
        model.ApplicationName.Should().Be("Claude Code");
        model.ClientHost.Should().Be("claude.ai");
        model.RegistrationType.Should().Be(SelfRegisteredClientConstants.ClientIdMetadataDocument);
        model.Resources.Should().Equal(SelfRegisteredClientTestContext.McpResource);
        model.WarnAboutLocalApp.Should().BeTrue("the code goes to an app on this computer, which any app could claim to be");
        _context.Stored.Should().BeEmpty();
        _authorizations.Should().BeEmpty();
    }

    [Fact]
    public async Task Authorize_HostWithLookalikeLetters_ShowsItsPunycode()
    {
        // Arrange
        const string lookalike = "https://\u0441laude.ai/oauth/client";
        _context.SetDocument(lookalike, "Claude Code", "http://localhost/callback");
        var controller = CreateController(SelfRegisteredClientTestContext.McpResource, lookalike);

        // Act
        var result = await controller.Authorize();

        // Assert
        var model = result.Should().BeOfType<ViewResult>().Subject.Model.Should().BeOfType<AuthorizeViewModel>().Subject;
        model.ClientHost.Should().Be("xn--laude-0ye.ai");
    }

    [Fact]
    public async Task Accept_FirstTime_StoresTheClientAndTheConsentToTheResource()
    {
        // Arrange
        var controller = CreateController(SelfRegisteredClientTestContext.McpResource);

        // Act
        var result = await controller.Accept();

        // Assert
        var signIn = result.Should().BeOfType<SignInResult>().Subject;
        signIn.Principal.GetResources().Should().Equal(SelfRegisteredClientTestContext.McpResource);
        _context.Stored.Should().ContainKey(ClientId);
        _authorizations.Should().ContainSingle().Which.Resources.Should().Equal(SelfRegisteredClientTestContext.McpResource);
        _authorizations[0].ApplicationId.Should().Be(_context.Stored[ClientId].Id);
    }

    [Fact]
    public async Task Authorize_ConsentedResource_SignsInWithoutAsking()
    {
        // Arrange
        await CreateController(SelfRegisteredClientTestContext.McpResource).Accept();
        var controller = CreateController(SelfRegisteredClientTestContext.McpResource);

        // Act
        var result = await controller.Authorize();

        // Assert
        result.Should().BeOfType<SignInResult>();
        _authorizations.Should().ContainSingle();
    }

    [Fact]
    public async Task Authorize_AnotherResource_AsksAgain()
    {
        // Arrange
        await CreateController(SelfRegisteredClientTestContext.McpResource).Accept();
        var controller = CreateController(VendorResource);

        // Act
        var result = await controller.Authorize();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Authorize_ClientOfAnAdministrator_AuthorizationWithoutResourcesStillCounts()
    {
        // Arrange
        var application = new VirtoOpenIddictEntityFrameworkCoreApplication
        {
            ClientId = "agent",
            ConsentType = ConsentTypes.Explicit,
        };
        _context.Stored["agent"] = application;
        _authorizations.Add((new object(), application.Id, null));
        var controller = CreateController(SelfRegisteredClientTestContext.McpResource, "agent");

        // Act
        var result = await controller.Authorize();

        // Assert
        result.Should().BeOfType<SignInResult>();
    }

    private AuthorizationController CreateController(string resource, string clientId = ClientId)
    {
        var request = new OpenIddictRequest
        {
            ClientId = clientId,
            ResponseType = ResponseTypes.Code,
            RedirectUri = "http://localhost:51234/callback",
            Scope = "openid profile offline_access",
            Resources = [resource],
            CodeChallengeMethod = CodeChallengeMethods.Sha256,
        };

        var authentication = new Mock<IAuthenticationService>();
        authentication
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), IdentityConstants.ApplicationScheme))
            .ReturnsAsync(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity("cookie")), IdentityConstants.ApplicationScheme)));

        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton(authentication.Object)
                .AddSingleton(Mock.Of<ITempDataDictionaryFactory>())
                .BuildServiceProvider(),
        };
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("platform.example.com");
        httpContext.Request.Path = "/connect/authorize";
        httpContext.Features.Set(new OpenIddictServerAspNetCoreFeature { Transaction = new OpenIddictServerTransaction { Request = request } });

        return new AuthorizationController(_context.CreateManager(), Options.Create(new IdentityOptions()), _signInManager.Object,
            Options.Create(new PasswordLoginOptions()), null, [], [], [], [], null, null, null, null, _authorizationManager.Object,
            Mock.Of<IOpenIddictScopeManager>(), new OAuthLoginPathProvider(Options.Create(new AuthorizationOptions())))
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext },
        };
    }
}
