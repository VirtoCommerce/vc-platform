using System;
using System.Collections.Generic;
using System.Text;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface IPermissionsRegistrar
    {
        void RegisterPermissions(Permission[] permissions);
        IEnumerable<Permission> GetAllPermissions();
    }
}
