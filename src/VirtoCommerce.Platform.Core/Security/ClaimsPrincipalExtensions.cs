using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class ClaimsPrincipalExtensions
    {
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
