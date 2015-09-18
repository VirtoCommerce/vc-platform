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
    public static class PermissionConverter
    {
        public static Permission ToCoreModel(this dataModel.PermissionEntity source)
        {
            var result = new Permission();
            result.InjectFrom(source);

            return result;
        }

        public static Permission ToCoreModel(this ModulePermission source, string moduleId, string groupName)
        {
            return new Permission
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description,
                ModuleId = moduleId,
                GroupName = groupName,
            };
        }

        public static dataModel.PermissionEntity ToDataModel(this Permission source)
        {
            var result = new dataModel.PermissionEntity();
            result.InjectFrom(source);
            return result;
        }

        public static void Patch(this dataModel.PermissionEntity source, dataModel.PermissionEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.PermissionEntity>(x => x.Name, x => x.Description);
            target.InjectFrom(patchInjection, source);
        }
    }
}
