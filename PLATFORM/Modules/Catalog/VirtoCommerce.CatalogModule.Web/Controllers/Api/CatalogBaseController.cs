using System.Linq;
using System.Net;
using System.Web.Http;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CatalogModule.Web.Controllers.Api
{
    public class CatalogBaseController : ApiController
    {
        private readonly ISecurityService _securityService;
        private readonly IPermissionScopeService _permissionScopeService;

        public CatalogBaseController(ISecurityService securityService, IPermissionScopeService permissionScopeService)
        {
            _securityService = securityService;
            _permissionScopeService = permissionScopeService;
        }

        protected string[] GetObjectPermissionScopeStrings(object obj)
        {
            return _permissionScopeService.GetObjectPermissionScopeStrings(obj).ToArray();
        }

        protected void CheckCurrentUserHasPermissionForObjects(string permission, params object[] objects)
        {
            //Scope bound security check
            var scopes = objects.SelectMany(x => _permissionScopeService.GetObjectPermissionScopeStrings(x)).Distinct().ToArray();
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, permission))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }

        /// <summary>
        /// Filter catalog search criteria based on current user permissions
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected void ApplyRestrictionsForCurrentUser(SearchCriteria criteria)
        {
            var userName = User.Identity.Name;
            criteria.ApplyRestrictionsForUser(userName, _securityService);
        }
    }
}
