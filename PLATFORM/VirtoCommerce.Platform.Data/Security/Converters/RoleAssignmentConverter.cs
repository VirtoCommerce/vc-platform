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
       
        public static void Patch(this dataModel.RoleAssignmentEntity source, dataModel.RoleAssignmentEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.RoleAssignmentEntity>(x => x.RoleId, x => x.AccountId);
            target.InjectFrom(patchInjection, source);

        }
    }
}
