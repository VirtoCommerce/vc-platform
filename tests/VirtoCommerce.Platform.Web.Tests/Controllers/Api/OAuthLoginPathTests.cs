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

/// <summary>
/// One Platform serves several front-ends, and the login path is redirected to relatively, so the
/// path in use depends on the host the authorization request arrives on.
/// </summary>
public class OAuthLoginPathTests : IDisposable
{
    private const string _returnUrl = "/connect/authorize?client_id=claude&state=opaque";
    private readonly ApplicationUser _user = new() { Id = "buyer" };
    private readonly ClaimsPrincipal _principal = new(new ClaimsIdentity("Bearer"));
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<IAuthenticationService> _authentication = new();
    private readonly ServiceProvider _services;
    private readonly DefaultHttpContext _httpContext;
    private readonly AuthorizationController _controller;
    private readonly AuthorizationOptions _options = new()
    {
        OAuthLoginPath = "/oauth/authorize",
        BuiltInLoginHosts = ["platform.example"],
    };

    public OAuthLoginPathTests()
    {
        _userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
        _userManager.Setup(x => x.GetUserAsync(_principal)).ReturnsAsync(_user);
        _signInManager.Setup(x => x.CanSignInAsync(_user)).ReturnsAsync(true);
        _principal.SetAudiences("resource_server");
        _principal.SetExpirationDate(DateTimeOffset.UtcNow.AddMinutes(30));
        _authentication.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme))
            .ReturnsAsync(AuthenticateResult.Success(new AuthenticationTicket(_principal, "Bearer")));
        _services = new ServiceCollection().AddSingleton(_authentication.Object).BuildServiceProvider();
        _httpContext = new DefaultHttpContext { RequestServices = _services };
        _httpContext.Request.Scheme = "https";
        _controller = new AuthorizationController(null, Options.Create(new IdentityOptions()), _signInManager.Object,
            Options.Create(new PasswordLoginOptions()), null, [], [], [], null, null, null, null, null, Options.Create(_options))
        {
            ControllerContext = new ControllerContext { HttpContext = _httpContext },
            Url = new UrlHelper(new ActionContext(_httpContext, new RouteData(), new ActionDescriptor())),
        };
    }

    [Fact]
    public async Task CreateSession_HostWithItsOwnFrontEnd_EstablishesTheSession()
    {
        UseHost("shop.example");

        Assert.IsType<NoContentResult>(await _controller.CreateSession(_returnUrl));
    }

    [Fact]
    public async Task CreateSession_HostServedByThePlatformItself_IsNotAvailable()
    {
        UseHost("platform.example");

        // The built-in login page signs the user in on its own, so there is no session to hand over
        // and nothing for a front-end to call here.
        Assert.IsType<NotFoundResult>(await _controller.CreateSession(_returnUrl));
    }

    [Fact]
    public async Task CreateSession_HostOverrideIsCaseInsensitive()
    {
        UseHost("Platform.Example");

        Assert.IsType<NotFoundResult>(await _controller.CreateSession(_returnUrl));
    }

    private void UseHost(string host)
    {
        _httpContext.Request.Host = new HostString(host);
        _httpContext.Request.Headers.Origin = $"https://{host}";
    }

    public void Dispose()
    {
        _services.Dispose();
        GC.SuppressFinalize(this);
    }
}
