using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.Web.Client.Extensions
{
    public static class UrlHelperFacetExtensions
    {
        public static string FacetPrefix = "f_";

        public static string GetFacetKey(this UrlHelper helper, string field)
        {
            return string.Concat(FacetPrefix, field);
        }

        public static string SetFacet(this UrlHelper helper, string field, string value)
        {
            return helper.SetFacet(helper.RequestContext.HttpContext.Request.RawUrl, field, value);
        }

        public static string SetFacet(this UrlHelper helper, string url, string field, string value)
        {
            return helper.SetParameters(url, new Dictionary<string, object> {
                {helper.GetFacetKey(field), value},
                {"p", 1}
            });
        }
     
        public static string RemoveFacet(this UrlHelper helper, string field)
        {
            var noFacet = helper.RemoveParametersUrl(helper.RequestContext.HttpContext.Request.RawUrl, helper.GetFacetKey(field));
            return helper.SetParameter(noFacet, "p", "1");
        }

        public static string RemoveAllFacets(this UrlHelper helper)
        {
            string url = helper.RequestContext.HttpContext.Request.RawUrl;
            var parts = url.Split('?');
            IDictionary<string, string> qs = new Dictionary<string, string>();
            if (parts.Length > 1)
                qs = ParseQueryString(parts[1]);

            List<string> keysToRemove = new List<string>();
            foreach (var p in qs)
            {
                if (p.Key.StartsWith(FacetPrefix))
                    keysToRemove.Add(p.Key);
            }

            foreach (var p in keysToRemove)
            {
                qs.Remove(p);
            }

            var noFacets = parts[0] + "?" + DictToQuerystring(qs);
            return helper.SetParameter(noFacets, "p", "1");
        }

        /// <summary>
        /// Sets/changes an url's query string parameter.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="key">Query string parameter key to set/change</param>
        /// <param name="value">Query string parameter value</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameter(this UrlHelper helper, string url, string key, string value)
        {
            return helper.SetParameters(url, new Dictionary<string, object> {
                {key, value}
            });
        }

        /// <summary>
        /// Sets/changes an url's query string parameters.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="parameters">Paramteres to set/change</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameters(this UrlHelper helper, string url, IDictionary<string, object> parameters)
        {
            var parts = url.Split('?');
            IDictionary<string, string> qs = new Dictionary<string, string>();
            if (parts.Length > 1)
                qs = ParseQueryString(parts[1]);
            foreach (var p in parameters)
                qs[p.Key] = p.Value.ToNullOrString();
            return parts[0] + "?" + DictToQuerystring(qs);
        }

        /// <summary>
        /// Sets/changes an url's query string parameters.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static string SetQueryParameters(this UrlHelper helper, string query, IDictionary<string, object> parameters)
        {
            var qs = ParseQueryString(query);
            foreach (var p in parameters)
            {
                qs[p.Key] = p.Value.ToNullOrString();
            }
            return DictToQuerystring(qs);
        }

        /// <summary>
        /// Removes parameters from an url's query string
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">URL to process</param>
        /// <param name="parameters">Query string parameter keys to remove</param>
        /// <returns>Resulting URL</returns>
        public static string RemoveParametersUrl(this UrlHelper helper, string url, params string[] parameters)
        {
            var parts = url.Split('?');
            IDictionary<string, string> qs = new Dictionary<string, string>();
            if (parts.Length > 1)
                qs = ParseQueryString(parts[1]);
            foreach (var p in parameters)
                qs.Remove(p);
            return parts[0] + "?" + DictToQuerystring(qs);
        }

        public static string RemoveParameters(this UrlHelper helper, params string[] parameters)
        {
            return helper.RemoveParametersUrl(helper.RequestContext.HttpContext.Request.RawUrl, parameters);
        }

        public static string DictToQuerystring(IDictionary<string, string> qs)
        {
            return string.Join("&", qs
                .Where(k => !string.IsNullOrEmpty(k.Key))
                .Select(k => string.Format("{0}={1}", HttpUtility.UrlEncode(k.Key), HttpUtility.UrlEncode(k.Value))).ToArray());
        }

        /// <summary>
        /// Sets/changes a single parameter from the current query string.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key">Parameter key</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameter(this UrlHelper helper, string key, object value)
        {
            return helper.SetParameter(helper.RequestContext.HttpContext.Request.RawUrl, key, value.ToNullOrString());
        }

        /// <summary>
        /// Sets/changes the current query string's parameters, using <paramref name="parameterDictionary"/> as dictionary
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="parameterDictionary">Parameters to set/change</param>
        /// <returns>Resulting URL</returns>
        public static string SetParameters(this UrlHelper helper, object parameterDictionary)
        {
            return helper.SetParameters(helper.RequestContext.HttpContext.Request.RawUrl, parameterDictionary.ToPropertyDictionary());
        }

        /// <summary>
        /// Parses a query string. If duplicates are present, the last key/value is kept.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ParseQueryString(string s)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (s == null)
                return d;
            if (s.StartsWith("?"))
                s = s.Substring(1);
            foreach (var kv in s.Split('&'))
            {
                var v = kv.Split('=');
                if (string.IsNullOrEmpty(v[0]))
                    continue;
                d[HttpUtility.UrlDecode(v[0])] = HttpUtility.UrlDecode(v[1]);
            }
            return d;
        }

    }
}