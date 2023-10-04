using System.Security.Claims;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetClaim(ClaimTypes.AuthenticationMethod);
        }

        public static bool IsSsoAuthenticationMethod(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.GetClaim(ClaimTypes.AuthenticationMethod) != null;
        }
    }
}
