using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CoreModule.Data.Converters
{
    public static class CurrencyConverter
    {

        public static coreModel.Currency ToCoreModel(this dataModel.Currency currency)
        {
            var retVal = new coreModel.Currency();
            retVal.InjectFrom(currency);
            return retVal;
        }

        public static dataModel.Currency ToDataModel(this coreModel.Currency currency)
        {
            var retVal = new dataModel.Currency();
            retVal.InjectFrom(currency);
            return retVal;
        }


        /// <summary>
        /// Patch changes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Patch(this dataModel.Currency source, dataModel.Currency target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            var patchInjection = new PatchInjection<dataModel.Currency>(x => x.Name,
                                                                        x => x.Symbol, x => x.ExchangeRate, x => x.IsPrimary,
                                                                        x => x.CustomFormatting);
            target.InjectFrom(patchInjection, source);
        }


    }
}