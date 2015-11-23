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
            retVal.ListPrice = new Money((decimal)(price.List ?? 0), price.Currency);
            retVal.SalePrice = new Money((decimal)(price.Sale ?? 0), price.Currency);
            retVal.ActiveDiscount = new Discount { DiscountAmount = new Money(0, price.Currency) };
            return retVal;
        }
    }
}