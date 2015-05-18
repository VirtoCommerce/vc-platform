using System;
using System.Collections.Generic;
using Hangfire.Dashboard;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    [CLSCompliant(false)]
    public class PermissionBasedAuthorizationFilter : CheckPermissionAttribute, IAuthorizationFilter
    {
        private readonly IPermissionService _permissionService;

        public PermissionBasedAuthorizationFilter(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            var context = new OwinContext(owinEnvironment);
            var principal = context.Authentication.User;
            var isAuthorized = IsAuthorized(_permissionService, principal);
            return isAuthorized;
        }
    }
}
