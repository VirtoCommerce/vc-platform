using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Security;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class PermissionService : IPermissionService
    {
        #region Fields

        private readonly CacheHelper _cache;
        private readonly TimeSpan _cacheTimeout;
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly Func<ISecurityRepository> _securityRepository;

        #endregion

        #region Constructors and Destructors

        public PermissionService(Func<ISecurityRepository> securityRepository, ICacheRepository cacheRepository, IModuleManifestProvider manifestProvider)
        {
            _securityRepository = securityRepository;
            _cache = new CacheHelper(cacheRepository);
            _cacheTimeout = TimeSpan.FromMinutes(1);
            _manifestProvider = manifestProvider;
        }

        #endregion

        #region Public Methods and Operators

        public PermissionDescriptor[] GetAllPermissions()
        {
            return _cache.Get(
                CacheHelper.CreateCacheKey("AllPermissionsCache"),
                LoadAllPermissions,
                _cacheTimeout);
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

        private PermissionDescriptor ConvertToPermissionDescriptor(ModulePermission permission)
        {
            return new PermissionDescriptor
            {
                Id = permission.Id,
                Name = permission.Name
            };
        }

        private UserWithPermissions GetUserWithPermissions(string userName)
        {
            return _cache.Get(
                CacheHelper.CreateCacheKey("UserWithPermissionsCache", userName),
                () => LoadUserWithPermissions(userName),
                _cacheTimeout);
        }

        private PermissionDescriptor[] LoadAllPermissions()
        {
            var permissions = _manifestProvider.GetModuleManifests().Values
                .Where(m => m.Permissions != null)
                .SelectMany(m => m.Permissions)
                .Select(ConvertToPermissionDescriptor)
                .ToArray();
            return permissions;
        }

        private UserWithPermissions LoadUserWithPermissions(string userName)
        {
            var allPermissionIds = GetAllPermissions().Select(p => p.Id).ToArray();
            var user = new UserWithPermissions();

            using (var repository = _securityRepository())
            {
                var account = repository.Accounts
                    .Include(a => a.RoleAssignments.Select(ra => ra.Role.RolePermissions))
                    .FirstOrDefault(a => a.UserName == userName);

                if (account != null)
                {
                    user.IsActive = account.AccountState == (int)AccountState.Approved;
                    user.RegisterType = (RegisterType)account.RegisterType;
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

            public bool IsActive { get; set; }
            public RegisterType RegisterType { get; set; }
            public string[] StoredPermissionIds { get; set; }
            public string[] ActivePermissionIds { get; set; }

            #endregion
        }
    }
}
