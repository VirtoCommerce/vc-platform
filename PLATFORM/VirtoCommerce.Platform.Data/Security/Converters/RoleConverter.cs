using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class RoleConverter
    {
        public static Role ToCoreModel(this dataModel.RoleEntity source)
        {
            var result = new Role();
            result.InjectFrom(source);
  
           result.Permissions = source.RolePermissions.Select(rp => rp.Permission.ToCoreModel()).ToArray();
           result.Scopes = source.Scopes.Select(x => x.Scope).ToArray();

            return result;
        }


        public static dataModel.RoleEntity ToDataModel(this Role source)
        {
            var result = new dataModel.RoleEntity
            {
                Name = source.Name,
                Description = source.Description,
            };

            if (source.Id != null)
                result.Id = source.Id;

            result.RolePermissions = new NullCollection<dataModel.RolePermissionEntity>();

            if (source.Permissions != null)
            {
                result.RolePermissions = new ObservableCollection<dataModel.RolePermissionEntity>(source.Permissions.Select(p => new dataModel.RolePermissionEntity { PermissionId = p.Id }));
            }
            if (source.Scopes != null)
            {
                result.Scopes = new ObservableCollection<dataModel.RoleScopeEntity>(source.Scopes.Select(x => new dataModel.RoleScopeEntity { Scope = x }));
            }
            return result;
        }

        public static dataModel.RoleAssignmentEntity ToAssignmentDataModel(this Role source)
        {
            var result = new dataModel.RoleAssignmentEntity();
            result.RoleId = source.Id;
            return result;
        }

        public static void Patch(this dataModel.RoleEntity source, dataModel.RoleEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.RoleEntity>(x => x.Name, x => x.Description);
            target.InjectFrom(patchInjection, source);

            if (!source.RolePermissions.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((dataModel.RolePermissionEntity rp) => rp.PermissionId);
                source.RolePermissions.Patch(target.RolePermissions, comparer, (sourceItem, targetItem) => { });
            }

            if (!source.Scopes.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((dataModel.RoleScopeEntity x) => x.Scope);
                source.Scopes.Patch(target.Scopes, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
