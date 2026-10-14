using System.Collections.Generic;
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
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
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
        requiring.Verify(x => x.IsTwoFactorRequiredAsync(It.Is<TwoFactorPolicyContext>(c => c.User == _user)), Times.Once);
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
        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<TwoFactorPolicyContext>()), Times.Never);
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_TokenRequest_PassesGrantTypeAndParametersToPolicies()
    {
        var request = new OpenIddictRequest { GrantType = "password", Username = "admin" };
        request.SetParameter("storeId", "B2B-store");
        SetTokenRequest(request);
        var policy = CreatePolicy(isRequired: false);

        await CreateSignInManager(policy.Object).IsTwoFactorEnabledAsync(_user);

        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.Is<TwoFactorPolicyContext>(c =>
            c.GrantType == "password" &&
            c.Parameters["STOREID"] == "B2B-store")), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_NoPolicies_ReturnsFalse()
    {
        var isEnabled = await CreateSignInManager().IsTwoFactorEnabledAsync(_user);

        isEnabled.Should().BeFalse();
    }

    [Theory]
    [InlineData(OpenIddictConstants.Parameters.Password)]
    [InlineData(OpenIddictConstants.Parameters.ClientSecret)]
    [InlineData(OpenIddictConstants.Parameters.RefreshToken)]
    [InlineData(OpenIddictConstants.Parameters.Code)]
    [InlineData(OpenIddictConstants.Parameters.ClientAssertion)]
    [InlineData(OpenIddictConstants.Parameters.Assertion)]
    public async Task IsTwoFactorEnabledAsync_TokenRequest_ExcludesSecretsFromTheContext(string secretParameter)
    {
        var request = new OpenIddictRequest { GrantType = "password" };
        request.SetParameter(secretParameter, "secret");
        SetTokenRequest(request);
        var policy = CreatePolicy(isRequired: false);

        await CreateSignInManager(policy.Object).IsTwoFactorEnabledAsync(_user);

        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.Is<TwoFactorPolicyContext>(c =>
            c.GrantType == "password" && !c.Parameters.ContainsKey(secretParameter))), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_NotATokenRequest_PassesNoGrantType()
    {
        var policy = CreatePolicy(isRequired: false);

        await CreateSignInManager(policy.Object).IsTwoFactorEnabledAsync(_user);

        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.Is<TwoFactorPolicyContext>(c =>
            c.GrantType == null && c.Parameters.Count == 0)), Times.Once);
    }

    [Fact]
    public async Task IsTwoFactorEnabledAsync_CalledTwiceForOneUser_EvaluatesPoliciesOnce()
    {
        var policy = CreatePolicy(isRequired: true);
        var signInManager = CreateSignInManager(policy.Object);

        (await signInManager.IsTwoFactorEnabledAsync(_user)).Should().BeTrue();
        (await signInManager.IsTwoFactorEnabledAsync(_user)).Should().BeTrue();

        policy.Verify(x => x.IsTwoFactorRequiredAsync(It.IsAny<TwoFactorPolicyContext>()), Times.Once);
    }

    [Fact]
    public async Task PasswordSignInAsync_PolicyRequiresTwoFactor_RequiresTwoFactorAndKeepsFailedCount()
    {
        _userManager.SetupGet(x => x.SupportsUserLockout).Returns(true);
        _userManager.Setup(x => x.IsLockedOutAsync(_user)).ReturnsAsync(false);
        _userManager.Setup(x => x.CheckPasswordAsync(_user, "secret")).ReturnsAsync(true);
        _userManager.Setup(x => x.GetValidTwoFactorProvidersAsync(_user)).ReturnsAsync(["Email"]);
        _userManager.Setup(x => x.GetUserIdAsync(_user)).ReturnsAsync(_user.Id);
        var authenticationService = new Mock<IAuthenticationService>();
        authenticationService
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.NoResult());
        _httpContext.RequestServices = new ServiceCollection().AddSingleton(authenticationService.Object).BuildServiceProvider();

        var result = await CreateSignInManager(CreatePolicy(isRequired: true).Object)
            .PasswordSignInAsync(_user, "secret", isPersistent: false, lockoutOnFailure: true);

        result.RequiresTwoFactor.Should().BeTrue();
        _userManager.Verify(x => x.ResetAccessFailedCountAsync(It.IsAny<ApplicationUser>()), Times.Never);
        authenticationService.Verify(x => x.SignInAsync(_httpContext, IdentityConstants.TwoFactorUserIdScheme,
            It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
    }

    private void SetTokenRequest(OpenIddictRequest request)
    {
        _httpContext.Features.Set(new OpenIddictServerAspNetCoreFeature
        {
            Transaction = new OpenIddictServerTransaction { Request = request },
        });
    }

    private static Mock<ITwoFactorPolicy> CreatePolicy(bool isRequired)
    {
        var policy = new Mock<ITwoFactorPolicy>();
        policy.Setup(x => x.IsTwoFactorRequiredAsync(It.IsAny<TwoFactorPolicyContext>())).ReturnsAsync(isRequired);

        return policy;
    }

    private CustomSignInManager CreateSignInManager(params ITwoFactorPolicy[] policies)
    {
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        schemes
            .Setup(x => x.GetSchemeAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => new AuthenticationScheme(name, displayName: null, typeof(CookieAuthenticationHandler)));

        return new CustomSignInManager(
            _userManager.Object,
            new HttpContextAccessor { HttpContext = _httpContext },
            Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
            Options.Create(new IdentityOptions()),
            NullLogger<SignInManager<ApplicationUser>>.Instance,
            schemes.Object,
            Mock.Of<IUserConfirmation<ApplicationUser>>(),
            new List<ITwoFactorPolicy>(policies));
    }
}
