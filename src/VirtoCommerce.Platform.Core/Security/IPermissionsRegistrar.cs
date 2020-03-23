using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IPermissionsRegistrar
    {
        void RegisterPermissions(Permission[] permissions);
        IEnumerable<Permission> GetAllPermissions();
    }
}
