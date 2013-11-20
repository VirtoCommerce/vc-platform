using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Web.Helpers
{
    public static class StringExtensions
    {
        public static int TryParse(this string u, int defaultValue)
        {
            try
            {
                return int.Parse(u);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int TryParse(this string u)
        {
            return TryParse(u, 0);
        }

        /// <summary>
        /// Like null coalescing operator (??) but including empty strings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string IfNullOrEmpty(this string a, string b)
        {
            return string.IsNullOrEmpty(a) ? b : a;
        }

        /// <summary>
        /// If <paramref name="a"/> is empty, returns null
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string EmptyToNull(this string a)
        {
            return string.IsNullOrEmpty(a) ? null : a;
        }

		/// <summary>
		/// Equalses the or null empty.
		/// </summary>
		/// <param name="str1">The STR1.</param>
		/// <param name="str2">The STR2.</param>
		/// <param name="comparisonType">Type of the comparison.</param>
		/// <returns></returns>
		public static bool EqualsOrNullEmpty(this string str1, string str2, StringComparison comparisonType)
		{
			return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
		}

        public static string GetCurrencyName(this string isoCurrencySymbol)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == isoCurrencySymbol)
                .Select(ri => ri.CurrencyNativeName)
                .FirstOrDefault() ?? isoCurrencySymbol;
        }
    }
}