using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class VirtoSignInManagerTests
{
    private const string Password = "secret";

    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly Mock<ITwoFactorSignInService> _twoFactorSignInService = new();
    private readonly Mock<IAuthenticationService> _authenticationService = new();
    private readonly DefaultHttpContext _httpContext = new();

    public VirtoSignInManagerTests()
    {
        // Answers come from the user: the password, and their own two-factor flag for the two-factor sign-in service.
        _userManager
            .SetupGet(x => x.SupportsUserLockout)
            .Returns(true);
        _userManager
            .Setup(x => x.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(false);
        _userManager
            .Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser _, string password) => password == Password);
        _userManager
            .Setup(x => x.GetUserIdAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser user) => user.Id);
        _twoFactorSignInService
            .Setup(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser user) => user.TwoFactorEnabled);
        _authenticationService
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.NoResult());
        _httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(_authenticationService.Object)
            .BuildServiceProvider();
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IsTwoFactorEnabledAsync_ReturnsWhatTheTwoFactorSignInServiceDecides(bool twoFactorRequired)
    {
        // Arrange
        var user = new ApplicationUser { Id = "user-1", UserName = "admin", TwoFactorEnabled = twoFactorRequired };
        var signInManager = CreateSignInManager();

        // Act
        var isEnabled = await signInManager.IsTwoFactorEnabledAsync(user);

        // Assert
        isEnabled.Should().Be(twoFactorRequired);
        _twoFactorSignInService.Verify(x => x.IsTwoFactorRequiredAsync(user), Times.Once);
    }

    [Fact]
    public async Task PasswordSignInAsync_TwoFactorRequired_RequiresTwoFactorAndKeepsFailedCount()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user-1", UserName = "admin", TwoFactorEnabled = true };
        var signInManager = CreateSignInManager();

        // Act
        var result = await signInManager.PasswordSignInAsync(user, Password, isPersistent: false, lockoutOnFailure: true);

        // Assert
        result.RequiresTwoFactor.Should().BeTrue();
        _userManager.Verify(x => x.ResetAccessFailedCountAsync(It.IsAny<ApplicationUser>()), Times.Never);
        _authenticationService.Verify(x => x.SignInAsync(_httpContext, IdentityConstants.TwoFactorUserIdScheme,
            It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
    }

    private VirtoSignInManager CreateSignInManager()
    {
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        schemes
            .Setup(x => x.GetSchemeAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => new AuthenticationScheme(name, displayName: null, typeof(CookieAuthenticationHandler)));

        return new VirtoSignInManager(
            _userManager.Object,
            new HttpContextAccessor { HttpContext = _httpContext },
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            NullLogger<SignInManager<ApplicationUser>>.Instance,
            schemes.Object,
            Mock.Of<IUserConfirmation<ApplicationUser>>(),
            _twoFactorSignInService.Object);
    }
}
