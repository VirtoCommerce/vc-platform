using System;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using dataModel = VirtoCommerce.Platform.Data.Model;
using webModel = VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Converters.Security
{
    public static class ApiAccountConverter
    {

        public static dataModel.ApiAccountEntity ToFoundation(this webModel.ApiAccount account)
        {
            var result = new dataModel.ApiAccountEntity();
            result.InjectFrom(account);

            if (account.Id != null)
            {
                result.Id = account.Id;
            }

            if (account.IsActive != null)
            {
                result.IsActive = account.IsActive.Value;
            }

            result.ApiAccountType = (dataModel.ApiAccountType)account.ApiAccountType;

            return result;
        }

        public static webModel.ApiAccount ToWebModel(this dataModel.ApiAccountEntity account)
        {
            var result = new webModel.ApiAccount();
            result.InjectFrom(account);

            result.Id = account.Id;
            result.ApiAccountType = (webModel.ApiAccountType)account.ApiAccountType;

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

            var patchInjection = new PatchInjection<dataModel.ApiAccountEntity>(x => x.ApiAccountType, x => x.IsActive, x => x.SecretKey, x => x.AppId);
            target.InjectFrom(patchInjection, source);
        }


    }
}