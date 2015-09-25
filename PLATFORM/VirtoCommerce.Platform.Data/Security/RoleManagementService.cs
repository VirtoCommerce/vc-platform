using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Security.Converters;

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

                result.Roles = roles.Select(r => r.ToCoreModel()).ToArray();
            }

            return result;
        }

        public Role GetRole(string roleId)
        {
            Role result = null;

            using (var repository = _platformRepository())
            {
                var role = repository.GetRoleById(roleId);

                if (role != null)
                {
                    result = role.ToCoreModel();
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
			    var targetEntry = repository.GetRoleById(role.Id);

                //Create not exist permissions
                if(role.Permissions != null)
                {
                    var permissionIds = role.Permissions.Select(x => x.Id).ToArray();
                    var alreadyExistPermissionIds = repository.Permissions.Where(x => permissionIds.Contains(x.Id))
                                                            .Select(x => x.Id)
                                                            .ToArray();
                    var notExistPermissionIds = permissionIds.Except(alreadyExistPermissionIds).ToArray();
                    foreach(var notExistPermissionId in notExistPermissionIds)
                    {
                        var permission = role.Permissions.First(x => x.Id == notExistPermissionId).ToDataModel();
                        repository.Add(permission);
                    }
                }
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
  
    }
}
