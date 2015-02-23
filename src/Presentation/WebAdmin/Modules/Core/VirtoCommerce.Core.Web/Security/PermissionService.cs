using System;
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
        private readonly Func<ISecurityRepository> _securityRepository;
        private readonly IModuleManifestProvider _manifestProvider;

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
            var permissions = _manifestProvider.GetModuleManifests().Values
                .SelectMany(m => m.Permissions)
                .Select(ConvertToPermissionDescriptor)
                .ToArray();
            return permissions;
        }

        private PermissionDescriptor ConvertToPermissionDescriptor(ModulePermission permission)
        {
            return new PermissionDescriptor
            {
                Id = permission.Id,
                Name = permission.Name
            };
        }

        public bool UserHasAnyPermission(string userName, params string[] permissionIds)
        {
            var user = GetUserWithPermissions(userName);
            var success = user.RegisterType == RegisterType.Administrator;

            if (!success)
            {
                success = user.PermissionIds
                    .Intersect(permissionIds, StringComparer.OrdinalIgnoreCase)
                    .Any();
            }

            return success;
        }

        #endregion

        #region Methods

        private UserWithPermissions GetUserWithPermissions(string userName)
        {
            return _cache.Get(
                CacheHelper.CreateCacheKey("UserWithPermissionsCache", userName),
                () => LoadUserWithPermissions(userName),
                _cacheTimeout);
        }

        private UserWithPermissions LoadUserWithPermissions(string userName)
        {
            using (var repository = _securityRepository())
            {
                var accountQuery = repository.Accounts.Where(a => a.UserName == userName);
                var registerType = (RegisterType)accountQuery
                    .Select(a => a.RegisterType)
                    .FirstOrDefault();

                var user = new UserWithPermissions
                {
                    RegisterType = registerType,
                };

                if (user.RegisterType != RegisterType.Administrator)
                {
                    user.PermissionIds = accountQuery
                        .SelectMany(a => a.RoleAssignments)
                        .Select(ra => ra.Role)
                        .SelectMany(r => r.RolePermissions)
                        .Select(rp => rp.PermissionId)
                        .ToArray();
                }

                return user;
            }
        }

        #endregion

        private class UserWithPermissions
        {
            #region Public Properties

            public string[] PermissionIds { get; set; }
            public RegisterType RegisterType { get; set; }

            #endregion
        }
    }
}
