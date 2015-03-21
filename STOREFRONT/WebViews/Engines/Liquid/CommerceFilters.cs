#region
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public class CommerceFilters
    {
        #region Static Fields
        private static readonly Lazy<CultureInfo[]> _cultures = new Lazy<CultureInfo[]>(
            CreateCultures,
            LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Public Properties
        public static CultureInfo[] Cultures
        {
            get
            {
                return _cultures.Value;
            }
        }
        #endregion

        #region Public Methods and Operators
        /// <summary>
        ///     Return the three letter ISO currency code for the current thread.
        /// </summary>
        /// <returns>current currency code in String</returns>
        public static String CurrentCurrencyCode()
        {
            return new RegionInfo(Thread.CurrentThread.CurrentCulture.Name).ISOCurrencySymbol;
        }

        public static string CurrentCurrencyCode(string cultureName)
        {
            return new RegionInfo(cultureName).ISOCurrencySymbol;
        }

        public static string CustomerLoginLink(string input)
        {
            return String.Format("<a href=\"/account/login\" id=\"customer_login_link\">{0}</a>", input);
        }

        public static string CustomerLogoutLink(string input)
        {
            return String.Format("<a href=\"/account/logoff\" id=\"customer_logout_link\">{0}</a>", input);
        }

        public static string CustomerRegisterLink(string input)
        {
            return String.Format("<a href=\"/account/register\" id=\"customer_register_link\">{0}</a>", input);
        }

        /// <summary>
        ///     Return the object which represents the place and language which matches the currency code which
        ///     the database is able to support.  Fall back to Current Thread's culture if the currencyCode we requested doesn't
        ///     match.
        /// </summary>
        /// <param name="currencyCode">the currency code to be matched for the culture</param>
        /// >
        /// <returns>CultureInfo object</returns>
        public static CultureInfo EffectiveCulture(string currencyCode)
        {
            var retVal = CultureInfo.CurrentCulture;

            if (!CurrentCurrencyCode().Equals(currencyCode, StringComparison.OrdinalIgnoreCase))
            {
                // Find currency culture
                var info =
                    Cultures.FirstOrDefault(
                        i =>
                            new RegionInfo(i.Name).ISOCurrencySymbol.Equals(
                                currencyCode,
                                StringComparison.OrdinalIgnoreCase));
                retVal = info ?? retVal;
            }

            //.NET for swiss currency returns Fr where normally it should be CHF
            if (retVal.Name.Equals("de-CH", StringComparison.InvariantCultureIgnoreCase))
            {
                retVal.NumberFormat.CurrencySymbol = "CHF";
            }

            return retVal;
        }

        /// <summary>
        ///     Attempt to format the currency based on the browser's locale, but if that currency
        ///     is not in the database, then fallback to current thread's culture.
        /// </summary>
        /// <param name="amount">the amount to be formated</param>
        /// <param name="currencyCode">
        ///     currency code which will be used to find the
        ///     effective culture
        /// </param>
        /// <returns>Formatted currency in String</returns>
        public static string FormatCurrency(decimal amount, string currencyCode)
        {
            return String.Format(EffectiveCulture(currencyCode), "{0:c}", amount);
        }

        public static string LinkTo(string input, string link)
        {
            return String.Format("<a title=\"A link to {0}\" href=\"{1}\">{0}</a>", input, link);
        }

        //public static string Money(object input)
        //{
        //    if (input == null)
        //    {
        //        return String.Empty;
        //    }

        //    decimal val;

        //    if (input is int)
        //    {
        //        var parsedDecimal = String.Format(
        //            "{0}{1}{2}",
        //            input.ToString().Substring(0, input.ToString().Length - 2),
        //            CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator,
        //            input.ToString().Substring(input.ToString().Length - 2));
        //        val = Decimal.Parse(parsedDecimal);
        //    }
        //    else
        //    {
        //        val = (decimal)input;
        //    }

        //    return FormatCurrency(val, CurrentCurrencyCode());
        //}
        #endregion

        #region Methods
        private static CultureInfo[] CreateCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }
        #endregion
    }
}