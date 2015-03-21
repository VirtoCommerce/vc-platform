using System;

namespace VirtoCommerce.ApiWebClient.Currencies
{
    public interface ICurrencyService
    {
        String FormatCurrency(decimal amount, string currencyCode);
        decimal ConvertCurrency(decimal amount, string currencyFrom, ref string currencyTo);
    }
}
