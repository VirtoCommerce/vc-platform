using System;
using System.Collections.Generic;
using Hangfire.Dashboard;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    [CLSCompliant(false)]
    public class PermissionBasedAuthorizationFilter : CheckPermissionAttribute, IDashboardAuthorizationFilter
    {
        private readonly ISecurityService _securityService;

        public PermissionBasedAuthorizationFilter(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public bool Authorize(DashboardContext context)
        {
            var owinContext = new OwinContext(context.GetOwinEnvironment());
            var principal = owinContext.Authentication.User;
            var isAuthorized = IsAuthorized(_securityService, principal);
            return isAuthorized;
        }
    }
}
