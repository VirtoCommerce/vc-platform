﻿using System;
using System.Collections.Generic;
using Hangfire.Dashboard;
using Microsoft.Owin;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web.Hangfire
{
    [CLSCompliant(false)]
    public class PermissionBasedAuthorizationFilter : CheckPermissionAttribute, IAuthorizationFilter
    {
        private readonly ISecurityService _securityService;

        public PermissionBasedAuthorizationFilter(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            var context = new OwinContext(owinEnvironment);
            var principal = context.Authentication.User;
            var isAuthorized = IsAuthorized(_securityService, principal);
            return isAuthorized;
        }
    }
}
