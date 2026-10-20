using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.OpenIddict;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Security;

public class TwoFactorSignInServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

    public TwoFactorSignInServiceTests()
    {
        // Answers come from the user, like the Identity Email token provider: it needs a confirmed email.
        _userManager
            .SetupGet(x => x.SupportsUserTwoFactor)
            .Returns(true);
        _userManager
            .Setup(x => x.GetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser user) => user.TwoFactorEnabled);
        _userManager
            .Setup(x => x.GetValidTwoFactorProvidersAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser user) => user.EmailConfirmed ? [TokenOptions.DefaultEmailProvider] : new List<string>());
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_ReturnFalse_When_NothingRequiresIt()
    {
        // Arrange
        var user = CreateUser();
        var service = CreateService([CreatePolicy(isRequired: false).Object]);

        // Act
        var isRequired = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        isRequired.Should().BeFalse();
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_ReturnFalse_When_NoPolicyIsRegistered()
    {
        // Arrange
        var user = CreateUser();
        var service = CreateService([]);

        // Act
        var isRequired = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        isRequired.Should().BeFalse();
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_ReturnTrue_When_TheUserEnabledTwoFactor()
    {
        // Arrange
        var user = CreateUser(twoFactorEnabled: true, emailConfirmed: true);
        var policy = CreatePolicy(isRequired: false);
        var service = CreateService([policy.Object]);

        // Act
        var isRequired = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        isRequired.Should().BeTrue();
        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_AskThePolicies_When_TheUserHasNoValidTwoFactorProvider()
    {
        // Arrange
        var user = CreateUser(twoFactorEnabled: true, emailConfirmed: false);
        var policy = CreatePolicy(isRequired: false);
        var service = CreateService([policy.Object]);

        // Act
        var isRequired = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        isRequired.Should().BeFalse();
        policy.Verify(x => x.IsTwoFactorRequiredAsync(user), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_ReturnTrue_When_AnyPolicyRequiresIt()
    {
        // Arrange
        var user = CreateUser();
        var requiring = CreatePolicy(isRequired: true);
        var service = CreateService([CreatePolicy(isRequired: false).Object, requiring.Object]);

        // Act
        var isRequired = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        isRequired.Should().BeTrue();
        requiring.Verify(x => x.IsTwoFactorRequiredAsync(user), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorRequiredAsync_Should_EvaluatePoliciesOnce_When_CalledTwiceForOneUser()
    {
        // Arrange
        var user = CreateUser();
        var policy = CreatePolicy(isRequired: true);
        var service = CreateService([policy.Object]);

        // Act
        var first = await service.IsTwoFactorRequiredAsync(user);
        var second = await service.IsTwoFactorRequiredAsync(user);

        // Assert
        first.Should().BeTrue();
        second.Should().BeTrue();
        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()), Times.Once);
    }

    [Fact]
    public void IsTwoFactorAvailable_Should_BeFalse_When_NoHandlerIsRegistered()
    {
        // Arrange
        var service = CreateService([]);

        // Act
        var isAvailable = service.IsTwoFactorAvailable;

        // Assert
        isAvailable.Should().BeFalse();
    }

    [Fact]
    public void IsTwoFactorAvailable_Should_BeTrue_When_AHandlerIsRegistered()
    {
        // Arrange
        var service = CreateService([], [CreateHandler(new BadRequestResult()).Object]);

        // Act
        var isAvailable = service.IsTwoFactorAvailable;

        // Assert
        isAvailable.Should().BeTrue();
    }

    [Fact]
    public async Task ChallengeAsync_Should_RefuseWithTwoFactorUnavailable_When_NoHandlerIsRegistered()
    {
        // Arrange
        var context = new TokenRequestContext();
        var service = CreateService([]);

        // Act
        var actionResult = await service.ChallengeAsync(context);

        // Assert
        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().BeOfType<TokenResponse>().Which.Code.Should().Be("two_factor_unavailable");
        context.FailureReason.Should().Be(SignInFailureReason.TwoFactorUnavailable);
    }

    [Fact]
    public async Task ChallengeAsync_Should_UseTheLastRegisteredHandler()
    {
        // Arrange
        var context = new TokenRequestContext();
        var firstHandler = CreateHandler(new BadRequestObjectResult("first"));
        var lastHandler = CreateHandler(new BadRequestObjectResult("last"));
        var service = CreateService([], [firstHandler.Object, lastHandler.Object]);

        // Act
        var actionResult = await service.ChallengeAsync(context);

        // Assert
        actionResult.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be("last");
        firstHandler.Verify(x => x.ChallengeAsync(It.IsAny<TokenRequestContext>()), Times.Never);
        context.FailureReason.Should().Be(SignInFailureReason.RequiresTwoFactor);
    }

    [Fact]
    public async Task ChallengeAsync_Should_KeepTheReasonTheHandlerSets()
    {
        // Arrange
        var context = new TokenRequestContext();
        var handler = CreateHandler(new BadRequestResult(), failureReason: "CustomReason");
        var service = CreateService([], [handler.Object]);

        // Act
        await service.ChallengeAsync(context);

        // Assert
        context.FailureReason.Should().Be("CustomReason");
    }

    private static ApplicationUser CreateUser(bool twoFactorEnabled = false, bool emailConfirmed = false)
    {
        return new ApplicationUser { Id = "user-1", UserName = "admin", TwoFactorEnabled = twoFactorEnabled, EmailConfirmed = emailConfirmed };
    }

    private TwoFactorSignInService CreateService(ITwoFactorPolicy[] policies, ITwoFactorSignInHandler[] handlers = null)
    {
        return new TwoFactorSignInService(_userManager.Object, policies, handlers ?? []);
    }

    private static Mock<ITwoFactorPolicy> CreatePolicy(bool isRequired)
    {
        var policy = new Mock<ITwoFactorPolicy>();
        policy
            .Setup(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(isRequired);

        return policy;
    }

    private static Mock<ITwoFactorSignInHandler> CreateHandler(ActionResult challengeResult, string failureReason = null)
    {
        var handler = new Mock<ITwoFactorSignInHandler>();
        handler
            .Setup(x => x.ChallengeAsync(It.IsAny<TokenRequestContext>()))
            .Callback<TokenRequestContext>(x => x.FailureReason = failureReason)
            .ReturnsAsync(challengeResult);

        return handler;
    }
}
