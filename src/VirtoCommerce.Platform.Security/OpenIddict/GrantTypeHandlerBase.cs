using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.SignInLog;
using VirtoCommerce.Platform.Security.Exceptions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using MvcSignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public abstract class GrantTypeHandlerBase : IGrantTypeHandler
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IdentityOptions _identityOptions;
    private readonly IEnumerable<ITokenRequestValidator> _requestValidators;
    private readonly IEnumerable<ITokenClaimProvider> _claimProviders;
    private readonly IEnumerable<ITokenRequestHandler> _requestHandlers;
    private readonly IEventPublisher _eventPublisher;

    protected GrantTypeHandlerBase(
        SignInManager<ApplicationUser> signInManager,
        IOptions<IdentityOptions> identityOptions,
        IEnumerable<ITokenRequestValidator> requestValidators,
        IEnumerable<ITokenClaimProvider> claimProviders,
        IEnumerable<ITokenRequestHandler> requestHandlers,
        IEventPublisher eventPublisher)
    {
        _signInManager = signInManager;
        _identityOptions = identityOptions.Value;
        _requestValidators = requestValidators.OrderByDescending(x => x.Priority).ThenBy(x => x.GetType().Name).ToList();
        _claimProviders = claimProviders;
        _requestHandlers = requestHandlers;
        _eventPublisher = eventPublisher;
    }

    public abstract string GrantType { get; }

    /// <summary>
    /// Sign-in type recorded in the sign-in log, at most 32 characters.
    /// </summary>
    protected abstract string SignInType { get; }

    public virtual async Task<ActionResult> HandleAsync(TokenRequestContext context)
    {
        context.DelayedResponse = DelayedResponse.Create(nameof(GrantTypeHandlerBase), nameof(HandleAsync), GrantType);

        context.GrantValidationResult = await ValidateGrantAsync(context);
        context.User = context.GrantValidationResult.User;

        if (!context.GrantValidationResult.Success)
        {
            return await FailAsync(context, context.GrantValidationResult.Error, context.GrantValidationResult.ErrorResult);
        }

        if (!await CanSignInAsync(context))
        {
            return await FailAsync(context, SecurityErrorDescriber.SignInNotAllowed());
        }

        var validationError = await ValidateRequestAsync(context);
        if (validationError != null)
        {
            return await FailAsync(context, validationError);
        }

        await BeforeSignInAsync(context);
        await CallRequestHandlersAsync(context);

        var ticket = await CreateTicketAsync(context);

        var lastLoginError = await UpdateLastLoginAsync(context);
        if (lastLoginError != null)
        {
            return await FailAsync(context, lastLoginError);
        }

        await AfterSignInAsync(context);

        return await SucceedAsync(context, ticket);
    }

    /// <summary>
    /// Verifies the authorization grant in the token request and resolves the user it was issued for,
    /// or returns the error to report instead. On failure, it also sets <see cref="TokenRequestContext.FailureReason"/>.
    /// </summary>
    protected abstract Task<GrantValidationResult> ValidateGrantAsync(TokenRequestContext context);

    protected virtual async Task<bool> CanSignInAsync(TokenRequestContext context)
    {
        var canSignIn = await _signInManager.CanSignInAsync(context.User);
        if (!canSignIn)
        {
            context.FailureReason = SignInFailureReason.NotAllowed;
        }

        return canSignIn;
    }

    protected virtual async Task<TokenResponse> ValidateRequestAsync(TokenRequestContext context)
    {
        foreach (var requestValidator in _requestValidators)
        {
            var errors = await requestValidator.ValidateAsync(context);
            if (errors.Count > 0)
            {
                return errors.First();
            }
        }

        return null;
    }

    protected virtual Task BeforeSignInAsync(TokenRequestContext context)
    {
        return _eventPublisher.Publish(new BeforeUserLoginEvent(context.User));
    }

    protected virtual async Task CallRequestHandlersAsync(TokenRequestContext context)
    {
        foreach (var requestHandler in _requestHandlers)
        {
            await requestHandler.HandleAsync(context.User, context);
        }
    }

    protected virtual async Task<AuthenticationTicket> CreateTicketAsync(TokenRequestContext context)
    {
        var principal = await _signInManager.CreateUserPrincipalAsync(context.User);

        SetTicketScopes(context, principal);

        principal.SetResources("resource_server");

        SetClaimDestinations(principal);

        foreach (var claimProvider in _claimProviders)
        {
            await claimProvider.SetClaimsAsync(principal, context);
        }

        return new AuthenticationTicket(principal, context.Properties, context.AuthenticationScheme);
    }

    protected virtual void SetTicketScopes(TokenRequestContext context, ClaimsPrincipal principal)
    {
        principal.SetScopes(new[]
        {
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Profile,
            Scopes.OfflineAccess,
            Scopes.Roles,
        }.Intersect(context.Request.GetScopes()));
    }

    protected virtual void SetClaimDestinations(ClaimsPrincipal principal)
    {
        foreach (var claim in principal.Claims)
        {
            if (claim.Type == _identityOptions.ClaimsIdentity.SecurityStampClaimType)
            {
                continue;
            }

            var destinations = new List<string>
            {
                Destinations.AccessToken,
            };

            var requiredScope = claim.Type switch
            {
                Claims.Name => Scopes.Profile,
                Claims.Email => Scopes.Email,
                Claims.Role => Scopes.Roles,
                _ => null,
            };

            if (requiredScope != null && principal.HasScope(requiredScope))
            {
                destinations.Add(Destinations.IdentityToken);
            }

            claim.SetDestinations(destinations);
        }
    }

    protected virtual async Task<TokenResponse> UpdateLastLoginAsync(TokenRequestContext context)
    {
        context.User.LastLoginDate = DateTime.UtcNow;

        try
        {
            await _signInManager.UserManager.UpdateAsync(context.User);
        }
        catch (DuplicateEmailException)
        {
            context.FailureReason = SignInFailureReason.DuplicateEmail;
            return SecurityErrorDescriber.DuplicateEmailLoginAttempt();
        }

        return null;
    }

    protected virtual Task AfterSignInAsync(TokenRequestContext context)
    {
        return _eventPublisher.Publish(new UserLoginEvent(context.User));
    }

    protected virtual async Task<ActionResult> SucceedAsync(TokenRequestContext context, AuthenticationTicket ticket)
    {
        await PublishSignInAttemptAsync(context, succeeded: true);
        await context.DelayedResponse.SucceedAsync();

        return new MvcSignInResult(context.AuthenticationScheme, ticket.Principal, ticket.Properties);
    }

    protected virtual async Task<ActionResult> FailAsync(TokenRequestContext context, TokenResponse error = null, ActionResult errorResult = null)
    {
        await PublishSignInAttemptAsync(context, succeeded: false);
        await context.DelayedResponse.FailAsync();

        return errorResult ?? new BadRequestObjectResult(error);
    }

    protected virtual Task PublishSignInAttemptAsync(TokenRequestContext context, bool succeeded)
    {
        return _eventPublisher.Publish(BuildSignInAttemptEvent(context, succeeded));
    }

    protected virtual UserSignInAttemptEvent BuildSignInAttemptEvent(TokenRequestContext context, bool succeeded)
    {
        var result = AbstractTypeFactory<UserSignInAttemptEvent>.TryCreateInstance();

        result.UserName = context.Request.Username ?? context.User?.UserName;
        result.UserId = context.User?.Id;
        result.Succeeded = succeeded;
        result.FailureReason = succeeded ? null : context.FailureReason ?? SignInFailureReason.Unknown;
        result.SignInType = SignInType;
        result.ClientId = context.Request.ClientId;
        result.StoreId = context.User?.StoreId;
        result.MemberId = context.User?.MemberId;

        return result;
    }
}
