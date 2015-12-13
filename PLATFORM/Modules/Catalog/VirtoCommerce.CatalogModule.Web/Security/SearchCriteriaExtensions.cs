using System.Linq;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CatalogModule.Web.Security
{
    public static class SearchCriteriaExtensions
    {
        public static void ApplyRestrictionsForUser(this SearchCriteria criteria, string userName, ISecurityService securityService)
        {
            // Check global permission
            if (!securityService.UserHasAnyPermission(userName, null, CatalogPredefinedPermissions.Read))
            {
                // Get user 'read' permission scopes
                var readPermissionScopes = securityService.GetUserPermissions(userName)
                    .Where(x => x.Id.StartsWith(CatalogPredefinedPermissions.Read))
                    .SelectMany(x => x.AssignedScopes)
                    .ToList();

                // Filter by selected catalog
                criteria.CatalogIds = readPermissionScopes.OfType<CatalogSelectedScope>()
                    .Select(x => x.Scope)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();

                // Filter by selected category
                criteria.CategoryIds = readPermissionScopes.OfType<CatalogSelectedCategoryScope>()
                    .Select(x => x.Scope)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToArray();
            }
        }
    }
}
