using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace VirtoCommerce.ApiClient.Extensions
{
    public static class StringExtension
    {
        #region Public Methods and Operators

        /// <summary>
        ///     If <paramref name="a" /> is empty, returns null
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string EmptyToNull(this string a)
        {
            return string.IsNullOrEmpty(a) ? null : a;
        }

        /// <summary>
        ///     Equalses the or null empty.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static bool EqualsOrNullEmpty(this string str1, string str2, StringComparison comparisonType)
        {
            return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }

        public static string EscapeSearchTerm(this string term)
        {
            char[] specialCharcters =
            {
                '+',
                '-',
                '!',
                '(',
                ')',
                '{',
                '}',
                '[',
                ']',
                '^',
                '"',
                '~',
                '*',
                '?',
                ':',
                '\\'
            };
            var retVal = "";
            //'&&', '||',
            foreach (var ch in term)
            {
                if (specialCharcters.Any(x => x == ch))
                {
                    retVal += "\\";
                }
                retVal += ch;
            }
            retVal = retVal.Replace("&&", @"\&&");
            retVal = retVal.Replace("||", @"\||");
            retVal = retVal.Trim();

            return retVal;
        }

        /// <summary>
        ///     Escapes the selector. Query requires special characters to be escaped in query
        ///     http://api.jquery.com/category/selectors/
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public static string EscapeSelector(this string attribute)
        {
            return Regex.Replace(attribute, string.Format("([{0}])", "/[!\"#$%&'()*+,./:;<=>?@^`{|}~\\]"), @"\\$1");
        }

        public static string GetCurrencyName(this string isoCurrencySymbol)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture && c.LCID != 127)
                .Select(
                    culture =>
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

        /// <summary>
        ///     Like null coalescing operator (??) but including empty strings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string IfNullOrEmpty(this string a, string b)
        {
            return string.IsNullOrEmpty(a) ? b : a;
        }

        public static string ToSpecificLangCode(this string lang)
        {
            try
            {
                var culture = CultureInfo.CreateSpecificCulture(lang);
                return culture.Name;
            }
            catch
            {
                return "";
            }
        }

        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + suffix;
        }

        public static int TryParse(this string u, int defaultValue)
        {
            var retVal = defaultValue;

            if (!string.IsNullOrEmpty(u))
            {
                int.TryParse(u, out retVal);
            }

            return retVal;
        }

        public static int? TryParse(this string u, int? defaultValue)
        {
            var result = TryParse(u, defaultValue.HasValue ? defaultValue.Value : int.MinValue);

            if (result == int.MinValue)
            {
                return null;
            }

            return result;
        }

        public static int TryParse(this string u)
        {
            return TryParse(u, 0);
        }

        #endregion
    }
}
