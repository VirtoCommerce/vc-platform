using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;
using webModel = VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Converters.Security
{
    public static class ApiAccountConverter
    {

        public static dataModel.ApiAccountEntity ToFoundation(this webModel.ApiAccount account)
        {
            var retVal = new dataModel.ApiAccountEntity();
            retVal.InjectFrom(account);

            if (account.Id != null)
            {
                retVal.Id = account.Id;
            }

            return retVal;
        }

        public static webModel.ApiAccount ToWebModel(this dataModel.ApiAccountEntity center)
        {
            var retVal = new webModel.ApiAccount();
            retVal.InjectFrom(center);
            retVal.Id = center.Id;
            return retVal;
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

            var patchInjection = new PatchInjection<dataModel.ApiAccountEntity>(x => x.ApiAccountType, x => x.IsActive, x => x.SecretKey, x => x.AppId);
            target.InjectFrom(patchInjection, source);
        }


    }
}