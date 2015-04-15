using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.CoreModule.Web.Converters;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Security;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class RoleManagementService : ServiceBase, IRoleManagementService
    {
        private readonly Func<ISecurityRepository> _securityRepository;

        public RoleManagementService(Func<ISecurityRepository> securityRepository)
        {
            _securityRepository = securityRepository;
        }

        #region IRoleManagementService Members

        public RoleSearchResponse SearchRoles(RoleSearchRequest request)
        {
            request = request ?? new RoleSearchRequest();
            var result = new RoleSearchResponse();

            using (var repository = _securityRepository())
            {
                var query = repository.Roles;

                if (request.Keyword != null)
                {
                    query = query.Where(r => r.Name.Contains(request.Keyword));
                }

                result.TotalCount = query.Count();

                var roles = query
                    .OrderBy(r => r.Name)
                    .Skip(request.Start)
                    .Take(request.Count)
                    .ToArray();

                result.Roles = roles.Select(r => ConvertToRoleDescriptor(r, false)).ToArray();
            }

            return result;
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

        public void DeleteRole(string roleId)
        {
            using (var repository = _securityRepository())
            {
                var role = repository.Roles.FirstOrDefault(r => r.RoleId == roleId);

                if (role != null)
                {
                    repository.Remove(role);
                    CommitChanges(repository);
                }
            }
        }

        public RoleDescriptor AddOrUpdateRole(RoleDescriptor role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            var sourceEntry = role.ToFoundation();

            using (var repository = _securityRepository())
            {
                AddOrUpdatePermissions(repository, role.Permissions);

                var targetEntry = repository.Roles
                    .Include(r => r.RolePermissions)
                    .FirstOrDefault(r => r.RoleId == role.Id);

                if (targetEntry == null)
                {
                    repository.Add(sourceEntry);
                }
                else
                {
                    sourceEntry.Patch(targetEntry);
                }

                CommitChanges(repository);
            }

            var result = GetRole(sourceEntry.RoleId);
            return result;
        }

        #endregion


        private static void AddOrUpdatePermissions(ISecurityRepository repository, PermissionDescriptor[] permissions)
        {
            if (permissions != null)
            {
                var permissionIds = permissions.Select(p => p.Id).ToArray();
                var existingPermissions = repository.Permissions.Where(p => permissionIds.Contains(p.PermissionId)).ToArray();

                foreach (var permission in permissions)
                {
                    var sourceEntry = permission.ToFoundation();
                    var targetEntry = existingPermissions.FirstOrDefault(p => string.Equals(p.PermissionId, permission.Id, StringComparison.OrdinalIgnoreCase));

                    if (targetEntry == null)
                    {
                        repository.Add(sourceEntry);
                    }
                    else
                    {
                        sourceEntry.Patch(targetEntry);
                    }
                }
            }
        }

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
