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
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Web.Security.Authentication;
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class BasicAuthenticationHandlerTests
{
    private const string Password = "secret";
    private const string InvalidCredentialsMessage = "Invalid user name or password.";

    private readonly ApplicationUser _admin = new() { Id = "user-1", UserName = "admin" };
    private readonly ApplicationUser _administrator = new() { Id = "user-3", UserName = "administrator", IsAdministrator = true };
    private readonly ApplicationUser _notAllowedUser = new() { Id = "user-4", UserName = "not-confirmed" };
    private readonly ApplicationUser _duplicateEmailUser = new() { Id = "user-5", UserName = "duplicate", Email = "shared@example.test" };
    private readonly PasswordLoginOptions _passwordLoginOptions = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;

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

        _userManager
            .Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string userName) => new[] { _admin, _administrator, _notAllowedUser, _duplicateEmailUser }.FirstOrDefault(x => x.UserName == userName));
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ApplicationUser user, string password, bool lockoutOnFailure) =>
            {
                if (user == _notAllowedUser)
                {
                    return SignInResult.NotAllowed;
                }

                // Saving the failure count validates the user, and the platform's FindByEmailAsync throws for a duplicated email.
                if (user == _duplicateEmailUser && password != Password && lockoutOnFailure)
                {
                    throw new DuplicateEmailException(user.Email);
                }

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
            .Setup(x => x.CreateUserPrincipalAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity("test")));
    }

    [Fact]
    public async Task AuthenticateAsync_CorrectPassword_Succeeds()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeTrue();
        _signInManager.Verify(x => x.CheckPasswordSignInAsync(_admin, Password, true), Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_Fails()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_admin.UserName, "wrong");

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
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
        // The last wrong password locks the account, and the correct password is then refused as locked out.
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
        _signInManager.Verify(x => x.CheckPasswordSignInAsync(_admin, It.IsAny<string>(), true), Times.Exactly(MaxFailedAccessAttempts + 1));
        _signInManager.Verify(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), false), Times.Never);
    }

    [Fact]
    public async Task AuthenticateAsync_LockedOut_FailsWithGenericMessage()
    {
        // Arrange
        _accessFailedCount = MaxFailedAccessAttempts;
        var handler = await CreateHandlerAsync(_admin.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
    }

    [Fact]
    public async Task AuthenticateAsync_NotAllowed_Fails()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_notAllowedUser.UserName, Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
    }

    [Fact]
    public async Task AuthenticateAsync_DuplicateEmail_FailsInsteadOfThrowing()
    {
        // Arrange
        var handler = await CreateHandlerAsync(_duplicateEmailUser.UserName, "wrong");

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Should().NotBeOfType<DuplicateEmailException>();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
    }

    [Fact]
    public async Task AuthenticateAsync_UnknownUser_Fails()
    {
        // Arrange
        var handler = await CreateHandlerAsync("nobody", Password);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
        _signInManager.Verify(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
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
        result.Failure.Message.Should().Be(InvalidCredentialsMessage);
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

    private async Task<BasicAuthenticationHandler> CreateHandlerAsync(string userName, string password)
    {
        var options = new Mock<IOptionsMonitor<BasicAuthenticationOptions>>();
        options
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(new BasicAuthenticationOptions());

        var handler = new BasicAuthenticationHandler(options.Object, NullLoggerFactory.Instance, UrlEncoder.Default, _userManager.Object, _signInManager.Object,
            Options.Create(_passwordLoginOptions));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));

        await handler.InitializeAsync(new AuthenticationScheme(BasicAuthenticationOptions.DefaultScheme, null, typeof(BasicAuthenticationHandler)), httpContext);

        return handler;
    }
}
