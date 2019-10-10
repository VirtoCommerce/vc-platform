using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    public class DefaultPermissionProvider : IPermissionsRegistrar
    {
        private readonly List<Permission> _permissions = new List<Permission>();

        public IEnumerable<Permission> GetAllPermissions()
        {
            return _permissions;
        }

        public void RegisterPermissions(params Permission[] permissions)
        {
            _permissions.AddRange(permissions);
        }
    }
}
