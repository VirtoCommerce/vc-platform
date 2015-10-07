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
        public static Permission ToCoreModel(this dataModel.RolePermissionEntity source, IPermissionScopeService scopeService)
        {
            var result = new Permission();
            result.InjectFrom(source.Permission);
            result.AssignedScopes = source.Scopes.Select(x => new { source = x, target = scopeService.GetScopeByTypeName(x.Type) })
                                                  .Where(x=> x.target != null)
                                                  .Select(x=> x.source.ToCoreModel(x.target))
                                                  .ToArray();
            result.AvailableScopes = scopeService.GetAvailablePermissionScopes(result.Id).ToArray();
            return result;
        }

     
        public static dataModel.RolePermissionEntity ToRolePemissionDataModel(this Permission source)
        {
            var result = new dataModel.RolePermissionEntity();
            result.PermissionId = source.Id;
            if (source.AssignedScopes != null)
            {
                result.Scopes = new ObservableCollection<dataModel.PermissionScopeEntity>(source.AssignedScopes.Where(x=>!String.IsNullOrEmpty(x.Scope)).Select(x => x.ToDataModel()));
            }
            return result;
        }

        public static void Patch(this dataModel.RolePermissionEntity source, dataModel.RolePermissionEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            if (!source.Scopes.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((dataModel.PermissionScopeEntity x) => x.Scope);
                source.Scopes.Patch(target.Scopes, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
