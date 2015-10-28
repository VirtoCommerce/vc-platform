using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/money-filters
    /// </summary>
    public class MoneyFilters
    {
        private static readonly Lazy<CultureInfo[]> _cultures = new Lazy<CultureInfo[]>(
           CreateCultures,
           LazyThreadSafetyMode.ExecutionAndPublication);

        private static CultureInfo[] CreateCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }


        /// <summary>
        /// Formats the price based on the shop's HTML without currency setting.
        /// {{ 145 | money }}
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Money(object input)
        {
            if (input == null)
            {
                return null;
            }
            var themeAdaptor = (ShopifyLiquidThemeStructure)Template.FileSystem;
            var value = Convert.ToDecimal(input, CultureInfo.InvariantCulture);

            var currencyCulture = _cultures.Value.FirstOrDefault(x => new RegionInfo(x.Name).ISOCurrencySymbol.Equals(themeAdaptor.WorkContext.CurrentCurrency, StringComparison.OrdinalIgnoreCase));

            return value.ToString("C", currencyCulture.NumberFormat);
        }
    }
}
