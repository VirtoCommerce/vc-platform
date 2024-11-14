using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static string[] UserIdClaimTypes { get; set; } = [];

        public static string[] UserNameClaimTypes { get; set; } = [];

        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindAnyValue(UserIdClaimTypes);
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindAnyValue(UserNameClaimTypes);
        }

        public static string FindAnyValue(this ClaimsPrincipal claimsPrincipal, IList<string> claimTypes)
        {
            if (claimsPrincipal != null)
            {
                foreach (var claimType in claimTypes)
                {
                    var value = claimsPrincipal.FindFirstValue(claimType);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                }
            }

            return null;
        }

        public static Permission FindPermission(this ClaimsPrincipal principal, string permissionName, JsonSerializerSettings jsonSettings)
        {
            return FindPermissions(principal, permissionName, jsonSettings).FirstOrDefault();
        }

        public static IList<Permission> FindPermissions(this ClaimsPrincipal principal, string permissionName, JsonSerializerSettings jsonSettings)
        {
            var result = new List<Permission>();
            foreach (var claim in principal.Claims)
            {
                var permission = Permission.TryCreateFromClaim(claim, jsonSettings);
                if (permission != null && permission.Name.EqualsInvariant(permissionName))
                {
                    result.Add(permission);
                }
            }
            return result;
        }

        public static bool HasGlobalPermission(this ClaimsPrincipal principal, string permissionName)
        {
            // TODO: Check cases with locked user
            var result = principal.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

            if (!result)
            {
                result = principal.HasClaim(PlatformConstants.Security.Claims.PermissionClaimType, permissionName);
            }
            return result;
        }
    }
}
