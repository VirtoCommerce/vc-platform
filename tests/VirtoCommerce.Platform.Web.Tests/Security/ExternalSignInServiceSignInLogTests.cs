using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Security.ExternalSignIn;
using VirtoCommerce.Platform.Web.Security;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class ExternalSignInServiceSignInLogTests
{
    private readonly List<UserSignInAttemptEvent> _published = [];
    private readonly Mock<IEventPublisher> _eventPublisher = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;

    public ExternalSignInServiceSignInLogTests()
    {
        _eventPublisher
            .Setup(x => x.Publish(It.IsAny<UserSignInAttemptEvent>(), It.IsAny<CancellationToken>()))
            .Callback<UserSignInAttemptEvent, CancellationToken>((e, _) => _published.Add(e))
            .Returns(Task.CompletedTask);

        _userManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _signInManager = new Mock<SignInManager<ApplicationUser>>(
            _userManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            null, null, null, null);
    }

    [Fact]
    public async Task SignInAsync_Success_PublishesExternalAttemptWithProvider()
    {
        var user = new ApplicationUser
        {
            Id = "user-1",
            UserName = "b2badmin@test.com",
            StoreId = "B2B-store",
        };

        var service = CreateService(user, SignInResult.Success);

        await service.SignInAsync();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.External);
        attempt.Provider.Should().Be("AzureAD");
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
        attempt.StoreId.Should().Be("B2B-store");
    }

    [Fact]
    public async Task SignInAsync_LockedOut_PublishesLockedOut()
    {
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };

        var service = CreateService(user, SignInResult.LockedOut);

        await service.SignInAsync();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.LockedOut);
        attempt.Provider.Should().Be("AzureAD");
    }

    private ExternalSignInService CreateService(ApplicationUser user, SignInResult signInResult)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, "external-key"),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.UserName),
        ], "AzureAD"));

        var loginInfo = new ExternalLoginInfo(principal, "AzureAD", "external-key", "Azure AD");

        _signInManager.Setup(x => x.GetExternalLoginInfoAsync(It.IsAny<string>())).ReturnsAsync(loginInfo);
        _signInManager
            .Setup(x => x.ExternalLoginSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(signInResult);

        _userManager.Setup(x => x.FindByLoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);
        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

        var provider = new Mock<IExternalSignInProvider>();
        provider.Setup(x => x.GetUserName(It.IsAny<ExternalLoginInfo>())).Returns(user.UserName);
        provider.Setup(x => x.GetEmail(It.IsAny<ExternalLoginInfo>())).Returns(user.UserName);

        var providerConfig = new ExternalSignInProviderConfiguration
        {
            AuthenticationType = "AzureAD",
            Provider = provider.Object,
        };

        return new ExternalSignInService(
            _signInManager.Object,
            _eventPublisher.Object,
            Options.Create(new IdentityOptions()),
            Mock.Of<ISettingsManager>(),
            [providerConfig],
            []);
    }
}
