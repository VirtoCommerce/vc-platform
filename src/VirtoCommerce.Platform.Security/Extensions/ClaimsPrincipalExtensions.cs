using System;
using System.Collections.Generic;
using System.Security.Claims;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Security.SignInLog;

namespace VirtoCommerce.Platform.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        [Obsolete("Use IsPasswordSignIn() instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public static bool IsExternalSignIn(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal != null && !claimsPrincipal.IsPasswordSignIn();
        }

        public static bool IsPasswordSignIn(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetAuthenticationMethod() == SignInType.Password;
        }

        public static string GetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
        }

        public static ClaimsPrincipal SetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal, string value, IList<string> destinations)
        {
            return claimsPrincipal?.SetClaimWithDestinations(ClaimTypes.AuthenticationMethod, value, destinations);
        }

        public static ClaimsPrincipal SetClaimWithDestinations(this ClaimsPrincipal claimsPrincipal, string type, string value, IList<string> destinations)
        {
            claimsPrincipal.SetClaim(type, value);

            foreach (var claim in claimsPrincipal.Claims)
            {
                if (claim.Type == type && claim.Value == value)
                {
                    claim.SetDestinations(destinations);
                }
            }

            return claimsPrincipal;
        }

        public static bool IsImpersonated(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.HasClaim(PlatformConstants.Security.Claims.OperatorUserId) == true &&
                   claimsPrincipal.HasClaim(PlatformConstants.Security.Claims.OperatorUserName);
        }
    }
}
