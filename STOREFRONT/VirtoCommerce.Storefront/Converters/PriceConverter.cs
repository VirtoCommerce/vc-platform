using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PriceConverter
    {
        public static ProductPrice ToWebModel(this VirtoCommercePricingModuleWebModelPrice price)
        {
            var retVal = new ProductPrice();
            retVal.InjectFrom(price);
            retVal.Currency = new Currency(EnumUtility.SafeParse<CurrencyCodes>(price.Currency, CurrencyCodes.USD));
            return retVal;
        }
    }
}