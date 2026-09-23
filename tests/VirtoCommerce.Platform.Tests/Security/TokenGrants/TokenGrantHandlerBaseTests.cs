using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Security.TokenGrants;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace VirtoCommerce.Platform.Tests.Security.TokenGrants;

public class TokenGrantHandlerBaseTests
{
    private const string _grantType = "test_grant";

    [Fact]
    public async Task HandleAsync_Should_ReturnFailed_When_AuthenticationFails()
    {
        var authError = new TokenResponse { Code = "bad_credential" };
        var context = CreateContext(authenticationResult: GrantAuthenticationResult.Failed(authError));

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        Assert.False(result.Success);
        Assert.Same(authError, result.Error);
        context.SignInManager.Verify(x => x.CanSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnSignInNotAllowed_When_UserCannotSignIn()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(authenticationResult: GrantAuthenticationResult.Authenticated(user));
        context.SignInManager.Setup(x => x.CanSignInAsync(user)).ReturnsAsync(false);

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        Assert.False(result.Success);
        Assert.Equal("sign_in_not_allowed", result.Error.Code);
        context.SignInManager.Verify(x => x.CreateUserPrincipalAsync(It.IsAny<ApplicationUser>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnFirstValidatorError_When_ARequestValidatorRejects()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var validatorError = new TokenResponse { Code = "custom_error" };
        var validator = new Mock<ITokenRequestValidator>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<TokenRequestContext>()))
            .ReturnsAsync((IList<TokenResponse>)[validatorError]);

        var context = CreateContext(
            authenticationResult: GrantAuthenticationResult.Authenticated(user),
            requestValidators: [validator.Object]);
        context.SignInManager.Setup(x => x.CanSignInAsync(user)).ReturnsAsync(true);

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        Assert.False(result.Success);
        Assert.Same(validatorError, result.Error);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnDuplicateEmailError_When_UpdatingLastLoginDateThrows()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(authenticationResult: GrantAuthenticationResult.Authenticated(user));
        context.SignInManager.Setup(x => x.CanSignInAsync(user)).ReturnsAsync(true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(user)).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));
        context.UserManager.Setup(x => x.UpdateAsync(user)).ThrowsAsync(new DuplicateEmailException("duplicate"));

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        Assert.False(result.Success);
        Assert.Equal("duplicate_email_login_attempt", result.Error.Code);
    }

    [Fact]
    public async Task HandleAsync_Should_SignIn_And_RunTheFullPipeline_When_AuthenticationSucceeds()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var claimProvider = new Mock<ITokenClaimProvider>();
        var requestHandler = new Mock<ITokenRequestHandler>();

        var context = CreateContext(
            authenticationResult: GrantAuthenticationResult.Authenticated(user),
            claimProviders: [claimProvider.Object],
            requestHandlers: [requestHandler.Object]);
        context.SignInManager.Setup(x => x.CanSignInAsync(user)).ReturnsAsync(true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(user)).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        result.Success.Should().BeTrue();
        result.Principal.Should().NotBeNull();
        result.Principal.FindFirst(ClaimTypes.AuthenticationMethod)?.Value.Should().Be(_grantType);
        user.LastLoginDate.Should().NotBeNull();
        context.UserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Once);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<UserLoginEvent>()), Times.Once);
        requestHandler.Verify(x => x.HandleAsync(user, context.RequestContext), Times.Once);
        claimProvider.Verify(x => x.SetClaimsAsync(It.IsAny<ClaimsPrincipal>(), context.RequestContext), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_HonorOverridesThatSkipSignInCheckAndLastLoginUpdate()
    {
        // Proves CanSignInAsync/LastLoginDate/Before-AfterSignIn are each independently skippable.
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(GrantAuthenticationResult.Authenticated(user), skipOptionalSteps: true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(user)).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        var result = await context.Handler.ProcessGrantAsync(context.RequestContext);

        result.Success.Should().BeTrue();
        context.SignInManager.Verify(x => x.CanSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        context.UserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        user.LastLoginDate.Should().BeNull();
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Never);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<UserLoginEvent>()), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnSignInResult_When_AuthenticationSucceeds()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(authenticationResult: GrantAuthenticationResult.Authenticated(user));
        context.SignInManager.Setup(x => x.CanSignInAsync(user)).ReturnsAsync(true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(user)).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var signInResult = actionResult.Should().BeOfType<SignInResult>().Subject;
        signInResult.Principal.Should().NotBeNull();
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnBadRequest_When_AuthenticationFails()
    {
        var authError = new TokenResponse { Code = "bad_credential" };
        var context = CreateContext(authenticationResult: GrantAuthenticationResult.Failed(authError));

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().BeSameAs(authError);
    }

    private static TestContext CreateContext(
        GrantAuthenticationResult authenticationResult,
        IEnumerable<ITokenRequestValidator> requestValidators = null,
        IEnumerable<ITokenClaimProvider> claimProviders = null,
        IEnumerable<ITokenRequestHandler> requestHandlers = null,
        bool skipOptionalSteps = false)
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object, null, null, null, null, null, null, null, null);

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var identityOptions = Options.Create(new IdentityOptions());
        var logger = new Mock<ILogger<SignInManager<ApplicationUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();
        var confirmation = new Mock<IUserConfirmation<ApplicationUser>>();

        var signInManager = new Mock<SignInManager<ApplicationUser>>(
            userManager.Object, contextAccessor.Object, claimsFactory.Object, identityOptions, logger.Object, schemes.Object, confirmation.Object);

        var eventPublisher = new Mock<IEventPublisher>();

        var handler = new TestGrantHandler(
            authenticationResult,
            skipOptionalSteps,
            signInManager.Object,
            identityOptions,
            requestValidators ?? [],
            claimProviders ?? [],
            requestHandlers ?? [],
            eventPublisher.Object);

        var requestContext = new TokenRequestContext
        {
            AuthenticationScheme = "test-scheme",
            Request = new OpenIddict.Abstractions.OpenIddictRequest { GrantType = _grantType },
            Properties = new AuthenticationProperties(),
        };

        return new TestContext(handler, requestContext, signInManager, userManager, eventPublisher);
    }

    private sealed record TestContext(
        TestGrantHandler Handler,
        TokenRequestContext RequestContext,
        Mock<SignInManager<ApplicationUser>> SignInManager,
        Mock<UserManager<ApplicationUser>> UserManager,
        Mock<IEventPublisher> EventPublisher);

    private sealed class TestGrantHandler : TokenGrantHandlerBase
    {
        private readonly GrantAuthenticationResult _authenticationResult;
        private readonly bool _skipOptionalSteps;

        public TestGrantHandler(
            GrantAuthenticationResult authenticationResult,
            bool skipOptionalSteps,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityOptions> identityOptions,
            IEnumerable<ITokenRequestValidator> requestValidators,
            IEnumerable<ITokenClaimProvider> claimProviders,
            IEnumerable<ITokenRequestHandler> requestHandlers,
            IEventPublisher eventPublisher)
            : base(signInManager, identityOptions, requestValidators, claimProviders, requestHandlers, eventPublisher)
        {
            _authenticationResult = authenticationResult;
            _skipOptionalSteps = skipOptionalSteps;
        }

        public override string GrantType => _grantType;

        protected override Task<GrantAuthenticationResult> AuthenticateAsync(TokenRequestContext context)
        {
            return Task.FromResult(_authenticationResult);
        }

        // Simulates a grant (e.g. impersonation) that skips these steps entirely.
        protected override Task<bool> CanSignInAsync(ApplicationUser user)
        {
            return _skipOptionalSteps ? Task.FromResult(true) : base.CanSignInAsync(user);
        }

        protected override Task<TokenResponse> UpdateLastLoginAsync(ApplicationUser user)
        {
            return _skipOptionalSteps ? Task.FromResult<TokenResponse>(null) : base.UpdateLastLoginAsync(user);
        }

        protected override Task BeforeSignInAsync(ApplicationUser user, TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.CompletedTask : base.BeforeSignInAsync(user, context);
        }

        protected override Task AfterSignInAsync(ApplicationUser user, TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.CompletedTask : base.AfterSignInAsync(user, context);
        }
    }
}
