using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Security
{
    public class RoleManagementService : ServiceBase, IRoleManagementService
    {
        private readonly Func<IPlatformRepository> _platformRepository;

        public RoleManagementService(Func<IPlatformRepository> platformRepository)
        {
            _platformRepository = platformRepository;
        }

        #region IRoleManagementService Members

        public RoleSearchResponse SearchRoles(RoleSearchRequest request)
        {
            request = request ?? new RoleSearchRequest();
            var result = new RoleSearchResponse();

            using (var repository = _platformRepository())
            {
                var query = repository.Roles;

                if (request.Keyword != null)
                {
                    query = query.Where(r => r.Name.Contains(request.Keyword));
                }

                result.TotalCount = query.Count();

                var roles = query
                    .OrderBy(r => r.Name)
                    .Skip(request.SkipCount)
                    .Take(request.TakeCount)
                    .Include(r => r.RolePermissions.Select(rp => rp.Permission))
                    .ToArray();

                result.Roles = roles.Select(r => r.ToCoreModel(true)).ToArray();
            }

            return result;
        }

        public Role GetRole(string roleId)
        {
            Role result = null;

            using (var repository = _platformRepository())
            {
                var role = repository.Roles
                    .Include(r => r.RolePermissions.Select(rp => rp.Permission))
                    .FirstOrDefault(r => r.Id == roleId);

                if (role != null)
                {
                    result = role.ToCoreModel(true);
                }
            }

            return result;
        }

        public void DeleteRole(string roleId)
        {
            using (var repository = _platformRepository())
            {
                var role = repository.Roles.FirstOrDefault(r => r.Id == roleId);

                if (role != null)
                {
                    repository.Remove(role);
                    CommitChanges(repository);
                }
            }
        }

        public Role AddOrUpdateRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            var sourceEntry = role.ToDataModel();

            using (var repository = _platformRepository())
			using(var changeTracker = GetChangeTracker(repository))
            {
				AddOrUpdatePermissions(repository, role.Permissions);

                var targetEntry = repository.Roles.Include(r => r.RolePermissions)
											.FirstOrDefault(r => r.Id == role.Id);
				if (targetEntry == null)
				{
					repository.Add(sourceEntry);
				}
				else
				{
					changeTracker.Attach(targetEntry);
					sourceEntry.Patch(targetEntry);
				}
                

                CommitChanges(repository);
            }

            var result = GetRole(sourceEntry.Id);
            return result;
        }

        #endregion
    
		private static void AddOrUpdatePermissions(IPlatformRepository repository, Permission[] permissions)
        {
            if (permissions != null)
            {
                var permissionIds = permissions.Select(p => p.Id).ToArray();
                var existingPermissions = repository.Permissions.Where(p => permissionIds.Contains(p.Id)).ToArray();

                foreach (var permission in permissions)
                {
                    var sourceEntry = permission.ToDataModel();
                    var targetEntry = existingPermissions.FirstOrDefault(p => string.Equals(p.Id, permission.Id, StringComparison.OrdinalIgnoreCase));

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
    }
}
