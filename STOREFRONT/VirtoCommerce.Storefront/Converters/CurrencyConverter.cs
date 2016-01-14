using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CurrencyConverter
    {
        public static Currency ToWebModel(this VirtoCommerceDomainCommerceModelCurrency currency, Language language)
        {
            var retVal = new Currency(language, currency.Code, currency.Name, currency.Symbol, (decimal)currency.ExchangeRate.Value);
            retVal.CustomFormatting = currency.CustomFormatting;
            return retVal;
        }
    }
}