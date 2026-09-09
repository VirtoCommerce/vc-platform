using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Controllers.Api;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api;

public class OAuthSessionTests : IDisposable
{
    private const string _returnUrl = "/connect/authorize?client_id=claude&state=opaque";
    private readonly ApplicationUser _user = new() { Id = "buyer" };
    private readonly ClaimsPrincipal _principal = new(new ClaimsIdentity("Bearer"));
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<IAuthenticationService> _authentication = new();
    private readonly ServiceProvider _services;
    private readonly AuthorizationController _controller;
    private readonly AuthorizationOptions _options = new() { OAuthLoginPath = "/oauth/authorize" };
    private AuthenticationProperties _cookieProperties;

    public OAuthSessionTests()
    {
        _userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
        _userManager.Setup(x => x.GetUserAsync(_principal)).ReturnsAsync(_user);
        _signInManager.Setup(x => x.CanSignInAsync(_user)).ReturnsAsync(true);
        _signInManager.Setup(x => x.SignInAsync(_user, It.IsAny<AuthenticationProperties>(), null))
            .Callback<ApplicationUser, AuthenticationProperties, string>((_, properties, _) => _cookieProperties = properties)
            .Returns(Task.CompletedTask);
        _principal.SetAudiences("resource_server");
        _principal.SetExpirationDate(DateTimeOffset.UtcNow.AddMinutes(30));
        _authentication.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme))
            .ReturnsAsync(AuthenticateResult.Success(new AuthenticationTicket(_principal, "Bearer")));
        _services = new ServiceCollection().AddSingleton(_authentication.Object).BuildServiceProvider();
        var httpContext = new DefaultHttpContext { RequestServices = _services };
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("shop.example");
        httpContext.Request.Headers.Origin = "https://shop.example";
        _controller = new AuthorizationController(null, Options.Create(new IdentityOptions()), _signInManager.Object,
            Options.Create(new PasswordLoginOptions()), null, [], [], [], null, null, null, null, null, Options.Create(_options))
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext },
            Url = new UrlHelper(new ActionContext(httpContext, new RouteData(), new ActionDescriptor())),
        };
    }

    [Fact]
    public async Task CreateSession_FirstPartyBuyer_BindsShortSessionToAuthorizationRequest()
    {
        var tokenExpiry = DateTimeOffset.UtcNow.AddMinutes(1);
        _principal.SetExpirationDate(tokenExpiry);

        Assert.IsType<NoContentResult>(await _controller.CreateSession(_returnUrl));
        Assert.Equal(_returnUrl, _cookieProperties.RedirectUri);
        Assert.False(_cookieProperties.IsPersistent);
        Assert.False(_cookieProperties.AllowRefresh);
        Assert.True(_cookieProperties.ExpiresUtc <= tokenExpiry);
    }

    [Theory]
    [InlineData("https://other.example/connect/authorize?state=x")]
    [InlineData("//other.example/connect/authorize?state=x")]
    [InlineData("/api/platform/security/users?state=x")]
    [InlineData("/connect/authorize?state=x#fragment")]
    [InlineData(null)]
    public async Task CreateSession_UnrelatedReturnUrl_Rejects(string returnUrl)
    {
        Assert.IsType<BadRequestResult>(await _controller.CreateSession(returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Fact]
    public async Task CreateSession_CrossOriginRequest_Rejects()
    {
        _controller.Request.Headers.Origin = "https://other.example";
        Assert.IsType<BadRequestResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateSession_ThirdPartyToken_Rejects(bool hasClient)
    {
        if (hasClient)
        {
            _principal.SetClaim(OpenIddictConstants.Claims.ClientId, "third-party");
        }
        else
        {
            _principal.SetAudiences("https://shop.example/ucp/mcp");
        }

        Assert.IsType<UnauthorizedResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Fact]
    public async Task CreateSession_LockedBuyer_Rejects()
    {
        _userManager.Setup(x => x.IsLockedOutAsync(_user)).ReturnsAsync(true);
        Assert.IsType<UnauthorizedResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Fact]
    public async Task CreateSession_DisabledByDefault_DoesNotAuthenticate()
    {
        _options.OAuthLoginPath = null;
        Assert.IsType<NotFoundResult>(await _controller.CreateSession(_returnUrl));
        _authentication.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RevokeCurrentUserToken_ClearsConsentSessionOnlyWhenEnabled(bool enabled)
    {
        _options.OAuthLoginPath = enabled ? "/oauth/authorize" : null;

        Assert.IsType<OkResult>(await _controller.RevokeCurrentUserToken());

        _authentication.Verify(x => x.SignOutAsync(It.IsAny<HttpContext>(), IdentityConstants.ApplicationScheme,
            It.IsAny<AuthenticationProperties>()), enabled ? Times.Once() : Times.Never());
    }

    [Fact]
    public async Task CreateSession_ExpiredPassword_Rejects()
    {
        _user.PasswordExpired = true;
        Assert.IsType<UnauthorizedResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Fact]
    public async Task CreateSession_SignInNotAllowed_Rejects()
    {
        _signInManager.Setup(x => x.CanSignInAsync(_user)).ReturnsAsync(false);
        Assert.IsType<UnauthorizedResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    [Fact]
    public async Task CreateSession_ExpiredTokenWithinClockSkew_Rejects()
    {
        _principal.SetExpirationDate(DateTimeOffset.UtcNow.AddSeconds(-1));
        Assert.IsType<UnauthorizedResult>(await _controller.CreateSession(_returnUrl));
        Assert.Null(_cookieProperties);
    }

    public void Dispose()
    {
        _services.Dispose();
        _userManager.Object.Dispose();
        GC.SuppressFinalize(this);
    }
}
