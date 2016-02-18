using System.Security.Claims;
using System.Threading;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class CurrentPrincipal
    {
        public static string GetCurrentUserName()
        {
            string userName = null;

            var principal = Thread.CurrentPrincipal;
            if (principal != null)
            {
                var claimsPrincipal = principal as ClaimsPrincipal;
                if (claimsPrincipal != null)
                {
                    var claim = claimsPrincipal.FindFirst(VirtoCommerceClaimTypes.UserName);
                    userName = claim != null ? claim.Value : null;
                }

                if (string.IsNullOrEmpty(userName))
                {
                    userName = principal.Identity.Name;
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                userName = "unknown";
            }

            return userName;
        }
    }
}
