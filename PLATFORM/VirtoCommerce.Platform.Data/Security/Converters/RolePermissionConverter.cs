using System;
using System.Collections.ObjectModel;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class RolePermissionConverter
    {
        public static Permission ToCoreModel(this dataModel.RolePermissionEntity source)
        {
            var result = new Permission();
            result.InjectFrom(source.Permission);
            result.ScopeBounded = source.ScopeBounded;
            return result;
        }

     
        public static dataModel.RolePermissionEntity ToRolePemissionDataModel(this Permission source)
        {
            var result = new dataModel.RolePermissionEntity();
            result.InjectFrom(source);
            result.PermissionId = source.Id;
            return result;
        }

        public static void Patch(this dataModel.RolePermissionEntity source, dataModel.RolePermissionEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.RolePermissionEntity>(x => x.ScopeBounded);
            target.InjectFrom(patchInjection, source);
        }
    }
}
