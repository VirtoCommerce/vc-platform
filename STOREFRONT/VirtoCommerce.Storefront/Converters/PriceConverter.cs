using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PriceConverter
    {
        public static ProductPrice ToWebModel(this VirtoCommercePricingModuleWebModelPrice price)
        {
            var currency = new Currency(EnumUtility.SafeParse<CurrencyCodes>(price.Currency, CurrencyCodes.USD));
            var retVal = new ProductPrice(currency);
            retVal.InjectFrom(price);
            retVal.Currency = currency;
            retVal.ListPrice = new Money((decimal)(price.List ?? 0), price.Currency);
            retVal.SalePrice = price.Sale == null ? retVal.ListPrice : new Money((decimal)price.Sale, price.Currency);
            retVal.ActiveDiscount = new Discount { Amount = new Money(0, price.Currency) };
            return retVal;
        }
    }
}