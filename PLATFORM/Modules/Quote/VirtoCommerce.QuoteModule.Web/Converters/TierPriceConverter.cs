using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using webModel = VirtoCommerce.QuoteModule.Web.Model;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.QuoteModule.Web.Converters
{
    public static class TierPriceConverter
    {
        public static webModel.TierPrice ToWebModel(this coreModel.TierPrice tierPrice)
        {
            var retVal = new webModel.TierPrice();
            retVal.InjectFrom(tierPrice);
            return retVal;
        }

        public static coreModel.TierPrice ToCoreModel(this webModel.TierPrice tierPrice)
        {
            var retVal = new coreModel.TierPrice();
            retVal.InjectFrom(tierPrice);
            return retVal;
        }


    }
}