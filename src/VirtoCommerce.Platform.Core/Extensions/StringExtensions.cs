using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class StringExtensions
    {

        public static bool IsAbsoluteUrl(this string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(url);
            }
            var shemes = new string[] { Uri.UriSchemeFile, Uri.UriSchemeFtp, Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto, Uri.UriSchemeNetPipe, Uri.UriSchemeNetTcp };
            var retVal = shemes.Any(x => url.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
            return retVal;
        }

        public static decimal TryParse(this string u, Decimal defaultValue)
        {
            var retVal = defaultValue;

            if (!string.IsNullOrEmpty(u))
            {
                decimal.TryParse(u, NumberStyles.Any, CultureInfo.InvariantCulture, out retVal);
            }

            return retVal;

        }

        public static bool TryParse(this string u, bool defaultValue)
        {
            var retVal = defaultValue;

            if (!string.IsNullOrEmpty(u))
            {
                bool.TryParse(u, out retVal);
            }

            return retVal;

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
            return string.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }

        /// <summary>
        /// Equals invariant
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <returns></returns>
        public static bool EqualsInvariant(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }


        public static string GetCurrencyName(this string isoCurrencySymbol)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture && c.LCID != 127)
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
                return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + suffix;
        }

        public static string EscapeSearchTerm(this string term)
        {
            char[] specialCharacters = { '+', '-', '!', '(', ')', '{', '}', '[', ']', '^', '"', '~', '*', '?', ':', '\\' };
            var retVal = new StringBuilder("");
            //'&&', '||',
            foreach (var ch in term)
            {
                if (specialCharacters.Any(x => x == ch))
                {
                    retVal.Append("\\");
                }
                retVal.Append(ch);
            }
            retVal = retVal.Replace("&&", @"\&&");
            retVal = retVal.Replace("||", @"\||");

            return retVal.ToString().Trim();
        }

        /// <summary>
        /// Escapes the selector. Query requires special characters to be escaped in query
        /// http://api.jquery.com/category/selectors/
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        public static string EscapeSelector(this string attribute)
        {
            return Regex.Replace(attribute, string.Format("([{0}])", "/[!\"#$%&'()*+,./:;<=>?@^`{|}~\\]"), @"\\$1");
        }

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space   
            str = str.Substring(0, str.Length <= 240 ? str.Length : 240).Trim(); // cut and trim it   
            str = Regex.Replace(str, @"\s", "-"); // hyphens   

            return str;
        }

        /// <summary>
        /// Only english characters,
        /// Numbers are allowed,  
        /// Dashes are allowed, 
        /// Spaces are replaced by dashes, 
        /// Nothing else is allowed,
        /// Possibly you could replace ;amp by "-and-"
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string MakeFileNameWebSafe(this string fileName)
        {
            string str = fileName.RemoveAccent().ToLower();
            str = str.Replace("&", "-and-");
            str = Regex.Replace(str, @"[^A-Za-z0-9_\-. ]", ""); // invalid chars           
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one dash
            str = str.Substring(0, str.Length <= 240 ? str.Length : 240).Trim(); // cut and trim it   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int ComputeLevenshteinDistance(this string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            var result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    var conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv?.ConvertFromInvariantString(s);
                }
            }
            catch
            {
                // Return default value in case of exception.
            }

            return result;
        }

        public static string[] LeftJoin(this IEnumerable<string> left, IEnumerable<string> right, string delimiter)
        {
            if (right == null)
            {
                right = Enumerable.Empty<string>();
            }

            return left.Join(right.DefaultIfEmpty(String.Empty), x => true, y => true, (x, y) => String.Join(delimiter, new[] { x, y }.Where(z => !String.IsNullOrEmpty(z)))).ToArray();
        }

        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentException("input");
            }
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

    }
}
