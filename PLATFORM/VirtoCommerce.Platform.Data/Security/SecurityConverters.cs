using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security
{
    public static class SecurityConverters
    {
        public static RoleEntity ToFoundation(this RoleDescriptor source)
        {
            var result = new RoleEntity
            {
                Name = source.Name
            };

            if (source.Id != null)
                result.Id = source.Id;

            result.RolePermissions = new NullCollection<RolePermissionEntity>();

            if (source.Permissions != null)
            {
                result.RolePermissions = new ObservableCollection<RolePermissionEntity>(source.Permissions.Select(p => new RolePermissionEntity { PermissionId = p.Id }));
            }

            return result;
        }

        public static PermissionEntity ToFoundation(this PermissionDescriptor source)
        {
            var result = new PermissionEntity
            {
                Id = source.Id,
                Name = source.Name
            };

            return result;
        }

        public static RoleDescriptor ToCoreModel(this RoleEntity source, bool fillPermissions)
        {
            var result = new RoleDescriptor
            {
                Id = source.Id,
                Name = source.Name,
            };

            if (fillPermissions && source.RolePermissions != null)
            {
                result.Permissions = source.RolePermissions.Select(rp => rp.Permission.ToCoreModel()).ToArray();
            }

            return result;
        }

        public static PermissionDescriptor ToCoreModel(this PermissionEntity source)
        {
            var result = new PermissionDescriptor
            {
                Id = source.Id,
                Name = source.Name,
            };

            return result;
        }

        public static void Patch(this RoleEntity source, RoleEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<RoleEntity>(x => x.Name, x => x.Description);
            target.InjectFrom(patchInjection, source);

            if (!source.RolePermissions.IsNullCollection())
            {
                var settingComparer = AnonymousComparer.Create((RolePermissionEntity rp) => rp.Id);
                source.RolePermissions.Patch(target.RolePermissions, settingComparer, (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
            }
        }

        public static void Patch(this RolePermissionEntity source, RolePermissionEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<RolePermissionEntity>(x => x.RoleId, x => x.PermissionId);
            target.InjectFrom(patchInjection, source);
        }

        public static void Patch(this PermissionEntity source, PermissionEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<PermissionEntity>(x => x.Name, x => x.Description);
            target.InjectFrom(patchInjection, source);
        }

        public static void Patch(this RoleAssignmentEntity source, RoleAssignmentEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<RoleAssignmentEntity>(x => x.RoleId, x => x.AccountId);
            target.InjectFrom(patchInjection, source);
        }
    }
}
