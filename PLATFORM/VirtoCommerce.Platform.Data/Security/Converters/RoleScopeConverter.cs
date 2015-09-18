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
    public static class RoleScopeConverter
    {
        public static RoleScope ToCoreModel(this dataModel.RoleScopeEntity source)
        {
            var result = new RoleScope();
            result.InjectFrom(source);

            return result;
        }


        public static dataModel.RoleScopeEntity ToDataModel(this RoleScope source)
        {
            var result = new dataModel.RoleScopeEntity();
            result.InjectFrom(source);

            return result;
        }

   
    }
}
