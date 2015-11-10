using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PriceConverter
    {
        public static Price ToWebModel(this VirtoCommercePricingModuleWebModelPrice price)
        {
            var retVal = new Price();
            retVal.InjectFrom(price);
            return retVal;
        }
    }
}