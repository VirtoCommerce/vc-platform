using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core.Web.Security
{
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        private static readonly string[] _emptyArray = new string[0];
        private string _permission;
        private string[] _permissions = _emptyArray;

        public string Permission
        {
            get { return _permission ?? string.Empty; }
            set
            {
                _permission = value;
                _permissions = this.SplitString(value, ',');
            }
        }

        public string[] Permissions
        {
            get { return _permissions ?? _emptyArray; }
            set
            {
                _permissions = value;
                _permission = string.Join(",", value ?? _emptyArray);
            }
        }

        /// <summary>
        ///     Indicates whether the specified control is authorized.
        /// </summary>
        /// <returns>
        ///     true if the control is authorized; otherwise, false.
        /// </returns>
        /// <param name="actionContext">The context.</param>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var isAuthorized = base.IsAuthorized(actionContext);
            if (isAuthorized && _permissions.Length > 0)
            {
                var principal = actionContext.RequestContext.Principal;
                var dependencyResolver = actionContext.ControllerContext.Configuration.DependencyResolver;

                var authenticationType = principal.Identity.AuthenticationType;
                var settings = dependencyResolver.GetService(typeof(ICheckPermissionAttributeSettings)) as ICheckPermissionAttributeSettings;
                if (settings != null && authenticationType == settings.LimitedCookieAuthenticationType)
                {
                    // If the user is authorized by helper cookies with limited set of permissions, we can authorize these permissions only.
                    // If this attribute requires any other permissions, this method will return false.
                    isAuthorized = Permissions.All(settings.LimitedCookiePermissions.Contains);
                }

                if (isAuthorized)
                {
                    var securityService = dependencyResolver.GetService(typeof(ISecurityService)) as ISecurityService;
                    isAuthorized = IsAuthorized(securityService, principal);
                }
            }

            return isAuthorized;
        }

        protected bool IsAuthorized(ISecurityService securityService, IPrincipal principal)
        {
            var isAuthorized = false;

            if (securityService != null && principal != null)
            {
                isAuthorized = securityService.UserHasAnyPermission(principal.Identity.Name, null, _permissions);
            }

            return isAuthorized;
        }
    }
}
