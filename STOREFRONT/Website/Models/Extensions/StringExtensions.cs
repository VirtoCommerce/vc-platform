#region
using System;
using System.Globalization;
using System.Web;

#endregion

namespace VirtoCommerce.Web.Models.Extensions
{
    public static class StringExtensions
    {
        #region Public Methods and Operators
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

        public static string ToAbsoluteUrl(this string relative)
        {
            if (relative == null) return null;

            return relative.StartsWith("~") ? VirtualPathUtility.ToAbsolute(relative) : relative;
        }
        #endregion
    }
}