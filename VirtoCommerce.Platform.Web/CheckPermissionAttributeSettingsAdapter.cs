using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Web.Security;

namespace VirtoCommerce.Platform.Web
{
    public class CheckPermissionAttributeSettingsAdapter : ICheckPermissionAttributeSettings
    {
        private readonly AuthenticationOptions _authenticationOptions;

        public CheckPermissionAttributeSettingsAdapter(AuthenticationOptions authenticationOptions)
        {
            _authenticationOptions = authenticationOptions;
        }

        public string RegularCookieAuthenticationType => _authenticationOptions.AuthenticationType;
        public string LimitedCookieAuthenticationType => _authenticationOptions.PermissionCookieAuthenticationType;
        public string[] LimitedCookiePermissions => _authenticationOptions.PermissionsToCheck.Split(new [] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToArray();
    }
}
