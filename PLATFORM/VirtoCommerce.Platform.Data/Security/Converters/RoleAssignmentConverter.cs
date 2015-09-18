using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class RoleAssignmentConverter
    {
        public static Role ToCoreModel(this dataModel.RoleAssignmentEntity entity)
        {
            var result = entity.Role.ToCoreModel();
            result.Scopes = entity.Scopes.Select(x => new RoleScope { Scope = x.Scope, Type = x.Type }).ToArray();

            return result;
        }

        public static void Patch(this dataModel.RoleAssignmentEntity source, dataModel.RoleAssignmentEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.RoleAssignmentEntity>(x => x.RoleId, x => x.AccountId);
            target.InjectFrom(patchInjection, source);

            if (!source.Scopes.IsNullCollection())
            {
                var comparer = AnonymousComparer.Create((dataModel.RoleScopeEntity x) => x.Scope + "-" + x.Type);
                source.Scopes.Patch(target.Scopes, comparer, (sourceItem, targetItem) => { });
            }
        }
    }
}
