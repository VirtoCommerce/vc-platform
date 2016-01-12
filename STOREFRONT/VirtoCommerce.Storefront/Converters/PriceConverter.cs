using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Marketing;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PriceConverter
    {
        public static ProductPrice ToWebModel(this VirtoCommercePricingModuleWebModelPrice price, IEnumerable<Currency> availCurrencies)
        {
            var currency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(price.Currency)) ?? new Currency(price.Currency, 1);
            var retVal = new ProductPrice(currency);
            retVal.InjectFrom(price);
            retVal.Currency = currency;
            retVal.ListPrice = new Money(price.List ?? 0d, currency);
            retVal.SalePrice = price.Sale == null ? retVal.ListPrice : new Money(price.Sale ?? 0d, currency);
            retVal.ActiveDiscount = new Discount { Amount = new Money(currency) };
            return retVal;
        }
    }
}