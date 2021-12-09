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
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Mvc.Server
{
    public class AuthorizationController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> _applicationManager;
        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly AuthorizationOptions _authorizationOptions;
        private readonly IEventPublisher _eventPublisher;

        public AuthorizationController(
            OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication> applicationManager,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IOptions<AuthorizationOptions> authorizationOptions,
            IEventPublisher eventPublisher)
        {
            _applicationManager = applicationManager;
            _identityOptions = identityOptions;
            _signInManager = signInManager;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationOptions = authorizationOptions.Value;
            _eventPublisher = eventPublisher;
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
                var user = await _userManager.FindByNameAsync(openIdConnectRequest.Username);

                if (user == null)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Validate the username/password parameters and ensure the account is not locked out.
                var result = await _signInManager.CheckPasswordSignInAsync(user, openIdConnectRequest.Password, lockoutOnFailure: true);
                if (!result.Succeeded)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(openIdConnectRequest, user);
                var claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

                var limitedPermissions = _authorizationOptions.LimitedCookiePermissions?.Split(PlatformConstants.Security.Claims.PermissionClaimTypeDelimiter, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

                if (!user.IsAdministrator)
                {
                    limitedPermissions = claimsPrincipal
                        .Claims
                        .Where(c => c.Type == PlatformConstants.Security.Claims.PermissionClaimType)
                        .Select(c => c.Value)
                        .Intersect(limitedPermissions, StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                }

                if (limitedPermissions.Any())
                {
                    // Set limited permissions and authenticate user with combined mode Cookies + Bearer.
                    //
                    // LimitedPermissions claims that will be granted to the user by cookies when bearer token authentication is enabled.
                    // This can help to authorize the user for direct(non - AJAX) GET requests to the VC platform API and / or to use some 3rd - party web applications for the VC platform(like Hangfire dashboard).
                    //
                    // If the user identity has claim named "limited_permissions", this attribute should authorize only permissions listed in that claim. Any permissions that are required by this attribute but
                    // not listed in the claim should cause this method to return false. However, if permission limits of user identity are not defined ("limited_permissions" claim is missing),
                    // then no limitations should be applied to the permissions.
                    ((ClaimsIdentity)claimsPrincipal.Identity).AddClaim(new Claim(PlatformConstants.Security.Claims.LimitedPermissionsClaimType, string.Join(PlatformConstants.Security.Claims.PermissionClaimTypeDelimiter, limitedPermissions)));
                    await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);
                }

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
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The token is no longer valid."
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored in the
                // authorization code/refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(openIdConnectRequest, user, info.Properties);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            else if (openIdConnectRequest.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.
                var application = await _applicationManager.FindByClientIdAsync(openIdConnectRequest.ClientId, HttpContext.RequestAborted);
                if (application == null)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidClient,
                        ErrorDescription = "The client application was not found in the database."
                    });
                }

                // Create a new authentication ticket.
                var ticket = CreateTicket(application);
                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
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
            identity.AddClaim(Claims.Subject, application.ClientId,
                Destinations.AccessToken,
                Destinations.IdentityToken);

            identity.AddClaim(Claims.Name, application.DisplayName,
                Destinations.AccessToken,
                Destinations.IdentityToken);

            // all clients act as administrator
            identity.AddClaim(
                Claims.Role,
                PlatformConstants.Security.SystemRoles.Administrator,
                Destinations.AccessToken,
                Destinations.IdentityToken);

            var principal = new ClaimsPrincipal(identity);

            principal.SetResources("resource_server");

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
                if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
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
    }
}
