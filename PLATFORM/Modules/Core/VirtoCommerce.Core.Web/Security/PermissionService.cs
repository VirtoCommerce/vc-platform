using System;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Foundation.Security.Repositories;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Security;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Security
{
    public class PermissionService : CachingSecurityService, IPermissionService
    {
        #region Fields

        private readonly IModuleManifestProvider _manifestProvider;
        private readonly Func<ISecurityRepository> _securityRepository;

        #endregion

        #region Constructors and Destructors

        public PermissionService(Func<ISecurityRepository> securityRepository, ICacheRepository cacheRepository, IModuleManifestProvider manifestProvider, ISettingsManager settingsManager)
            : base(cacheRepository, settingsManager)
        {
            _securityRepository = securityRepository;
            _manifestProvider = manifestProvider;
        }

        #endregion

        #region Public Methods and Operators

        public PermissionDescriptor[] GetAllPermissions()
        {
            return Cache.Get(
                Cache.CreateKey("AllPermissions"),
                LoadAllPermissions,
                GetCacheTimeout());
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

        private static PermissionDescriptor ConvertToPermissionDescriptor(ModulePermission permission, string moduleId)
        {
            return new PermissionDescriptor
            {
                Id = permission.Id,
                Name = permission.Name,
                ModuleId = moduleId
            };
        }

        private UserWithPermissions GetUserWithPermissions(string userName)
        {
            return Cache.Get(
                Cache.CreateKey("UserWithPermissions", userName),
                () => LoadUserWithPermissions(userName),
                GetCacheTimeout());
        }

        private PermissionDescriptor[] LoadAllPermissions()
        {
            var permissions = _manifestProvider.GetModuleManifests().Values
                .Where(m => m.Permissions != null)
                .SelectMany(m => m.Permissions.Select(p => ConvertToPermissionDescriptor(p, m.Id)))
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

            public string[] ActivePermissionIds { get; set; }

            public bool IsActive { get; set; }
            public RegisterType RegisterType { get; set; }
            public string[] StoredPermissionIds { get; set; }

            #endregion
        }
    }
}
