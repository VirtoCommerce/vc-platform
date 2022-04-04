using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public static class IPermissionsRegistrarExtensions
    {
        public static IPermissionsRegistrar RegisterPermissions(this IPermissionsRegistrar registrar, string moduleId, string groupName, IEnumerable<string> permissions)
        {
            registrar.RegisterPermissions(permissions
                .Select(x => new Permission
                {
                    ModuleId = moduleId,
                    GroupName = groupName,
                    Name = x,
                })
                .ToArray());

            return registrar;
        }

        public static IPermissionsRegistrar WithAvailabeScopesForPermission(this IPermissionsRegistrar registrar, string permissionName, params PermissionScope[] scopes)
        {
            return registrar.WithAvailabeScopesForPermissions(new[] { permissionName }, scopes);
        }

        public static IPermissionsRegistrar WithAvailabeScopesForPermissions(this IPermissionsRegistrar registrar, string[] permissionNames, params PermissionScope[] scopes)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }
            if (permissionNames == null)
            {
                throw new ArgumentNullException(nameof(permissionNames));
            }

            var permissions = registrar.GetAllPermissions().Where(x => permissionNames.Contains(x.Name));
            foreach (var permission in permissions)
            {
                permission.AvailableScopes.AddRange(scopes.Distinct());
            }

            return registrar;
        }
    }
}
