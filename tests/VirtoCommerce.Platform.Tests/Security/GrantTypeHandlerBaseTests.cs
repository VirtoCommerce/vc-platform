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
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Security.OpenIddict;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace VirtoCommerce.Platform.Tests.Security;

public class GrantTypeHandlerBaseTests
{
    private const string _grantType = "test_grant";

    [Fact]
    public async Task HandleAsync_Should_ReturnBadRequest_When_AuthenticationFails()
    {
        var authError = new TokenResponse { Code = "bad_credential" };
        var context = CreateContext(validationResult: GrantValidationResult.Fail(authError), failureReason: SignInFailureReason.InvalidPassword);

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().BeSameAs(authError);
        context.SignInManager.Verify(x => x.CanSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        VerifySignInAttempt(context, succeeded: false, SignInFailureReason.InvalidPassword);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnTheGrantErrorResult_When_GrantFailsWithAnActionResult()
    {
        var errorResult = new ForbidResult();
        var context = CreateContext(validationResult: GrantValidationResult.Fail(errorResult), failureReason: SignInFailureReason.Forbidden);

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        actionResult.Should().BeSameAs(errorResult);
        VerifySignInAttempt(context, succeeded: false, SignInFailureReason.Forbidden);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnBadRequest_When_UserCannotSignIn()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(validationResult: GrantValidationResult.Succeed(user));
        context.SignInManager.Setup(x => x.CanSignInAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(false);

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        ((TokenResponse)badRequest.Value).Code.Should().Be("sign_in_not_allowed");
        context.SignInManager.Verify(x => x.CreateUserPrincipalAsync(It.IsAny<ApplicationUser>()), Times.Never);
        VerifySignInAttempt(context, succeeded: false, SignInFailureReason.NotAllowed);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnBadRequest_When_ARequestValidatorRejects()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var validatorError = new TokenResponse { Code = "custom_error" };
        var validator = new Mock<ITokenRequestValidator>();
        validator.Setup(x => x.ValidateAsync(It.IsAny<TokenRequestContext>()))
            .Callback<TokenRequestContext>(x => x.FailureReason = SignInFailureReason.LockedOut)
            .ReturnsAsync((IList<TokenResponse>)[validatorError]);

        var context = CreateContext(
            validationResult: GrantValidationResult.Succeed(user),
            requestValidators: [validator.Object]);
        context.SignInManager.Setup(x => x.CanSignInAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(true);

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().BeSameAs(validatorError);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Never);
        VerifySignInAttempt(context, succeeded: false, SignInFailureReason.LockedOut);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnTheErrorOfTheHighestPriorityValidator()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var lowPriorityError = new TokenResponse { Code = "low_priority_error" };
        var highPriorityError = new TokenResponse { Code = "high_priority_error" };

        var context = CreateContext(
            validationResult: GrantValidationResult.Succeed(user),
            requestValidators: [CreateRejectingValidator(1, lowPriorityError), CreateRejectingValidator(10, highPriorityError)]);

        context.SignInManager.Setup(x => x.CanSignInAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(true);

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequest.Value.Should().BeSameAs(highPriorityError);
    }

    [Fact]
    public async Task HandleAsync_Should_ReturnBadRequest_When_UpdatingLastLoginDateThrows()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(validationResult: GrantValidationResult.Succeed(user));
        context.SignInManager.Setup(x => x.CanSignInAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));
        context.UserManager.Setup(x => x.UpdateAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ThrowsAsync(new DuplicateEmailException("duplicate"));

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var badRequest = actionResult.Should().BeOfType<BadRequestObjectResult>().Subject;
        ((TokenResponse)badRequest.Value).Code.Should().Be("duplicate_email_login_attempt");
        VerifySignInAttempt(context, succeeded: false, SignInFailureReason.DuplicateEmail);
    }

    [Fact]
    public async Task HandleAsync_Should_SignIn_And_RunTheFullPipeline_When_AuthenticationSucceeds()
    {
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var claimProvider = new Mock<ITokenClaimProvider>();
        var requestHandler = new Mock<ITokenRequestHandler>();

        var context = CreateContext(
            validationResult: GrantValidationResult.Succeed(user),
            claimProviders: [claimProvider.Object],
            requestHandlers: [requestHandler.Object]);
        context.SignInManager.Setup(x => x.CanSignInAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        var signInResult = actionResult.Should().BeOfType<SignInResult>().Subject;
        signInResult.Principal.Should().NotBeNull();
        signInResult.Principal.FindFirst(ClaimTypes.AuthenticationMethod).Should().BeNull();
        context.RequestContext.User.LastLoginDate.Should().NotBeNull();
        context.UserManager.Verify(x => x.UpdateAsync(It.Is<ApplicationUser>(u => u.Id == user.Id)), Times.Once);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Once);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<UserLoginEvent>()), Times.Once);
        requestHandler.Verify(x => x.HandleAsync(It.Is<ApplicationUser>(u => u.Id == user.Id), context.RequestContext), Times.Once);
        claimProvider.Verify(x => x.SetClaimsAsync(It.IsAny<ClaimsPrincipal>(), context.RequestContext), Times.Once);
        VerifySignInAttempt(context, succeeded: true, failureReason: null);
    }

    [Fact]
    public async Task HandleAsync_Should_HonorOverridesThatSkipSignInCheckAndLastLoginUpdate()
    {
        // Proves CanSignInAsync/LastLoginDate/Before-AfterSignIn are each independently skippable.
        var user = new ApplicationUser { Email = "buyer@acme.com" };
        var context = CreateContext(GrantValidationResult.Succeed(user), skipOptionalSteps: true);
        context.SignInManager.Object.UserManager = context.UserManager.Object;
        context.SignInManager.Setup(x => x.CreateUserPrincipalAsync(It.Is<ApplicationUser>(u => u.Id == user.Id))).ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

        var actionResult = await context.Handler.HandleAsync(context.RequestContext);

        actionResult.Should().BeOfType<SignInResult>();
        context.SignInManager.Verify(x => x.CanSignInAsync(It.IsAny<ApplicationUser>()), Times.Never);
        context.UserManager.Verify(x => x.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        context.RequestContext.User.LastLoginDate.Should().BeNull();
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<BeforeUserLoginEvent>()), Times.Never);
        context.EventPublisher.Verify(x => x.Publish(It.IsAny<UserLoginEvent>()), Times.Never);
    }

    private static void VerifySignInAttempt(TestContext context, bool succeeded, string failureReason)
    {
        context.EventPublisher.Verify(x => x.Publish(It.Is<UserSignInAttemptEvent>(e =>
            e.Succeeded == succeeded &&
            e.FailureReason == failureReason &&
            e.SignInType == _grantType)), Times.Once);
    }

    private static ITokenRequestValidator CreateRejectingValidator(int priority, TokenResponse error)
    {
        var validator = new Mock<ITokenRequestValidator>();
        validator.SetupGet(x => x.Priority).Returns(priority);
        validator.Setup(x => x.ValidateAsync(It.IsAny<TokenRequestContext>())).ReturnsAsync((IList<TokenResponse>)[error]);

        return validator.Object;
    }

    private static TestContext CreateContext(
        GrantValidationResult validationResult,
        IEnumerable<ITokenRequestValidator> requestValidators = null,
        IEnumerable<ITokenClaimProvider> claimProviders = null,
        IEnumerable<ITokenRequestHandler> requestHandlers = null,
        bool skipOptionalSteps = false,
        string failureReason = null)
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
            validationResult,
            failureReason,
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
            Request = new OpenIddictRequest { GrantType = _grantType },
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

    private sealed class TestGrantHandler : GrantTypeHandlerBase
    {
        private readonly GrantValidationResult _validationResult;
        private readonly string _failureReason;
        private readonly bool _skipOptionalSteps;

        public TestGrantHandler(
            GrantValidationResult validationResult,
            string failureReason,
            bool skipOptionalSteps,
            SignInManager<ApplicationUser> signInManager,
            IOptions<IdentityOptions> identityOptions,
            IEnumerable<ITokenRequestValidator> requestValidators,
            IEnumerable<ITokenClaimProvider> claimProviders,
            IEnumerable<ITokenRequestHandler> requestHandlers,
            IEventPublisher eventPublisher)
            : base(signInManager, identityOptions, requestValidators, claimProviders, requestHandlers, eventPublisher)
        {
            _validationResult = validationResult;
            _failureReason = failureReason;
            _skipOptionalSteps = skipOptionalSteps;
        }

        public override string GrantType => _grantType;

        protected override string SignInType => _grantType;

        protected override Task<GrantValidationResult> ValidateGrantAsync(TokenRequestContext context)
        {
            context.FailureReason = _failureReason;

            return Task.FromResult(_validationResult);
        }

        // Simulates a grant (e.g. impersonation) that skips these steps entirely.
        protected override Task<bool> CanSignInAsync(TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.FromResult(true) : base.CanSignInAsync(context);
        }

        protected override Task<TokenResponse> UpdateLastLoginAsync(TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.FromResult<TokenResponse>(null) : base.UpdateLastLoginAsync(context);
        }

        protected override Task BeforeSignInAsync(TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.CompletedTask : base.BeforeSignInAsync(context);
        }

        protected override Task AfterSignInAsync(TokenRequestContext context)
        {
            return _skipOptionalSteps ? Task.CompletedTask : base.AfterSignInAsync(context);
        }
    }
}
