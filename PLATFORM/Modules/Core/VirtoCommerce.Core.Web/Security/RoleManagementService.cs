using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Security;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly Func<ISecurityRepository> _securityRepository;

        public RoleManagementService(Func<ISecurityRepository> securityRepository)
        {
            _securityRepository = securityRepository;
        }

        #region IRoleManagementService Members

        public RoleDescriptor[] GetAllRoles()
        {
            using (var repository = _securityRepository())
            {
                var roles = repository.Roles
                    .OrderBy(r => r.Name)
                    .ToArray();

                var result = roles.Select(r => ConvertToRoleDescriptor(r, false))
                    .ToArray();

                return result;
            }
        }

        public RoleDescriptor GetRole(string roleId)
        {
            RoleDescriptor result = null;

            using (var repository = _securityRepository())
            {
                var role = repository.Roles
                    .Include(r => r.RolePermissions.Select(rp => rp.Permission))
                    .FirstOrDefault(r => r.RoleId == roleId);

                if (role != null)
                {
                    result = ConvertToRoleDescriptor(role, true);
                }
            }

            return result;
        }

        #endregion


        private static RoleDescriptor ConvertToRoleDescriptor(Role role, bool fillPermissions)
        {
            var result = new RoleDescriptor
            {
                Id = role.RoleId,
                Name = role.Name,
            };

            if (fillPermissions && role.RolePermissions != null)
            {
                result.Permissions = role.RolePermissions.Select(rp => ConvertToPermissionDescriptor(rp.Permission)).ToArray();
            }

            return result;
        }

        private static PermissionDescriptor ConvertToPermissionDescriptor(Permission permission)
        {
            return new PermissionDescriptor
            {
                Id = permission.PermissionId,
                Name = permission.Name
            };
        }
    }
}
