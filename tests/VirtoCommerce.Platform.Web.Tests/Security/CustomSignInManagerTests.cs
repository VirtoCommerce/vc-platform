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
using Xunit;

namespace VirtoCommerce.Platform.Web.Tests.Security;

public class CustomSignInManagerTests
{
    private readonly ApplicationUser _user = new() { Id = "user-1", UserName = "admin" };
    private readonly Mock<UserManager<ApplicationUser>> _userManager = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
    private readonly DefaultHttpContext _httpContext = new();

    [Fact]
    public async Task IsTwoFactorEnabledAsync_NothingRequiresIt_ReturnsFalse()
    {
        var policy = CreatePolicy(isRequired: false);

        var isEnabled = await CreateSignInManager(policy.Object).IsTwoFactorEnabledAsync(_user);

        isEnabled.Should().BeFalse();
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_AnyPolicyRequiresIt_ReturnsTrue()
    {
        var notRequiring = CreatePolicy(isRequired: false);
        var requiring = CreatePolicy(isRequired: true);

        var isEnabled = await CreateSignInManager(notRequiring.Object, requiring.Object).IsTwoFactorEnabledAsync(_user);

        isEnabled.Should().BeTrue();
        requiring.Verify(x => x.IsTwoFactorRequiredAsync(_user), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_UserEnabledTwoFactor_ReturnsTrueWithoutPolicies()
    {
        _userManager.SetupGet(x => x.SupportsUserTwoFactor).Returns(true);
        _userManager.Setup(x => x.GetTwoFactorEnabledAsync(_user)).ReturnsAsync(true);
        _userManager.Setup(x => x.GetValidTwoFactorProvidersAsync(_user)).ReturnsAsync(["Email"]);
        var policy = CreatePolicy(isRequired: false);

        var isEnabled = await CreateSignInManager(policy.Object).IsTwoFactorEnabledAsync(_user);

        isEnabled.Should().BeTrue();
        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    private static Mock<ITwoFactorPolicy> CreatePolicy(bool isRequired)
    {
        var policy = new Mock<ITwoFactorPolicy>();
        policy.Setup(x => x.IsTwoFactorRequiredAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(isRequired);

        return policy;
    }

    private CustomSignInManager CreateSignInManager(params ITwoFactorPolicy[] policies)
    {
        return new CustomSignInManager(
            _userManager.Object,
            new HttpContextAccessor { HttpContext = _httpContext },
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            NullLogger<SignInManager<ApplicationUser>>.Instance,
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<ApplicationUser>>(),
            [.. policies]);
    }
}
