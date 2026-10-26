using System;
using System.Linq;
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
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class BasicAuthenticationHandlerTests
{
    private const string Password = "secret";

    private readonly ApplicationUser _admin = new() { Id = "user-1", UserName = "admin" };
    private readonly ApplicationUser _twoFactorAdmin = new() { Id = "user-2", UserName = "mfa-admin", TwoFactorEnabled = true };
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<ITwoFactorSignInService> _twoFactorSignInService = new();

    public BasicAuthenticationHandlerTests()
    {
        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);

        // Answers come from the test users: their name, the password and their own two-factor flag.
        _userManager
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string userName) => new[] { _admin, _twoFactorAdmin }.FirstOrDefault(x => x.UserName == userName));
        _userManager
            .Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser _, string password) => password == Password);
        _signInManager
            .Setup(x => x.CanSignInAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(true);
        _signInManager
            .Setup(x => x.CreateUserPrincipalAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity("test")));
        _twoFactorSignInService
            .Setup(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser user) => user.TwoFactorEnabled);
    }

    [Fact]
    public async Task AuthenticateAsync_TwoFactorNotRequired_Succeeds()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task AuthenticateAsync_TwoFactorRequired_Fails()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_twoFactorAdmin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Contain("Two-factor authentication is required");
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_DoesNotRevealTwoFactor()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_twoFactorAdmin.UserName, "wrong");

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Failure.Message.Should().Be("Invalid user name or password.");
        _twoFactorSignInService.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    private async Task<BasicAuthenticationHandler> CreateHandlerAsync(string userName, string password)
    {
        var options = new Mock<IOptionsMonitor<BasicAuthenticationOptions>>();
        options
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(new BasicAuthenticationOptions());

        var handler = new BasicAuthenticationHandler(options.Object, NullLoggerFactory.Instance, UrlEncoder.Default, _userManager.Object, _signInManager.Object, _twoFactorSignInService.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

        await handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationOptions.DefaultScheme, null, typeof(BasicAuthenticationHandler)), httpContext);

        return handler;
    }
}
