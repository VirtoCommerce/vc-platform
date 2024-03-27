using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Security;
using VirtoCommerce.Platform.Security.Model;
using VirtoCommerce.Platform.Security.Services;
using VirtoCommerce.Platform.Web.Model.Security;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Mvc.Server
{
    public class AuthorizationController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> _applicationManager;
        private readonly IdentityOptions _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PasswordLoginOptions _passwordLoginOptions;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEnumerable<IUserSignInValidator> _userSignInValidators;
        private readonly OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> _tokenManager;

        private UserManager<ApplicationUser> UserManager => _signInManager.UserManager;

        public AuthorizationController(
            OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> applicationManager,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptions<PasswordLoginOptions> passwordLoginOptions,
            IEventPublisher eventPublisher,
            IEnumerable<IUserSignInValidator> userSignInValidators,
            OpenIddictTokenManager<OpenIddictEntityFrameworkCoreToken> tokenManager)
        {
            _applicationManager = applicationManager;
            _identityOptions = identityOptions.Value;
            _passwordLoginOptions = passwordLoginOptions.Value ?? new PasswordLoginOptions();
            _signInManager = signInManager;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _userSignInValidators = userSignInValidators;
            _tokenManager = tokenManager;
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
            OpenIddictRequest openIdConnectRequest = HttpContext.GetOpenIddictServerRequest();

            if (openIdConnectRequest.IsPasswordGrantType())
            {
                var userName = openIdConnectRequest.Username;

                // Allows signin to back office by either username (login) or email if IdentityOptions.User.RequireUniqueEmail is True. 
                if (_identityOptions.User.RequireUniqueEmail)
                {
                    var userByName = await UserManager.FindByNameAsync(userName);

                    if (userByName == null)
                    {
                        var userByEmail = await UserManager.FindByEmailAsync(userName);
                        if (userByEmail != null)
                        {
                            userName = userByEmail.UserName;
                        }
                    }
                }

                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return BadRequest(SecurityErrorDescriber.LoginFailed());
                }

                if (!_passwordLoginOptions.Enabled && !user.IsAdministrator)
                {
                    return BadRequest(SecurityErrorDescriber.PasswordLoginDisabled());
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, openIdConnectRequest.Password, lockoutOnFailure: true);

                var context = new SignInValidatorContext
                {
                    User = user.Clone() as ApplicationUser,
                    DetailedErrors = _passwordLoginOptions.DetailedErrors,
                    IsSucceeded = result.Succeeded,
                    IsLockedOut = result.IsLockedOut,
                };

                var storeIdParameter = openIdConnectRequest.GetParameter("storeId");
                if (storeIdParameter != null)
                {
                    context.StoreId = (string)storeIdParameter.GetValueOrDefault();
                }

                foreach (var loginValidation in _userSignInValidators.OrderByDescending(x => x.Priority).ThenBy(x => x.GetType().Name))
                {
                    var validationErrors = await loginValidation.ValidateUserAsync(context);
                    var error = validationErrors.FirstOrDefault();
                    if (error != null)
                    {
                        return BadRequest(error);
                    }
                }

                await _eventPublisher.Publish(new BeforeUserLoginEvent(user));

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(openIdConnectRequest, user);

                await SetLastLoginDate(user);
                await _eventPublisher.Publish(new UserLoginEvent(user));

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (openIdConnectRequest.IsRefreshTokenGrantType())
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

                // Create a new authentication ticket, but reuse the properties stored in the
                // authorization code/refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(openIdConnectRequest, user, info.Properties);
                return SignIn(ticket.Principal, ticket.AuthenticationScheme);
            }
            else if (openIdConnectRequest.IsClientCredentialsGrantType())
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

            identity.SetDestinations(static claim => new[] { Destinations.AccessToken, Destinations.IdentityToken });

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                principal,
                new AuthenticationProperties(),
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return ticket;
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIddictRequest request, ApplicationUser user, AuthenticationProperties properties = null)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
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
                }.Intersect(request.GetScopes()));
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

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(
                principal,
                properties,
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            return ticket;
        }

        private Task SetLastLoginDate(ApplicationUser user)
        {
            user.LastLoginDate = DateTime.UtcNow;
            return _signInManager.UserManager.UpdateAsync(user);
        }
    }
}
