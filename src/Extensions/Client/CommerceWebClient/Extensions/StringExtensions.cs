using System.Web.Mvc;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Localizes the HTML.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <returns>localized MvcHtmlString.</returns>
        public static MvcHtmlString LocalizeHtml(this string source, params object[] format)
        {
            return new MvcHtmlString(string.Format(source.Localize((string)null), format));
        }

    }
}