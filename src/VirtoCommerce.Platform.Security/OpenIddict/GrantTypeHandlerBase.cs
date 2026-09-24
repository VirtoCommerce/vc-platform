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
using VirtoCommerce.Platform.Security.Exceptions;
using VirtoCommerce.Platform.Security.Extensions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using MvcSignInResult = Microsoft.AspNetCore.Mvc.SignInResult;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public abstract class GrantTypeHandlerBase : IGrantTypeHandler
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IdentityOptions _identityOptions;
    private readonly IEventPublisher _eventPublisher;

    private readonly IEnumerable<ITokenRequestValidator> _requestValidators;
    private readonly IEnumerable<ITokenClaimProvider> _claimProviders;
    private readonly IEnumerable<ITokenRequestHandler> _requestHandlers;

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
        _requestValidators = requestValidators;
        _claimProviders = claimProviders;
        _requestHandlers = requestHandlers;
        _eventPublisher = eventPublisher;
    }

    public abstract string GrantType { get; }

    public virtual async Task<ActionResult> HandleAsync(TokenRequestContext context)
    {
        var delayedResponse = DelayedResponse.Create(nameof(GrantTypeHandlerBase), nameof(HandleAsync), GrantType);

        var validationResult = await ValidateGrantAsync(context);
        if (!validationResult.Success)
        {
            await delayedResponse.FailAsync();
            return new BadRequestObjectResult(validationResult.Error);
        }

        var user = validationResult.User;

        if (!await CanSignInAsync(user))
        {
            await delayedResponse.FailAsync();
            return new BadRequestObjectResult(SecurityErrorDescriber.SignInNotAllowed());
        }

        context.User = user.CloneTyped();

        var validationError = await ValidateRequestAsync(context);
        if (validationError != null)
        {
            await delayedResponse.FailAsync();
            return new BadRequestObjectResult(validationError);
        }

        await BeforeSignInAsync(user, context);

        foreach (var requestHandler in _requestHandlers)
        {
            await requestHandler.HandleAsync(user, context);
        }

        var ticket = await CreateTicketAsync(user, context);
        await EnrichTicketAsync(ticket, user, context);

        var lastLoginError = await UpdateLastLoginAsync(user);
        if (lastLoginError != null)
        {
            await delayedResponse.FailAsync();
            return new BadRequestObjectResult(lastLoginError);
        }

        await AfterSignInAsync(user, context);

        await delayedResponse.SucceedAsync();

        return new MvcSignInResult(context.AuthenticationScheme, ticket.Principal, ticket.Properties);
    }

    /// <summary>
    /// Verifies the authorization grant in the token request and resolves the user it was issued for,
    /// or returns the error to report instead.
    /// </summary>
    protected abstract Task<GrantValidationResult> ValidateGrantAsync(TokenRequestContext context);

    protected virtual Task<bool> CanSignInAsync(ApplicationUser user)
    {
        return _signInManager.CanSignInAsync(user);
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

    protected virtual Task BeforeSignInAsync(ApplicationUser user, TokenRequestContext context)
    {
        return _eventPublisher.Publish(new BeforeUserLoginEvent(user));
    }

    protected virtual async Task<AuthenticationTicket> CreateTicketAsync(ApplicationUser user, TokenRequestContext context)
    {
        var principal = await _signInManager.CreateUserPrincipalAsync(user);

        SetTicketScopes(principal, context);

        principal.SetResources("resource_server");

        SetClaimDestinations(principal);

        foreach (var claimProvider in _claimProviders)
        {
            await claimProvider.SetClaimsAsync(principal, context);
        }

        return new AuthenticationTicket(principal, context.Properties, context.AuthenticationScheme);
    }

    protected virtual void SetTicketScopes(ClaimsPrincipal principal, TokenRequestContext context)
    {
        principal.SetScopes(new[]
        {
            Scopes.OpenId,
            Scopes.Email,
            Scopes.Profile,
            Scopes.OfflineAccess,
            Scopes.Roles
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
                Destinations.AccessToken
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

    protected virtual Task EnrichTicketAsync(AuthenticationTicket ticket, ApplicationUser user, TokenRequestContext context)
    {
        ticket.Principal.SetAuthenticationMethod(GrantType, [Destinations.AccessToken]);

        return Task.CompletedTask;
    }

    protected virtual async Task<TokenResponse> UpdateLastLoginAsync(ApplicationUser user)
    {
        user.LastLoginDate = DateTime.UtcNow;

        try
        {
            await _signInManager.UserManager.UpdateAsync(user);
        }
        catch (DuplicateEmailException)
        {
            return SecurityErrorDescriber.DuplicateEmailLoginAttempt();
        }

        return null;
    }

    protected virtual Task AfterSignInAsync(ApplicationUser user, TokenRequestContext context)
    {
        return _eventPublisher.Publish(new UserLoginEvent(user));
    }
}
