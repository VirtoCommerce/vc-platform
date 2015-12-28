﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Filters
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

   
        #endregion

        #region Methods
        private static CultureInfo[] CreateCultures()
        {
            return CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        }
        #endregion
    }
}
