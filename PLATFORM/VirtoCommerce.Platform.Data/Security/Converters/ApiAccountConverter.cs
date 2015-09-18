using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;

namespace VirtoCommerce.Platform.Data.Security.Converters
{
    public static class ApiAccountConverter
    {
        public static ApiAccount ToCoreModel(this dataModel.ApiAccountEntity entity)
        {
            var result = new ApiAccount();
            result.InjectFrom(entity);

            result.ApiAccountType = entity.ApiAccountType;
            result.IsActive = entity.IsActive;

            return result;
        }

        public static dataModel.ApiAccountEntity ToDataModel(this ApiAccount model)
        {
            var result = new dataModel.ApiAccountEntity();
            result.InjectFrom(model);

            if (model.Id != null)
            {
                result.Id = model.Id;
            }

            if (model.IsActive != null)
            {
                result.IsActive = model.IsActive.Value;
            }

            result.ApiAccountType = model.ApiAccountType;

            return result;
        }

        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.ApiAccountEntity source, dataModel.ApiAccountEntity target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            var patchInjection = new PatchInjection<dataModel.ApiAccountEntity>(x => x.Name, x => x.ApiAccountType, x => x.IsActive, x => x.SecretKey, x => x.AppId);
            target.InjectFrom(patchInjection, source);
        }
    }
}
