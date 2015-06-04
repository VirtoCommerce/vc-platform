#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.ApiClient.Extensions
{
    internal static class QueryExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets an HTTP query string serialization of this query compatible with FromQueryString
        /// </summary>
        public static string GetQueryString(this BrowseQuery query, object additionalParameters = null)
        {
            var parts = new List<string>();

            if (query.Skip != null)
            {
                parts.Add("skip=" + HttpUtility.UrlEncode(query.Skip.Value.ToString(CultureInfo.InvariantCulture)));
            }

            if (query.Take != null)
            {
                parts.Add("take=" + HttpUtility.UrlEncode(query.Take.Value.ToString(CultureInfo.InvariantCulture)));
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

            if (!string.IsNullOrEmpty(query.Outline))
            {
                parts.Add("outline=" + HttpUtility.UrlEncode(query.Outline));
            }

            if (query.StartDateFrom.HasValue)
            {
                parts.Add(
                    "startdatefrom="
                        + HttpUtility.UrlEncode(query.StartDateFrom.Value.ToString(CultureInfo.InvariantCulture)));
            }

            if (query.Filters != null && query.Filters.Count > 0)
            {
                parts.AddRange(
                    query.Filters.Select(
                        filter => String.Format("t_{0}={1}", filter.Key, String.Join(",", filter.Value))));
            }

            if (query.PriceLists != null && query.PriceLists.Length > 0)
            {
                parts.Add(String.Format("pricelists={0}", String.Join(",", query.PriceLists)));
            }

            if (additionalParameters != null)
            {
                var parameters = additionalParameters.ToPropertyDictionary();
                parts.AddRange(parameters.Select(parameter => String.Format("{0}={1}", parameter.Key, parameter.Value)));
            }

            return string.Join("&", parts);
        }

        public static string GetQueryString(this TagQuery query, object additionalParameters = null)
        {
            var parts = new List<string>();

            if (query.Names != null)
            {
                parts.AddRange(
                    from name in query.Names
                    let value = query[name]
                    select "tags=" + HttpUtility.UrlEncode(name) + ":" + HttpUtility.UrlEncode(value.ToString()));
            }

            if (additionalParameters != null)
            {
                var parameters = additionalParameters.ToPropertyDictionary();
                parts.AddRange(parameters.Select(parameter => String.Format("{0}={1}", parameter.Key, parameter.Value)));
            }

            return string.Join("&", parts);
        }

        #endregion
    }
}
