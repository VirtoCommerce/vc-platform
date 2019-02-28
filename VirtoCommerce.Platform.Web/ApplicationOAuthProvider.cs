using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;
using VirtoCommerce.Platform.Core.Web.Security;
using VirtoCommerce.Platform.Data.Security.Identity;
using PlatformAuthenticationOptions = VirtoCommerce.Platform.Core.Security.AuthenticationOptions;

namespace VirtoCommerce.Platform.Web
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private readonly PlatformAuthenticationOptions _authenticationOptions;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISecurityService _securityService;

        public ApplicationOAuthProvider(string publicClientId, PlatformAuthenticationOptions authenticationOptions,
            IEventPublisher eventPublisher, ISecurityService securityService)
        {
            _publicClientId = publicClientId ?? throw new ArgumentNullException(nameof(publicClientId));
            _authenticationOptions = authenticationOptions;
            _eventPublisher = eventPublisher;
            _securityService = securityService;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oAuthIdentity = await userManager.CreateIdentityAsync(user, OAuthDefaults.AuthenticationType);
            var properties = new Dictionary<string, string>
            {
                { "userName", user.UserName },
            };
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties(properties));
            context.Validated(ticket);

            var platformUser = await _securityService.FindByNameAsync(context.UserName, UserDetails.Full);
            var limitedCookiePermissions = _authenticationOptions.BearerAuthorizationLimitedCookiePermissions?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
            if (!platformUser.IsAdministrator)
            {
                var allUserPermissions = _securityService.GetUserPermissions(user.UserName).Select(x => x.Id);
                limitedCookiePermissions = limitedCookiePermissions.Intersect(allUserPermissions, StringComparer.OrdinalIgnoreCase).ToArray();
            }
            if (limitedCookiePermissions.Any())
            {
                // Issue a helper cookie - it will be used to authorize some non-AJAX requests
                var cookiesIdentity = await userManager.CreateIdentityAsync(user, _authenticationOptions.AuthenticationType);
                cookiesIdentity.AddClaim(new Claim(VirtoCommerceClaimTypes.LimitedPermissionsClaimName, string.Join(";", limitedCookiePermissions)));
                context.Request.Context.Authentication.SignIn(cookiesIdentity);
            }

            await _eventPublisher.Publish(new UserLoginEvent(platformUser));
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change auth ticket for refresh token requests
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.CompletedTask;
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.CompletedTask;
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.CompletedTask;
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                var expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
                else if (context.ClientId == OwinConfig.PublicClientId)
                {
                    var expectedUri = new Uri(context.Request.Uri, "/");
                    context.Validated(expectedUri.AbsoluteUri);
                }
            }

            return Task.CompletedTask;
        }
    }
}
