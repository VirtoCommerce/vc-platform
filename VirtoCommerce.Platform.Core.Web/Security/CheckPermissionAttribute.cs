using System;
using System.Linq;
using System.Security.Claims;
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
                var securityService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISecurityService)) as ISecurityService;
                isAuthorized = IsAuthorized(securityService, principal);
            }

            return isAuthorized;
        }

        protected bool IsAuthorized(ISecurityService securityService, IPrincipal principal)
        {
            var isAuthorized = false;

            if (securityService != null && principal != null)
            {
                var filteredPermissions = _permissions;

                // NOTE: if the user identity has claim named "LimitedPermissions", this attribute should authorize only
                //       permissions listed in that claim. Any permissions that are required by this attribute but
                //       not listed in the claim should cause this method to return false.
                //       However, if permission limits of user identity are not defined ("LimitedPermissions" claim is missing),
                //       then no limitations should be applied to the permissions.
                if (principal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var claim = claimsIdentity.FindFirst(PermissionConstants.LimitedPermissionsClaimName);
                    if (claim != null)
                    {
                        var limitedPermissions = claim.Value?.Split(new[] { PermissionConstants.PermissionsDelimiter }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                        filteredPermissions = _permissions.Where(limitedPermissions.Contains).ToArray();
                    }
                }

                if (filteredPermissions.Any())
                {
                    isAuthorized = securityService.UserHasAnyPermission(principal.Identity.Name, null, filteredPermissions);
                }
            }

            return isAuthorized;
        }
    }
}
