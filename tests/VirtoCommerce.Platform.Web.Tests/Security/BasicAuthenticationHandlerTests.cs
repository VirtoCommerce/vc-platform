using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class BasicAuthenticationHandlerTests
{
    private readonly ApplicationUser _user = new() { Id = "user-1", UserName = "admin" };
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;

    public BasicAuthenticationHandlerTests()
    {
        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        _userManager.Setup(x => x.FindByNameAsync("admin")).ReturnsAsync(_user);
        _userManager.Setup(x => x.CheckPasswordAsync(_user, "secret")).ReturnsAsync(true);
        _signInManager.Setup(x => x.CanSignInAsync(_user)).ReturnsAsync(true);
        _signInManager.Setup(x => x.CreateUserPrincipalAsync(_user)).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity("test")));
    }

    [Fact]
    public async Task AuthenticateAsync_TwoFactorNotRequired_Succeeds()
    {
        _signInManager.Setup(x => x.IsTwoFactorEnabledAsync(_user)).ReturnsAsync(false);

        var result = await AuthenticateAsync("admin", "secret");

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task AuthenticateAsync_TwoFactorRequired_Fails()
    {
        _signInManager.Setup(x => x.IsTwoFactorEnabledAsync(_user)).ReturnsAsync(true);

        var result = await AuthenticateAsync("admin", "secret");

        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Contain("Two-factor authentication is required");
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_DoesNotRevealTwoFactor()
    {
        _signInManager.Setup(x => x.IsTwoFactorEnabledAsync(_user)).ReturnsAsync(true);

        var result = await AuthenticateAsync("admin", "wrong");

        result.Failure.Message.Should().Be("Invalid user name or password.");
        _signInManager.Verify(x => x.IsTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    private async Task<AuthenticateResult> AuthenticateAsync(string userName, string password)
    {
        var options = new Mock<IOptionsMonitor<BasicAuthenticationOptions>>();
        options.Setup(x => x.Get(It.IsAny<string>())).Returns(new BasicAuthenticationOptions());

        var handler = new BasicAuthenticationHandler(options.Object, NullLoggerFactory.Instance, UrlEncoder.Default, _userManager.Object, _signInManager.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

        await handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationOptions.DefaultScheme, null, typeof(BasicAuthenticationHandler)), httpContext);

        return await handler.AuthenticateAsync();
    }
}
