using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.ExternalSignIn;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Security.Model.OpenIddict;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Web.Controllers.Api;
using VirtoCommerce.Platform.Web.Security;
using Xunit;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Web.Tests.Controllers.Api;

/// <summary>
/// Covers what the token endpoint records for audit. These tests assert only on published
/// <see cref="UserSignInAttemptEvent"/>s — the authentication logic itself is deliberately unchanged.
/// </summary>
public class AuthorizationControllerSignInLogTests
{
    private readonly List<UserSignInAttemptEvent> _published = [];
    private readonly Mock<IEventPublisher> _eventPublisher = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<SignInManager<ApplicationUser>> _signInManager;
    private readonly Mock<IAuthorizationService> _authorizationService = new();
    private readonly Mock<IOpenIddictAuthorizationManager> _authorizationManager = new();
    private readonly Mock<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>> _applicationManager;

    public AuthorizationControllerSignInLogTests()
    {
        // Only FindByClientIdAsync and GetIdAsync are exercised, and both are overridden below, so the
        // cache and store are never reached - they exist because the base constructor rejects nulls.
        _applicationManager = new Mock<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>(
            Mock.Of<IOpenIddictApplicationCache<VirtoOpenIddictEntityFrameworkCoreApplication>>(),
            NullLogger<OpenIddictApplicationManager<VirtoOpenIddictEntityFrameworkCoreApplication>>.Instance,
            Mock.Of<IOptionsMonitor<OpenIddictCoreOptions>>(x => x.CurrentValue == new OpenIddictCoreOptions()),
            Mock.Of<IOpenIddictApplicationStore<VirtoOpenIddictEntityFrameworkCoreApplication>>());

        var application = new VirtoOpenIddictEntityFrameworkCoreApplication { Id = "app-1" };

        // Faithful to OpenIddict: FindByClientIdAsync calls ArgumentException.ThrowIfNullOrEmpty and
        // throws on a missing identifier rather than returning null. A mock that quietly answered any
        // string is why a token request with no client_id reached production as a 500.
        _applicationManager
            .Setup(x => x.FindByClientIdAsync(It.Is<string>(s => string.IsNullOrEmpty(s)), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentNullException("identifier"));
        _applicationManager
            .Setup(x => x.FindByClientIdAsync(It.Is<string>(s => !string.IsNullOrEmpty(s)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(application);
        _applicationManager
            .Setup(x => x.GetIdAsync(It.IsAny<VirtoOpenIddictEntityFrameworkCoreApplication>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("app-1");

        var authorization = new object();

        _authorizationManager
            .Setup(x => x.CreateAsync(
                It.IsAny<ClaimsPrincipal>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<ImmutableArray<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(authorization);
        _authorizationManager
            .Setup(x => x.GetIdAsync(authorization, It.IsAny<CancellationToken>()))
            .ReturnsAsync("auth-1");

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

        _signInManager
            .Setup(x => x.CreateUserPrincipalAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser u) => new ClaimsPrincipal(
                new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, u.Id ?? "x")], "test")));
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_PublishesImpersonationWithOperatorIdentity()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var target = new ApplicationUser
        {
            Id = "user-1",
            UserName = "b2badmin@test.com",
            StoreId = "B2B-store",
            MemberId = "member-1",
        };

        var controller = CreateController(operatorUser, permitted: true, targetUserId: "user-1", target: target);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
        attempt.UserName.Should().Be("b2badmin@test.com");
        attempt.OperatorUserId.Should().Be("op-1");
        attempt.OperatorUserName.Should().Be("support@virtocommerce.com");
        attempt.StoreId.Should().Be("B2B-store");
        attempt.MemberId.Should().Be("member-1");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_WithoutPermission_PublishesForbidden()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "nosy@test.com" };

        var controller = CreateController(operatorUser, permitted: false, targetUserId: "user-1", target: null);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.Forbidden);
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.OperatorUserName.Should().Be("nosy@test.com");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_TargetNotFound_PublishesUserNotFound()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };

        var controller = CreateController(operatorUser, permitted: true, targetUserId: "ghost", target: null);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.UserNotFound);
        attempt.OperatorUserId.Should().Be("op-1");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_NoUserId_PublishesRevert()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };

        // No user_id parameter means "revert to the operator", and FindByIdAsync resolves the operator.
        var controller = CreateController(operatorUser, permitted: true, targetUserId: null, target: operatorUser);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.ImpersonationRevert);
        attempt.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_Revert_NamesTheCustomerAndTheRealOperator()
    {
        // The live shape of a revert: the request is authenticated as the customer, and the operator
        // only exists in the claims the impersonation grant attached.
        var customer = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };

        var controller = CreateController(customer, permitted: true, targetUserId: null, target: operatorUser,
            principalClaims:
            [
                new Claim(PlatformConstants.Security.Claims.OperatorUserId, "op-1"),
                new Claim(PlatformConstants.Security.Claims.OperatorUserName, "support@virtocommerce.com"),
            ]);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.ImpersonationRevert);

        // The row is about the customer being left, driven by the operator. Reading the operator after
        // the revert branch cleared it used to invert both columns: the operator was recorded as the
        // user and the customer as their own operator.
        attempt.UserId.Should().Be("user-1");
        attempt.UserName.Should().Be("b2badmin@test.com");
        attempt.OperatorUserId.Should().Be("op-1");
        attempt.OperatorUserName.Should().Be("support@virtocommerce.com");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_WithoutClientId_StillGrantsAndRecords()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var target = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };

        // The storefront's impersonate call carries no client_id at all:
        //   grant_type=impersonate&scope=offline_access&user_id=...
        // Resolving an application from a null identifier threw, and the whole grant answered 500.
        var controller = CreateController(operatorUser, permitted: true, targetUserId: "user-1", target: target,
            scope: OpenIddictConstants.Scopes.OfflineAccess, clientId: null);

        var result = await controller.Exchange();

        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.SignInResult>();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
        attempt.OperatorUserId.Should().Be("op-1");

        // Without a client there is no application to hang an authorization on, so the row simply
        // carries no session id rather than failing the sign-in.
        attempt.SessionId.Should().BeNull();
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_Unauthenticated_PublishesForbidden()
    {
        var controller = CreateController(currentUser: null, permitted: false, targetUserId: "user-1", target: null);

        await controller.Exchange();

        // No operator to name, but a call to the impersonate grant with no session at all is exactly
        // what an audit trail exists to surface.
        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.Forbidden);
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.UserId.Should().Be("user-1");
        attempt.OperatorUserId.Should().BeNull();
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_ValidatorRejects_PublishesNotAllowed()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var target = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };

        var controller = CreateController(operatorUser, permitted: true, targetUserId: "user-1", target: target,
            validators: [RejectingValidator("Password expired")]);

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.NotAllowed);
        attempt.UserId.Should().Be("user-1");
        attempt.OperatorUserId.Should().Be("op-1");
    }

    [Fact]
    public async Task Exchange_ImpersonateGrant_PublishesTheSessionIdOfTheAuthorizationItCreates()
    {
        var operatorUser = new ApplicationUser { Id = "op-1", UserName = "support@virtocommerce.com" };
        var target = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };

        var controller = CreateController(operatorUser, permitted: true, targetUserId: "user-1", target: target,
            // offline_access is what makes this a session Active Sessions can list.
            scope: OpenIddictConstants.Scopes.OfflineAccess);

        await controller.Exchange();

        // OpenIddict only assigns an authorization id while it processes the SignIn result, so reading
        // it off the freshly built ticket - as this used to - always produced null, and the Active
        // Sessions join never matched a single impersonation row.
        _published.Should().ContainSingle().Which.SessionId.Should().Be("auth-1");
    }

    [Fact]
    public async Task Exchange_PasswordGrant_UnknownUser_PublishesUserNotFound()
    {
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
        _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

        var controller = BuildController(new OpenIddictRequest
        {
            GrantType = OpenIddictConstants.GrantTypes.Password,
            Username = "ghost@test.com",
            Password = "whatever",
            ClientId = "frontend",
        });

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.UserName.Should().Be("ghost@test.com");
        attempt.UserId.Should().BeNull();
        attempt.Succeeded.Should().BeFalse();
        attempt.FailureReason.Should().Be(SignInFailureReason.UserNotFound);
        attempt.SignInType.Should().Be(SignInType.Password);
        attempt.ClientId.Should().Be("frontend");
    }

    [Fact]
    public async Task Exchange_PasswordGrant_ValidatorRejects_PublishesNotAllowedNotInvalidPassword()
    {
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var controller = BuildController(
            new OpenIddictRequest
            {
                GrantType = OpenIddictConstants.GrantTypes.Password,
                Username = "b2badmin@test.com",
                Password = "right",
                ClientId = "frontend",
            },
            validators: [RejectingValidator("Password expired")]);

        await controller.Exchange();

        // The password itself was accepted - something after the check rejected the request. Recording
        // that as a bad password sends an administrator hunting a credential-stuffing run that never
        // happened, and hides the real cause.
        _published.Should().ContainSingle().Which.FailureReason.Should().Be(SignInFailureReason.NotAllowed);
    }

    [Fact]
    public async Task Exchange_PasswordGrant_Success_PublishesSucceededAttemptWithStore()
    {
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com", StoreId = "B2B-store" };
        _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
        _signInManager
            .Setup(x => x.CheckPasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var controller = BuildController(new OpenIddictRequest
        {
            GrantType = OpenIddictConstants.GrantTypes.Password,
            Username = "b2badmin@test.com",
            Password = "right",
            ClientId = "frontend",
        });

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.Succeeded.Should().BeTrue();
        attempt.UserId.Should().Be("user-1");
        attempt.StoreId.Should().Be("B2B-store");
        attempt.SignInType.Should().Be(SignInType.Password);
    }

    [Fact]
    public async Task Exchange_RefreshToken_WithOperatorClaims_PublishesImpersonation()
    {
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = BuildRefreshController(user, operatorUserId: "op-1", operatorUserName: "support@virtocommerce.com");

        await controller.Exchange();

        var attempt = _published.Should().ContainSingle().Subject;
        attempt.SignInType.Should().Be(SignInType.Impersonation);
        attempt.OperatorUserId.Should().Be("op-1");
        attempt.OperatorUserName.Should().Be("support@virtocommerce.com");
        attempt.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Exchange_RefreshToken_WithoutOperatorClaims_PublishesNothing()
    {
        var user = new ApplicationUser { Id = "user-1", UserName = "b2badmin@test.com" };
        var controller = BuildRefreshController(user, operatorUserId: null, operatorUserName: null);

        await controller.Exchange();

        // Ordinary token rotation is high volume and low value - deliberately not audited.
        _published.Should().BeEmpty();
    }

    private AuthorizationController BuildRefreshController(ApplicationUser user, string operatorUserId, string operatorUserName)
    {
        _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        _signInManager.Setup(x => x.CanSignInAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(true);

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id) };
        if (operatorUserId != null)
        {
            claims.Add(new Claim(PlatformConstants.Security.Claims.OperatorUserId, operatorUserId));
            claims.Add(new Claim(PlatformConstants.Security.Claims.OperatorUserName, operatorUserName));
        }

        var refreshPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

        var authService = new Mock<IAuthenticationService>();
        authService
            .Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.Success(
                new AuthenticationTicket(refreshPrincipal, new AuthenticationProperties(), "test")));

        var services = new ServiceCollection();
        services.AddSingleton(authService.Object);

        var controller = BuildController(new OpenIddictRequest
        {
            GrantType = OpenIddictConstants.GrantTypes.RefreshToken,
            ClientId = "frontend",
        });

        controller.ControllerContext.HttpContext.RequestServices = services.BuildServiceProvider();

        return controller;
    }

    /// <summary>
    /// A token request validator that always rejects, standing in for the password-expiry validator.
    /// </summary>
    private static ITokenRequestValidator RejectingValidator(string error)
    {
        var validator = new Mock<ITokenRequestValidator>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<TokenRequestContext>()))
            .ReturnsAsync([new TokenResponse { Error = error }]);

        return validator.Object;
    }

    private AuthorizationController CreateController(
        ApplicationUser currentUser,
        bool permitted,
        string targetUserId,
        ApplicationUser target,
        IEnumerable<Claim> principalClaims = null,
        IEnumerable<ITokenRequestValidator> validators = null,
        string scope = null,
        string clientId = "frontend")
    {
        _userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(currentUser);
        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(target);
        _userManager.Setup(x => x.GetUserIdAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync((ApplicationUser u) => u.Id);

        _authorizationService
            .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
            .ReturnsAsync(permitted ? AuthorizationResult.Success() : AuthorizationResult.Failed());

        var request = new OpenIddictRequest
        {
            GrantType = PlatformConstants.Security.GrantTypes.Impersonate,
            ClientId = clientId,
            Scope = scope,
        };

        if (targetUserId != null)
        {
            request.SetParameter("user_id", targetUserId);
        }

        return BuildController(request, principalClaims, validators);
    }

    private AuthorizationController BuildController(
        OpenIddictRequest request,
        IEnumerable<Claim> principalClaims = null,
        IEnumerable<ITokenRequestValidator> validators = null)
    {
        var controller = new AuthorizationController(
            applicationManager: _applicationManager.Object,
            identityOptions: Options.Create(new IdentityOptions()),
            signInManager: _signInManager.Object,
            passwordLoginOptions: Options.Create(new PasswordLoginOptions { Enabled = true }),
            eventPublisher: _eventPublisher.Object,
            requestValidators: validators ?? [],
            claimProviders: [],
            requestHandlers: [],
            // The audited branches never touch the token manager, and the constructor only assigns
            // fields. Passing null keeps this harness off OpenIddict's internal store plumbing.
            tokenManager: null,
            authorizationService: _authorizationService.Object,
            externalSignInService: Mock.Of<IExternalSignInService>(),
            authorizationManager: _authorizationManager.Object,
            scopeManager: Mock.Of<IOpenIddictScopeManager>(),
            authorizationOptions: Options.Create(new Core.Security.AuthorizationOptions()));

        var claims = new List<Claim> { new(ClaimTypes.Name, "operator") };
        claims.AddRange(principalClaims ?? []);

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test")),
        };

        // GetOpenIddictServerRequest() reads the request off the server transaction feature.
        httpContext.Features.Set(new OpenIddictServerAspNetCoreFeature
        {
            Transaction = new OpenIddictServerTransaction { Request = request },
        });

        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        return controller;
    }
}
