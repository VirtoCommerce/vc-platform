using System;
using System.Web;

namespace VirtoCommerce.LiquidThemeEngine.Extensions
{
    public static class UriExtensions
    {
        /// <summary>
        /// Sets the given parameter value in the query string.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name">Name of the parameter to set.</param>
        /// <param name="value">Value for the parameter to set. Pass null to remove the parameter with given name.</param>
        /// <returns>Url with given parameter value.</returns>
        public static Uri SetQueryParameter(this Uri url, string name, string value)
        {
            var query = HttpUtility.ParseQueryString(url.Query);

            if (value != null)
            {
                query[name] = value;
            }
            else
            {
                query.Remove(name);
            }

            var uriBuilder = new UriBuilder(url)
            {
                Query = query.HasKeys() ? query.ToString() : string.Empty
            };

            return uriBuilder.Uri;
        }
    }
}
