using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Currencies
{
    public interface ICurrencyService
    {
        String FormatCurrency(decimal amount, string currencyCode);
        decimal ConvertCurrency(decimal amount, string currencyFrom, ref string currencyTo);
    }
}
