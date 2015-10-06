using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Platform.Core.Security;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

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
        /// Filter catalog search criteria relate to current user permissions
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        protected coreModel.SearchCriteria ChangeCriteriaToCurentUserPermissions(coreModel.SearchCriteria criteria)
        {
            var userName = User.Identity.Name;
            //first check global permission
            if (!_securityService.UserHasAnyPermission(userName, null, CatalogPredefinedPermissions.Read))
            {
                //Get user 'read' permission scopes
                var readPermissionScopes = _securityService.GetUserPermissions(userName)
                                                      .Where(x => x.Id.StartsWith(CatalogPredefinedPermissions.Read))
                                                      .SelectMany(x => x.AssignedScopes);

                //Filter by selected catalog
                criteria.CatalogsIds = readPermissionScopes.OfType<SelectedCatalogScope>()
                                                         .Select(x => x.Scope)
                                                         .Where(x => !String.IsNullOrEmpty(x)).ToArray();
                //Filter by selected category
                criteria.CategoriesIds = readPermissionScopes.OfType<SelectedCategoryScope>()
                                                        .Select(x => x.Scope)
                                                        .Where(x => !String.IsNullOrEmpty(x)).ToArray();
            }
            return criteria;
        }
    }
}
