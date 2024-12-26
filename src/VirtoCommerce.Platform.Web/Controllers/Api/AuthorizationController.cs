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
using VirtoCommerce.Platform.Web.ActionConstraints;
using VirtoCommerce.Platform.Web.Extensions;
using VirtoCommerce.Platform.Web.Model;
using VirtoCommerce.Platform.Web.Security;
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
        private readonly IOpenIddictAuthorizationManager _authorizationManager;
        private readonly IOpenIddictScopeManager _scopeManager;

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
            IExternalSignInService externalSignInService,
            IOpenIddictAuthorizationManager authorizationManager,
            IOpenIddictScopeManager scopeManager)
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
            _authorizationManager = authorizationManager;
            _scopeManager = scopeManager;
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
                // when the user password/roles change, use _signInManager.ValidateSecurityStampAsync(info.Principal) instead.
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

                var destinations = new[] { Destinations.AccessToken };
                CopyClaim(info.Principal, ticket.Principal, ClaimTypes.AuthenticationMethod, destinations);
                CopyClaim(info.Principal, ticket.Principal, PlatformConstants.Security.Claims.OperatorUserId, destinations);
                CopyClaim(info.Principal, ticket.Principal, PlatformConstants.Security.Claims.OperatorUserName, destinations);

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
                var operatorUserId = User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserId)?.EmptyToNull() ?? user.Id;
                var operatorUserName = User.FindFirstValue(PlatformConstants.Security.Claims.OperatorUserName)?.EmptyToNull() ?? user.UserName;

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

            if (openIdConnectRequest.IsAuthorizationCodeGrantType())
            {
                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                // Note: if you want to automatically invalidate the refresh token
                // when the user password/roles change, use _signInManager.ValidateSecurityStampAsync(info.Principal) instead.
                var user = await _userManager.GetUserAsync(info.Principal);
                if (user == null)
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The refresh token is no longer valid.",
                    });

                    return Forbid(properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    var properties = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in.",
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

            return BadRequest(SecurityErrorDescriber.UnsupportedGrantType());
        }

        #endregion

        [HttpGet("~/connect/authorize")]
        [HttpPost("~/connect/authorize")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Authorize()
        {
            var request = GetOpenIddictServerRequest();

            // If prompt=login was specified by the client application,

            // Retrieve the user principal stored in the authentication cookie.
            // If a max_age parameter was provided, ensure that the cookie is not too old.
            // If the user principal can't be extracted or the cookie is too old, redirect the user to the login page.
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

            if (!result.Succeeded || RequestHasExpired(request, result))
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
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is not logged in.",
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

            var (user, application, authorizations) = await GetUserApplicationAuthorizationsAsync(request, result.Principal);

            switch (await _applicationManager.GetConsentTypeAsync(application))
            {
                // If the consent is external (e.g. when authorizations are granted by a sysadmin),
                // immediately return an error if no authorization can be found in the database.
                case ConsentTypes.External when authorizations.Count == 0:
                    return ConsentNotGiven();

                // If the consent is implicit or if an authorization was found,
                // return an authorization response without displaying the consent form.
                case ConsentTypes.Implicit:
                case ConsentTypes.External when authorizations.Count != 0:
                case ConsentTypes.Explicit when authorizations.Count != 0 && !request.HasPrompt(Prompts.Consent):
                    return await SignInAsync(request, user, application, authorizations);

                // At this point, no authorization was found in the database and an error must be returned
                // if the client application specified prompt=none in the authorization request.
                case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                case ConsentTypes.Systematic when request.HasPrompt(Prompts.None):
                    return Forbid(
                        authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "Interactive user consent is required.",
                        }));

                // In every other case, render the consent form.
                default:
                    HttpContext.OverrideScpFormActionUri(request.RedirectUri);

                    return View(new AuthorizeViewModel
                    {
                        ApplicationName = await _applicationManager.GetDisplayNameAsync(application),
                        Scope = request.Scope
                    });
            }
        }

        [HttpPost("~/connect/authorize")]
        [HasFormValue("submit.Deny")]
        [Authorize]
        [AllowAnonymous]
        // Notify OpenIddict that the authorization grant has been denied by the resource owner
        // to redirect the user agent to the client application using the appropriate response_mode.
        public IActionResult Deny()
        {
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpPost("~/connect/authorize")]
        [HasFormValue("submit.Accept")]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Accept()
        {
            var request = GetOpenIddictServerRequest();

            var (user, application, authorizations) = await GetUserApplicationAuthorizationsAsync(request, User);

            // Note: the same check is already made in the other action but is repeated
            // here to ensure a malicious user can't abuse this POST-only endpoint and
            // force it to return a valid response without the external authorization.
            if (authorizations.Count == 0 && await _applicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            {
                return ConsentNotGiven();
            }

            return await SignInAsync(request, user, application, authorizations);
        }

        // GET: /api/userinfo
        [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
        [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
        [AllowAnonymous]
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
                            "The specified access token is bound to an account that no longer exists.",
                    }));
            }

            var claims = new Dictionary<string, object>(StringComparer.Ordinal)
            {
                // Note: the "sub" claim is a mandatory claim and must be included in the JSON response.
                [Claims.Subject] = await _userManager.GetUserIdAsync(user),
                [Claims.Username] = user.UserName,
                [Claims.Name] = user.UserName
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

        [HttpGet("~/connect/logout")]
        public async Task<IActionResult> Logout()
        {
            // Ask ASP.NET Core Identity to delete the local and external cookies created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g. Google or Facebook).
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


        private static void CopyClaim(ClaimsPrincipal source, ClaimsPrincipal target, string claimType, IList<string> destinations)
        {
            var value = source.FindFirstValue(claimType);
            target.SetClaimWithDestinations(claimType, value, destinations);
        }

        private static bool RequestHasExpired(OpenIddictRequest request, AuthenticateResult result)
        {
            return request.MaxAge != null &&
                   result.Properties?.IssuedUtc != null &&
                   DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value);
        }

        private OpenIddictRequest GetOpenIddictServerRequest()
        {
            return HttpContext.GetOpenIddictServerRequest() ??
                   throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
        }

        private async Task<(ApplicationUser user, OpenIddictEntityFrameworkCoreApplication application, List<object> authorizations)>
            GetUserApplicationAuthorizationsAsync(OpenIddictRequest request, ClaimsPrincipal principal)
        {
            // Retrieve the profile of the logged-in user.
            var user = await _userManager.GetUserAsync(principal) ??
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

            return (user, application, authorizations);
        }

        private ForbidResult ConsentNotGiven()
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The logged-in user has not given consent for this application.",
                }));
        }

        private async Task<IActionResult> SignInAsync(
            OpenIddictRequest request,
            ApplicationUser user,
            OpenIddictEntityFrameworkCoreApplication application,
            List<object> authorizations)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // Note: in this sample, the granted scopes match the requested scope,
            // but you may want to allow the user to uncheck specific scopes.
            // For that, simply restrict the list of scopes before calling SetScopes.
            principal.SetScopes(request.GetScopes());
            principal.SetResources(await _scopeManager.ListResourcesAsync(principal.GetScopes()).ToListAsync());

            // Automatically create a permanent authorization to avoid requiring explicit consent
            // for future authorization or token requests containing the same scopes.
            var authorization = authorizations.LastOrDefault() ??
                                await _authorizationManager.CreateAsync(
                                    principal: principal,
                                    subject: await _userManager.GetUserIdAsync(user),
                                    client: await _applicationManager.GetIdAsync(application),
                                    type: AuthorizationTypes.Permanent,
                                    scopes: principal.GetScopes());

            principal.SetAuthorizationId(await _authorizationManager.GetIdAsync(authorization));

            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch (claim.Type)
            {
                case Claims.Name:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Profile))
                    {
                        yield return Destinations.IdentityToken;
                    }

                    yield break;

                case Claims.Email:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Email))
                    {
                        yield return Destinations.IdentityToken;
                    }

                    yield break;

                case Claims.Role:
                    yield return Destinations.AccessToken;

                    if (principal.HasScope(Scopes.Roles))
                    {
                        yield return Destinations.IdentityToken;
                    }

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp":
                    yield break;

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
