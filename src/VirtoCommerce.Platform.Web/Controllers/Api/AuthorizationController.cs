using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Security.ExternalSignIn;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Security.Extensions;
using VirtoCommerce.Platform.Security.OpenIddict;
using VirtoCommerce.Platform.Web.Extensions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    public class AuthorizationController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> _applicationManager;
        private readonly IdentityOptions _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordLoginOptions _passwordLoginOptions;
        private readonly IEventPublisher _eventPublisher;
        private readonly List<ITokenRequestValidator> _requestValidators;
        private readonly IEnumerable<ITokenClaimProvider> _claimProviders;
        private readonly OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> _tokenManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IExternalSignInService _externalSignInService;

        public AuthorizationController(
            OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> applicationManager,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            IOptions<PasswordLoginOptions> passwordLoginOptions,
            IEventPublisher eventPublisher,
            IEnumerable<ITokenRequestValidator> requestValidators,
            IEnumerable<ITokenClaimProvider> claimProviders,
            OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> tokenManager,
            IAuthorizationService authorizationService,
            IExternalSignInService externalSignInService)
        {
            _applicationManager = applicationManager;
            _identityOptions = identityOptions.Value;
            _passwordLoginOptions = passwordLoginOptions.Value;
            _signInManager = signInManager;
            _userManager = _signInManager.UserManager;
            _eventPublisher = eventPublisher;
            _requestValidators = requestValidators.OrderByDescending(x => x.Priority).ThenBy(x => x.GetType().Name).ToList();
            _claimProviders = claimProviders;
            _tokenManager = tokenManager;
            _authorizationService = authorizationService;
            _externalSignInService = externalSignInService;
        }

        [HttpPost("~/revoke/token")]
        public async Task<ActionResult> RevokeCurrentUserToken()
        {
            var tokenId = HttpContext.User.GetClaim("oi_tkn_id");
            var authId = HttpContext.User.GetClaim("oi_au_id");

            if (authId != null)
            {
                var tokens = _tokenManager.FindByAuthorizationIdAsync(authId);
                await foreach (var token in tokens)
                {
                    await _tokenManager.TryRevokeAsync(token);
                }
            }
            else if (tokenId != null)
            {
                var token = await _tokenManager.FindByIdAsync(tokenId);
                if (token?.Authorization != null)
                {
                    foreach (var authorizationToken in token.Authorization.Tokens)
                    {
                        await _tokenManager.TryRevokeAsync(authorizationToken);
                    }
                }
            }

            return Ok();
        }

        #region Password, authorization code and refresh token flows
        // Note: to support non-interactive flows like password,
        // you must provide your own token endpoint action:

        [HttpPost("~/connect/token"), Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenIddictResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(OpenIddictResponse))]
        // Be aware: look into OpenIDEndpointDescriptionFilter to know parameters description for the swagger document about this endpoint
        public async Task<ActionResult> Exchange()
        {
            var openIdConnectRequest = HttpContext.GetOpenIddictServerRequest();

            var context = new TokenRequestContext
            {
                AuthenticationScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Request = openIdConnectRequest,
                DetailedErrors = _passwordLoginOptions.DetailedErrors,
            };

            if (openIdConnectRequest.IsPasswordGrantType())
            {
                // Measure the duration of a succeeded response and delay subsequent failed responses to prevent timing attacks
                var delayedResponse = DelayedResponse.Create(nameof(AuthorizationController), nameof(Exchange), "Password");

                var user = await _userManager.FindByNameAsync(openIdConnectRequest.Username);

                // Allows signin to back office by either username (login) or email if IdentityOptions.User.RequireUniqueEmail is True. 
                if (user is null && _identityOptions.User.RequireUniqueEmail)
                {
                    user = await _userManager.FindByEmailAsync(openIdConnectRequest.Username);
                }

                if (user is null)
                {
                    await delayedResponse.FailAsync();
                    return BadRequest(SecurityErrorDescriber.LoginFailed());
                }

                if (!_passwordLoginOptions.Enabled && !user.IsAdministrator)
                {
                    await delayedResponse.FailAsync();
                    return BadRequest(SecurityErrorDescriber.PasswordLoginDisabled());
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                context.SignInResult = await _signInManager.CheckPasswordSignInAsync(user, openIdConnectRequest.Password, lockoutOnFailure: true);
                context.User = user.CloneTyped();

                foreach (var requestValidator in _requestValidators)
                {
                    var errors = await requestValidator.ValidateAsync(context);
                    if (errors.Count > 0)
                    {
                        await delayedResponse.FailAsync();
                        return BadRequest(errors.First());
                    }
                }

                await _eventPublisher.Publish(new BeforeUserLoginEvent(user));

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(user, context);

                await SetLastLoginDate(user);
                await _eventPublisher.Publish(new UserLoginEvent(user));

                await delayedResponse.SucceedAsync();

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            if (openIdConnectRequest.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the authorization code/refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the authorization code/refresh token.
                // Note: if you want to automatically invalidate the authorization code/refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    return BadRequest(SecurityErrorDescriber.TokenInvalid());
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(SecurityErrorDescriber.SignInNotAllowed());
                }

                context.User = user;
                context.Principal = info.Principal;
                context.Properties = info.Properties;

                foreach (var requestValidator in _requestValidators)
                {
                    var errors = await requestValidator.ValidateAsync(context);
                    if (errors.Count > 0)
                    {
                        return BadRequest(errors.First());
                    }
                }

                // Create a new authentication ticket, but reuse the properties stored in the
                // authorization code/refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(user, context);
                ticket.Principal.SetAuthenticationMethod(info.Principal.GetAuthenticationMethod(), [Destinations.AccessToken]);

                return SignIn(ticket.Principal, ticket.AuthenticationScheme);
            }

            if (openIdConnectRequest.GrantType == PlatformConstants.Security.GrantTypes.ExternalSignIn)
            {
                var signInResult = await _externalSignInService.SignInAsync();

                // Remove identity cookies regardless of the result
                await _signInManager.SignOutAsync();

                if (!signInResult.Success)
                {
                    return BadRequest(SecurityErrorDescriber.LoginFailed());
                }

                if (!await _signInManager.CanSignInAsync(signInResult.User))
                {
                    return BadRequest(SecurityErrorDescriber.SignInNotAllowed());
                }

                context.User = signInResult.User.CloneTyped();

                foreach (var requestValidator in _requestValidators)
                {
                    var errors = await requestValidator.ValidateAsync(context);
                    if (errors.Count > 0)
                    {
                        return BadRequest(errors.First());
                    }
                }

                var ticket = await CreateTicketAsync(signInResult.User, context);
                ticket.Principal.SetAuthenticationMethod(signInResult.LoginProvider, [Destinations.AccessToken]);

                return SignIn(ticket.Principal, ticket.AuthenticationScheme);
            }

            if (openIdConnectRequest.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.
                var application = await _applicationManager.FindByClientIdAsync(openIdConnectRequest.ClientId, HttpContext.RequestAborted);
                if (application == null)
                {
                    return BadRequest(SecurityErrorDescriber.InvalidClient());
                }

                // Create a new authentication ticket.
                var ticket = CreateTicket(application);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            if (openIdConnectRequest.IsImpersonateGrantType())
            {
                // Only Authorized User has access for impersonation
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Check if user has permission for login on behalf
                if (string.IsNullOrEmpty(User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserId)))
                {
                    var loginOnBehalfAuthResult = await _authorizationService.AuthorizeAsync(User, null,
                        new PermissionAuthorizationRequirement(PlatformConstants.Security.Permissions.SecurityLoginOnBehalf));
                    if (!loginOnBehalfAuthResult.Succeeded)
                    {
                        return Forbid();
                    }
                }

                // Resolve Impersonator from claims or from current user
                var operatorUserId = string.IsNullOrEmpty(User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserId)) ?
                    user.Id : User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserId);
                var operatorUserName = string.IsNullOrEmpty(User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserName)) ?
                    user.UserName : User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserName);

                var userId = openIdConnectRequest.GetParameter("user_id")?.Value?.ToString();
                ApplicationUser impersonatedUser;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Find impersonated user by id
                    impersonatedUser = await _userManager.FindByIdAsync(userId);
                }
                else
                {
                    // Reset impersonation to operator
                    impersonatedUser = await _userManager.FindByIdAsync(operatorUserId);
                    operatorUserId = string.Empty;
                    operatorUserName = string.Empty;
                }

                if (impersonatedUser == null)
                {
                    return BadRequest(SecurityErrorDescriber.TokenInvalid());
                }

                context.User = impersonatedUser.CloneTyped();

                foreach (var requestValidator in _requestValidators)
                {
                    var errors = await requestValidator.ValidateAsync(context);
                    if (errors.Count > 0)
                    {
                        return BadRequest(errors.First());
                    }
                }

                // Create a new authentication ticket, but reuse the properties stored in the
                // authorization code/refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(impersonatedUser, context);

                // Extend Token with custom claim for XAPI vc_xapi_impersonated_customerid
                var destinations = new[] { Destinations.AccessToken };
                ticket.Principal
                    .SetClaimWithDestinations(PlatformConstants.Security.Claims.OperatorUserId, operatorUserId, destinations)
                    .SetClaimWithDestinations(PlatformConstants.Security.Claims.OperatorUserName, operatorUserName, destinations);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(SecurityErrorDescriber.UnsupportedGrantType());
        }

        #endregion

        private AuthenticationTicket CreateTicket(OpenIddictEntityFrameworkCoreApplication application)
        {
            // Create a new ClaimsIdentity containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                Claims.Name,
                Claims.Role);


            // Use the client_id as the subject identifier.
            identity.SetClaim(Claims.Subject, application.ClientId);

            identity.SetClaim(Claims.Name, application.DisplayName);

            // all clients act as administrator
            identity.SetClaim(Claims.Role, PlatformConstants.Security.SystemRoles.Administrator);

            var principal = new ClaimsPrincipal(identity);

            principal.SetResources("resource_server");

            identity.SetDestinations(static _ => [Destinations.AccessToken, Destinations.IdentityToken]);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                principal,
                new AuthenticationProperties(),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return ticket;
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(ApplicationUser user, TokenRequestContext context)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            if (!context.Request.IsAuthorizationCodeGrantType() && !context.Request.IsRefreshTokenGrantType())
            {
                // Set the list of scopes granted to the client application.
                // Note: the offline_access scope must be granted
                // to allow OpenIddict to return a refresh token.
                principal.SetScopes(new[]
                {
                    Scopes.OpenId,
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                    Scopes.Roles
                }.Intersect(context.Request.GetScopes()));
            }

            principal.SetResources("resource_server");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            foreach (var claim in principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                if (claim.Type == _identityOptions.ClaimsIdentity.SecurityStampClaimType)
                {
                    continue;
                }

                var destinations = new List<string>
                {
                    Destinations.AccessToken
                };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                if (claim.Type == Claims.Name && principal.HasScope(Scopes.Profile) ||
                    claim.Type == Claims.Email && principal.HasScope(Scopes.Email) ||
                    claim.Type == Claims.Role && principal.HasScope(Scopes.Roles))
                {
                    destinations.Add(Destinations.IdentityToken);
                }

                claim.SetDestinations(destinations);
            }

            foreach (var claimProvider in _claimProviders)
            {
                await claimProvider.SetClaimsAsync(principal, context);
            }

            // Create a new authentication ticket holding the user identity.
            return new AuthenticationTicket(principal, context.Properties, context.AuthenticationScheme);
        }

        private Task<IdentityResult> SetLastLoginDate(ApplicationUser user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            return _userManager.UpdateAsync(user);
        }
    }
}
