using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Helpers;
using VirtoCommerce.Platform.Web.Model;
using VirtoCommerce.Platform.Web.Model.Security;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Mvc.Server
{
    public class AuthorizationController : Controller
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;

        private readonly IOptions<IdentityOptions> _identityOptions;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly AuthorizationOptions _authorizationOptions;
        private readonly PasswordLoginOptions _passwordLoginOptions;
        private readonly IEventPublisher _eventPublisher;

        public AuthorizationController(
            IOpenIddictApplicationManager applicationManager,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager,
            IOptions<IdentityOptions> identityOptions,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IOptions<AuthorizationOptions> authorizationOptions,
            IOptions<PasswordLoginOptions> passwordLoginOptions,
            IEventPublisher eventPublisher)
        {
            _applicationManager = applicationManager;
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
            _identityOptions = identityOptions;
            _passwordLoginOptions = passwordLoginOptions.Value ?? new PasswordLoginOptions();
            _signInManager = signInManager;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationOptions = authorizationOptions.Value;
            _eventPublisher = eventPublisher;
        }

        // GET: /api/userinfo
        [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
        public async Task<IActionResult> Userinfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The specified access token is bound to an account that no longer exists."
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await _userManager.GetUserIdAsync(user)
            };

            if (User.HasScope(Scopes.Email))
            {
                claims[Claims.Email] = await _userManager.GetEmailAsync(user);
                claims[Claims.EmailVerified] = await _userManager.IsEmailConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Phone))
            {
                claims[Claims.PhoneNumber] = await _userManager.GetPhoneNumberAsync(user);
                claims[Claims.PhoneNumberVerified] = await _userManager.IsPhoneNumberConfirmedAsync(user);
            }

            if (User.HasScope(Scopes.Roles))
            {
                claims[Claims.Role] = await _userManager.GetRolesAsync(user);
            }

            // Note: the complete list of standard claims supported by the OpenID Connect specification
            // can be found here: http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims

            return Ok(claims);
        }


        #region Password, authorization code and refresh token flows
        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
           throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // If prompt=login was specified by the client application,

            // Retrieve the user principal stored in the authentication cookie.
            // If a max_age parameter was provided, ensure that the cookie is not too old.
            // If the user principal can't be extracted or the cookie is too old, redirect the user to the login page.
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (result == null || !result.Succeeded || (request.MaxAge != null && result.Properties?.IssuedUtc != null &&
                DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value)))
            {
                // If the client application requested promptless authentication,
                // return an error indicating that the user is not logged in.
                if (request.HasPrompt(Prompts.None))
                {
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.LoginRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in."
                        }));
                }

                return Challenge(
                    authenticationSchemes: IdentityConstants.ApplicationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }


            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(result.Principal) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await _authorizationManager.FindAsync(
                subject: await _userManager.GetUserIdAsync(user),
                client: await _applicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();


            switch (await _applicationManager.GetConsentTypeAsync(application))
            {
                // If the consent is external (e.g when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found in the database.
                case ConsentTypes.External when !authorizations.Any():
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "The logged in user is not allowed to access this client application."
                        }));

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case ConsentTypes.Implicit:
                case ConsentTypes.External when authorizations.Any():
                case ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(Prompts.Consent):
                    var principal = await _signInManager.CreateUserPrincipalAsync(user);

                    // Note: in this sample, the granted scopes match the requested scope
                    // but you may want to allow the user to uncheck specific scopes.
                    // For that, simply restrict the list of scopes before calling SetScopes.
                    principal.SetScopes(request.GetScopes());
                    principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

                    // Automatically create a permanent authorization to avoid requiring explicit consent
                    // for future authorization or token requests containing the same scopes.
                    var authorization = authorizations.LastOrDefault();
                    if (authorization == null)
                    {
                        authorization = await _authorizationManager.CreateAsync(
                            principal: principal,
                            subject: await _userManager.GetUserIdAsync(user),
                            client: await _applicationManager.GetIdAsync(application),
                            type: AuthorizationTypes.Permanent,
                            scopes: principal.GetScopes());
                    }

                    principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

                    foreach (var claim in principal.Claims)
                    {
                        claim.SetDestinations(GetDestinations(claim, principal));
                    }

                    return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // At this point, no authorization was found in the database and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                                "Interactive user consent is required."
                        }));

                // In every other case, render the consent form.
                default:
                    return View(new AuthorizeViewModel
                    {
                        ApplicationName = await _applicationManager.GetDisplayNameAsync(application),
                        Scope = request.Scope
                    });           
            }
        }
        [Microsoft.AspNetCore.Authorization.Authorize, FormValueRequired("submit.Deny")]
        [HttpPost("~/connect/authorize")]
        // Notify OpenIddict that the authorization grant has been denied by the resource owner
        // to redirect the user agent to the client application using the appropriate response_mode.
        public IActionResult Deny() => Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        [Microsoft.AspNetCore.Authorization.Authorize, FormValueRequired("submit.Accept")]
        [HttpPost("~/connect/authorize"),]
        public async Task<IActionResult> Accept()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // Retrieve the profile of the logged in user.
            var user = await _userManager.GetUserAsync(User) ??
                throw new InvalidOperationException("The user details cannot be retrieved.");

            // Retrieve the application details from the database.
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId) ??
                throw new InvalidOperationException("Details concerning the calling client application cannot be found.");

            // Retrieve the permanent authorizations associated with the user and the calling client application.
            var authorizations = await _authorizationManager.FindAsync(
                subject: await _userManager.GetUserIdAsync(user),
                client: await _applicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            // Note: the same check is already made in the other action but is repeated
            // here to ensure a malicious user can't abuse this POST-only endpoint and
            // force it to return a valid response without the external authorization.
            if (!authorizations.Any() && await _applicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            }

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Note: in this sample, the granted scopes match the requested scope
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            // Automatically create a permanent authorization to avoid requiring explicit consent
            // for future authorization or token requests containing the same scopes.
            var authorization = authorizations.LastOrDefault();
            if (authorization == null)
            {
                authorization = await _authorizationManager.CreateAsync(
                    principal: principal,
                    subject: await _userManager.GetUserIdAsync(user),
                    client: await _applicationManager.GetIdAsync(application),
                    type: AuthorizationTypes.Permanent,
                    scopes: principal.GetScopes());
            }

            principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }



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

                if (!_passwordLoginOptions.Enabled && !user.IsAdministrator)
                {
                    return BadRequest(new OpenIddictResponse
                    {
                        Error = Errors.InvalidGrant,
                        ErrorDescription = "The username/password login is disabled."
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
                //var ticket = await CreateTicketAsync(openIdConnectRequest, user);
                var claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

                // Set the list of scopes granted to the client application.
                claimsPrincipal.SetScopes(new[]
                {
                    Scopes.OpenId,
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.Roles
                }.Intersect(openIdConnectRequest.GetScopes()));

                foreach (var claim in claimsPrincipal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim, claimsPrincipal));
                }


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
                }

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, claimsPrincipal);
                await _eventPublisher.Publish(new UserLoginEvent(user));

                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (openIdConnectRequest.IsAuthorizationCodeGrantType() || openIdConnectRequest.IsRefreshTokenGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Create a new ClaimsPrincipal containing the claims that
                // will be used to create an id_token, a token or a code.
                var principal = await _signInManager.CreateUserPrincipalAsync(user);

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim, principal));
                }

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }
            else if (openIdConnectRequest.IsClientCredentialsGrantType())
            {
                var application = await _applicationManager.FindByClientIdAsync(openIdConnectRequest.ClientId);
                if (application == null)
                {
                    throw new InvalidOperationException("The application details cannot be found in the database.");
                }

                // Create a new ClaimsIdentity containing the claims that
                // will be used to create an id_token, a token or a code.
                var identity = new ClaimsIdentity(
                    TokenValidationParameters.DefaultAuthenticationType,
                    Claims.Name, Claims.Role);

                // Use the client_id as the subject identifier.
                identity.AddClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application),
                    Destinations.AccessToken, Destinations.IdentityToken);

                identity.AddClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application),
                    Destinations.AccessToken, Destinations.IdentityToken);

                // Note: In the original OAuth 2.0 specification, the client credentials grant
                // doesn't return an identity token, which is an OpenID Connect concept.
                //
                // As a non-standardized extension, OpenIddict allows returning an id_token
                // to convey information about the client application when the "openid" scope
                // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
                // scope is not explicitly set, no identity token is returned to the client application.

                // Set the list of scopes granted to the client application in access_token.
                var principal = new ClaimsPrincipal(identity);
                principal.SetScopes(openIdConnectRequest.GetScopes());
                principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

                foreach (var claim in principal.Claims)
                {
                    claim.SetDestinations(GetDestinations(claim));
                }

                return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return BadRequest(new OpenIddictResponse
            {
                Error = Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        [HttpGet("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            await _signInManager.SignOutAsync();

            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
            // to the post_logout_redirect_uri specified by the client application or to
            // the RedirectUri specified in the authentication properties if none was set.
            return SignOut(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = "/"
                });
        }
        #endregion
        private IEnumerable<string> GetDestinations(Claim claim)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            return claim.Type switch
            {
                Claims.Name or
                Claims.Subject
                    => ImmutableArray.Create(Destinations.AccessToken, Destinations.IdentityToken),

                _ => ImmutableArray.Create(Destinations.AccessToken),
            };
        }
        private IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }

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
