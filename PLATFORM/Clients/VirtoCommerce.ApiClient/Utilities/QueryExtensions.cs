using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.ApiClient.Utilities
{
    internal static class QueryExtensions
    {
        /// <summary>
        /// Gets an HTTP query string serialization of this query compatible with FromQueryString
        /// </summary>
        public static string GetQueryString(this BrowseQuery query)
        {
            var parts = new List<string>();

            if (query.Skip != null)
            {
                parts.Add("skip=" + HttpUtility.UrlEncode(query.Skip.Value.ToString()));
            }

            if (query.Take != null)
            {
                parts.Add("take=" + HttpUtility.UrlEncode(query.Take.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(query.SortProperty))
            {
                parts.Add("sort=" + HttpUtility.UrlEncode(query.SortProperty));
            }

            if (!string.IsNullOrEmpty(query.SortDirection))
            {
                parts.Add("sortorder=" + HttpUtility.UrlEncode(query.SortDirection));
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                parts.Add("q=" + HttpUtility.UrlEncode(query.Search));
            }

            if (query.Filters != null && query.Filters.Count > 0)
            {
                parts.AddRange(query.Filters.Select(filter => String.Format("t_{0}={1}", filter.Key, String.Join(",", filter.Value))));

                parts.Add("q=" + HttpUtility.UrlEncode(query.Search));
            }

            return string.Join("&", parts);
        }
    }
}
