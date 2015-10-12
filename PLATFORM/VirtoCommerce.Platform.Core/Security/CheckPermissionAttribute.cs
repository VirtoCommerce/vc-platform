using System;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        #region Static Fields

        private static readonly string[] _emptyArray = new string[0];

        #endregion

        #region Fields

        private string _permission;
        private string[] _permissions = _emptyArray;

        #endregion

        #region Public Properties

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

        #endregion

        #region Methods

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
                throw new ArgumentNullException("actionContext");
            }

            var isAuthorized = base.IsAuthorized(actionContext);

            if (isAuthorized && _permissions.Length > 0)
            {
                var securityService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ISecurityService)) as ISecurityService;
                var principal = actionContext.RequestContext.Principal;
                isAuthorized = IsAuthorized(securityService, principal);
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

        #endregion
    }
}
