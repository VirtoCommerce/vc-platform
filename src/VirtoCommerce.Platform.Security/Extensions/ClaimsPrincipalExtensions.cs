using System;
using System.Collections.Generic;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsExternalSignIn(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetAuthenticationMethod() != null;
        }

        public static string GetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
        }

        public static ClaimsPrincipal SetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal, string value, IList<string> destinations)
        {
            return claimsPrincipal?.SetClaimWithDestinations(ClaimTypes.AuthenticationMethod, value, destinations);
        }

        [Obsolete("Use IsExternalSignIn()", DiagnosticId = "VC0009", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
        public static bool IsSsoAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsExternalSignIn();
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
    }
}
