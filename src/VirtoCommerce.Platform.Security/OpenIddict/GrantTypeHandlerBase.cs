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

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    /// <summary>
    /// Base class for an <see cref="IGrantTypeHandler"/> that authenticates a user and signs them in.
    /// Every step past <see cref="AuthenticateAsync"/> is a separate <c>protected virtual</c> method, so a
    /// grant that needs to skip or change one - e.g. impersonation skipping <see cref="CanSignInAsync"/>, or
    /// a refresh reusing scopes via <see cref="SetTicketScopes"/> - overrides just that step.
    /// </summary>
    public abstract class GrantTypeHandlerBase : IGrantTypeHandler
    {
        protected SignInManager<ApplicationUser> SignInManager { get; }
        protected IdentityOptions IdentityOptions { get; }
        protected IEventPublisher EventPublisher { get; }

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
            SignInManager = signInManager;
            IdentityOptions = identityOptions.Value;
            _requestValidators = requestValidators;
            _claimProviders = claimProviders;
            _requestHandlers = requestHandlers;
            EventPublisher = eventPublisher;
        }

        public abstract string GrantType { get; }

        public virtual async Task<ActionResult> HandleAsync(TokenRequestContext context)
        {
            var authenticationResult = await AuthenticateAsync(context);
            if (!authenticationResult.Success)
            {
                return new BadRequestObjectResult(authenticationResult.Error);
            }

            var user = authenticationResult.User;

            if (!await CanSignInAsync(user))
            {
                return new BadRequestObjectResult(SecurityErrorDescriber.SignInNotAllowed());
            }

            context.User = user.CloneTyped();

            var validationError = await ValidateRequestAsync(context);
            if (validationError != null)
            {
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
                return new BadRequestObjectResult(lastLoginError);
            }

            await AfterSignInAsync(user, context);

            return new MvcSignInResult(context.AuthenticationScheme, ticket.Principal, ticket.Properties);
        }

        /// <summary>
        /// Verifies the grant-specific credential and resolves the user it belongs to.
        /// </summary>
        protected abstract Task<GrantAuthenticationResult> AuthenticateAsync(TokenRequestContext context);

        protected virtual Task<bool> CanSignInAsync(ApplicationUser user)
        {
            return SignInManager.CanSignInAsync(user);
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
            return EventPublisher.Publish(new BeforeUserLoginEvent(user));
        }

        protected virtual async Task<AuthenticationTicket> CreateTicketAsync(ApplicationUser user, TokenRequestContext context)
        {
            var principal = await SignInManager.CreateUserPrincipalAsync(user);

            SetTicketScopes(principal, context);

            principal.SetResources("resource_server");

            SetClaimDestinations(principal);

            foreach (var claimProvider in _claimProviders)
            {
                await claimProvider.SetClaimsAsync(principal, context);
            }

            return new AuthenticationTicket(principal, context.Properties, context.AuthenticationScheme);
        }

        /// <summary>
        /// Sets the ticket's scopes. Override to reuse an existing authorization's scopes (refresh,
        /// authorization code) instead of recomputing them from the request.
        /// </summary>
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
                if (claim.Type == IdentityOptions.ClaimsIdentity.SecurityStampClaimType)
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

        /// <summary>
        /// Adds anything grant-specific to the ticket. Defaults to setting the authentication-method claim
        /// to <see cref="GrantType"/> - override to add claims like an impersonation grant's operator id/name.
        /// </summary>
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
                await SignInManager.UserManager.UpdateAsync(user);
            }
            catch (DuplicateEmailException)
            {
                return SecurityErrorDescriber.DuplicateEmailLoginAttempt();
            }

            return null;
        }

        protected virtual Task AfterSignInAsync(ApplicationUser user, TokenRequestContext context)
        {
            return EventPublisher.Publish(new UserLoginEvent(user));
        }
    }
}
