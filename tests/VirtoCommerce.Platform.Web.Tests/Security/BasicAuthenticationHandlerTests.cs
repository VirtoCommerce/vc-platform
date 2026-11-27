using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class BasicAuthenticationHandlerTests
{
    private const string Password = "secret";

    private readonly ApplicationUser _admin = new() { Id = "user-1", UserName = "admin" };
    private readonly ApplicationUser _twoFactorAdmin = new() { Id = "user-2", UserName = "mfa-admin", TwoFactorEnabled = true };
    private readonly ApplicationUser _administrator = new() { Id = "user-3", UserName = "administrator", IsAdministrator = true };
    private readonly Mock<IEventPublisher> _eventPublisher = new();
    private readonly PasswordLoginOptions _passwordLoginOptions = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<ITwoFactorSignInService> _twoFactorSignInService = new();

    // Emulates ASP.NET Core Identity lockout: only a lockout-aware check counts failures.
    private const int MaxFailedAccessAttempts = 5;
    private int _accessFailedCount;

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
            .ReturnsAsync((string userName) => new[] { _admin, _twoFactorAdmin, _administrator }.FirstOrDefault(x => x.UserName == userName));
        _userManager
            .Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser _, string password) => password == Password);
        _userManager
            .SetupGet(x => x.SupportsUserLockout)
            .Returns(true);
        _userManager
            .Setup(x => x.IsLockedOutAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(() => _accessFailedCount >= MaxFailedAccessAttempts);
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ApplicationUser _, string password, bool lockoutOnFailure) =>
            {
                if (_accessFailedCount >= MaxFailedAccessAttempts)
                {
                    return SignInResult.LockedOut;
                }

                if (password == Password)
                {
                    _accessFailedCount = 0;
                    return SignInResult.Success;
                }

                if (lockoutOnFailure && ++_accessFailedCount >= MaxFailedAccessAttempts)
                {
                    return SignInResult.LockedOut;
                }

                return SignInResult.Failed;
            });
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

    [Fact]
    public async Task AuthenticateAsync_RepeatedWrongPasswords_LockAccount()
    {
        // Arrange
        for (var i = 0; i < MaxFailedAccessAttempts; i++)
        {
            var wrongAttempt = await CreateHandlerAsync(_admin.UserName, "wrong-" + i);
            (await wrongAttempt.AuthenticateAsync()).Succeeded.Should().BeFalse();
        }

        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        _accessFailedCount.Should().Be(MaxFailedAccessAttempts);
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be("Invalid user name or password.");
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_PublishesFailedAttempt()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_admin.UserName, "wrong");

        // Act
        await handler.AuthenticateAsync();

        // Assert
        VerifyFailedAttemptPublished(_admin.UserName, _admin.Id, SignInFailureReason.InvalidPassword);
    }

    [Fact]
    public async Task AuthenticateAsync_UnknownUser_PublishesFailedAttempt()
    {
        // Arrange
        var handler = await CreateHandlerAsync("nobody", Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Failure.Message.Should().Be("Invalid user name or password.");
        VerifyFailedAttemptPublished("nobody", null, SignInFailureReason.UserNotFound);
    }

    [Fact]
    public async Task AuthenticateAsync_LockedOut_PublishesLockedOut()
    {
        // Arrange
        _accessFailedCount = MaxFailedAccessAttempts;
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Failure.Message.Should().Be("Invalid user name or password.");
        VerifyFailedAttemptPublished(_admin.UserName, _admin.Id, SignInFailureReason.LockedOut);
    }

    [Fact]
    public async Task AuthenticateAsync_Success_DoesNotPublishAttempt()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        await handler.AuthenticateAsync();

        // Assert
        _eventPublisher.Verify(x => x.Publish(It.IsAny<UserSignInAttemptEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        _eventPublisher.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task AuthenticateAsync_PasswordLoginDisabled_NonAdministratorFails()
    {
        // Arrange
        _passwordLoginOptions.Enabled = false;
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        VerifyFailedAttemptPublished(_admin.UserName, _admin.Id, SignInFailureReason.PasswordLoginDisabled);
        _signInManager.Verify(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_PasswordLoginDisabled_AdministratorSucceeds()
    {
        // Arrange
        _passwordLoginOptions.Enabled = false;
        var handler = await CreateHandlerAsync(_administrator.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeTrue();
    }

    private void VerifyFailedAttemptPublished(string userName, string userId, string failureReason)
    {
        _eventPublisher.Verify(x => x.Publish(
            It.Is<UserSignInAttemptEvent>(e =>
                e.UserName == userName &&
                e.UserId == userId &&
                !e.Succeeded &&
                e.FailureReason == failureReason &&
                e.SignInType == SignInType.Password),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private async Task<BasicAuthenticationHandler> CreateHandlerAsync(string userName, string password)
    {
        var options = new Mock<IOptionsMonitor<BasicAuthenticationOptions>>();
        options
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(new BasicAuthenticationOptions());

        var handler = new BasicAuthenticationHandler(options.Object, NullLoggerFactory.Instance, UrlEncoder.Default, _userManager.Object, _signInManager.Object, _twoFactorSignInService.Object,
            Options.Create(_passwordLoginOptions), _eventPublisher.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

        await handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationOptions.DefaultScheme, null, typeof(BasicAuthenticationHandler)), httpContext);

        return handler;
    }
}
