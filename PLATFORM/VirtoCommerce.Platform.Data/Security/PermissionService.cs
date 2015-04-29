using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

namespace VirtoCommerce.Platform.Data.Security
{
    public class PermissionService : IPermissionService
    {
        private readonly Func<IPlatformRepository> _platformRepository;
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly CacheManager _cacheManager;

        public PermissionService(Func<IPlatformRepository> platformRepository, IModuleManifestProvider manifestProvider, CacheManager cacheManager)
        {
            _platformRepository = platformRepository;
            _cacheManager = cacheManager;
            _manifestProvider = manifestProvider;
        }

        #region Public Methods and Operators

        public Permission[] GetAllPermissions()
        {
            return _cacheManager.Get(
                CacheKey.Create(CacheGroups.Security, "AllPermissions"),
                LoadAllPermissions);
        }

        public string[] GetUserPermissionIds(string userName)
        {
            var user = GetUserWithPermissions(userName);
            return user.ActivePermissionIds;
        }

        public bool UserHasAnyPermission(string userName, params string[] permissionIds)
        {
            var user = GetUserWithPermissions(userName);

            var success = user.IsActive && (
                user.RegisterType == RegisterType.Administrator
                    || user.ActivePermissionIds.Intersect(permissionIds, StringComparer.OrdinalIgnoreCase).Any()
                    || user.RegisterType == RegisterType.SiteAdministrator && permissionIds.Contains(PredefinedPermissions.SecurityCallApi, StringComparer.OrdinalIgnoreCase) // Temporary workaround for frontend. Will be deleted later.
                );

            return success;
        }

        #endregion

        #region Methods


        private UserWithPermissions GetUserWithPermissions(string userName)
        {
            return _cacheManager.Get(
                CacheKey.Create(CacheGroups.Security, "UserWithPermissions", userName),
                () => LoadUserWithPermissions(userName));
        }

        private Permission[] LoadAllPermissions()
        {
            var manifestPermissions = _manifestProvider.GetModuleManifests().Values
                .Where(m => m.Permissions != null)
                .SelectMany(m => m.Permissions.Select(p => p.ToCoreModel(m.Id)))
                .ToArray();

            var allPermissions = PredefinedPermissions.Permissions.Union(manifestPermissions).ToArray();
            return allPermissions;
        }

        private UserWithPermissions LoadUserWithPermissions(string userName)
        {
            var allPermissionIds = GetAllPermissions().Select(p => p.Id).ToArray();
            var user = new UserWithPermissions();

            using (var repository = _platformRepository())
            {
                var account = repository.Accounts
                    .Include(a => a.RoleAssignments.Select(ra => ra.Role.RolePermissions))
                    .FirstOrDefault(a => a.UserName == userName);

                if (account != null)
                {
                    user.IsActive = account.AccountState == AccountState.Approved;
                    user.RegisterType = account.RegisterType;
                    user.StoredPermissionIds = account.RoleAssignments
                        .Select(ra => ra.Role)
                        .SelectMany(r => r.RolePermissions)
                        .Select(rp => rp.PermissionId)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                }
            }

            user.ActivePermissionIds = allPermissionIds.Intersect(user.StoredPermissionIds).ToArray();

            return user;
        }

        #endregion

        private class UserWithPermissions
        {
            #region Static Fields

            private static readonly string[] _emptyPermissionIds = new string[0];

            #endregion

            #region Constructors and Destructors

            public UserWithPermissions()
            {
                StoredPermissionIds = _emptyPermissionIds;
                ActivePermissionIds = _emptyPermissionIds;
            }

            #endregion

            #region Public Properties

            public string[] ActivePermissionIds { get; set; }

            public bool IsActive { get; set; }
            public RegisterType RegisterType { get; set; }
            public string[] StoredPermissionIds { get; set; }

            #endregion
        }
    }
}
