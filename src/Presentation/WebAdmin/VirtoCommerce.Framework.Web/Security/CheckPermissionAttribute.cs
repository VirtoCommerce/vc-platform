using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using VirtoCommerce.Framework.Web.Common;

namespace VirtoCommerce.Framework.Web.Security
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
                isAuthorized = false;

                var permissionService = actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(IPermissionService)) as IPermissionService;
                if (permissionService != null)
                {
                    isAuthorized = permissionService.UserHasAnyPermission(actionContext.ControllerContext.RequestContext.Principal.Identity.Name, _permissions);
                }
            }

            return isAuthorized;
        }

        #endregion
    }
}
