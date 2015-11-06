using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotLiquid;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Filters
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/filters/money-filters
    /// </summary>
    public class MoneyFilters
    {
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
            var themeEngine = (ShopifyLiquidThemeEngine)Template.FileSystem;
            var amount = Convert.ToDecimal(input, CultureInfo.InvariantCulture);
            var money = new Money(amount, themeEngine.WorkContext.CurrentCurrency.Code);

            return money.ToString();
        }
    }
}
