using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CurrencyConverter
    {
        public static Currency ToWebModel(this VirtoCommerceDomainCommerceModelCurrency currency)
        {
            var retVal = new Currency(currency.Code, currency.Name, currency.Symbol, (decimal)currency.ExchangeRate.Value);
            return retVal;
        }
    }
}