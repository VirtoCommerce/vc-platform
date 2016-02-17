using System;
using System.Security.Claims;

namespace VirtoCommerce.Storefront.Common
{
    public static class IdentityExtensions
    {
        public static string FindFirstValue(this ClaimsPrincipal principal, string claimType)
        {
            if (principal == null)
                throw new ArgumentNullException("principal");

            var claim = principal.FindFirst(claimType);
            return claim != null ? claim.Value : null;
        }
    }
}
