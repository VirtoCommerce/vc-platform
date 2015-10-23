using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DotLiquid.ViewEngine.Extensions
{
    public static class StringExtensions
    {
        public static string TrimQuotes(this string input)
        {
            return input.Trim('\'', '\"');
        }

        public static CultureInfo TryGetCultureInfo(this string languageCode)
        {
            try
            {
                return !string.IsNullOrEmpty(languageCode) ? CultureInfo.CreateSpecificCulture(languageCode) : null;
            }
            catch
            {
                return null;
            }
        }
    }
}
