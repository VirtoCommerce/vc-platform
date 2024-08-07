using System.Collections.Generic;
using System.Security.Claims;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsSsoAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetAuthenticationMethod() != null;
        }

        public static string GetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetClaim(ClaimTypes.AuthenticationMethod);
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
    }
}
