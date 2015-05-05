using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MerchandisingModule.Web.Binders
{
    /// <summary>
    ///     Class SearchCriteriaBinder.
    /// </summary>
    public class CatalogItemSearchCriteriaBinder : IModelBinder
    {
        #region Static Fields

        private static readonly Regex FacetRegex = new Regex("^f_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///     The facet regex
        /// </summary>
        private static readonly Regex TermRegex = new Regex("^t_", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Public Methods and Operators

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(CatalogIndexedSearchCriteria))
            {
                return false;
            }

            var key = actionContext.Request.RequestUri.Query;
            var qs = HttpUtility.ParseQueryString(key);
            var qsDict = this.NvToDict(qs);

            // parse facets
            var facets =
                qsDict.Where(k => FacetRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(FacetRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','));

            // parse facets
            var terms =
                qsDict.Where(k => TermRegex.IsMatch(k.Key))
                    .Select(k => k.WithKey(TermRegex.Replace(k.Key, "")))
                    .ToDictionary(x => x.Key, y => y.Value.Split(','));

			var result = new CatalogIndexedSearchCriteria
                         {
                             SearchPhrase = qs["q"].EmptyToNull(),
                             RecordsToRetrieve = qs["take"].TryParse(20),
                             StartingRecord = qs["skip"].TryParse(0),
                         };

            // apply filters if one specified
            if (terms.Count > 0)
            {
                foreach (var term in terms)
                {
                    var termFilter = new AttributeFilter
                                     {
                                         Key = term.Key.ToLowerInvariant(),
                                         Values =
                                             term.Value.Select(
                                                 x =>
                                             new AttributeFilterValue()
                                             {
                                                 Id = x.ToLowerInvariant(),
                                                 Value = x.ToLowerInvariant()
                                             }).ToArray()
                                     };

                    result.Apply(termFilter);
                }
            }

            //result.ClassTypes.Add("Product");

            var startDateFromStr = qs["startdatefrom"].EmptyToNull();

            if (!string.IsNullOrWhiteSpace(startDateFromStr))
            {
                DateTime startDateFrom;

                if (DateTime.TryParse(startDateFromStr, out startDateFrom))
                {
                    result.StartDateFrom = startDateFrom;
                }
            }

            //TODO load pricelists
            result.Pricelists = null;
            result.Currency = qs["curreny"].EmptyToNull();

            var sortQuery = qs["sort"].EmptyToNull();
            var sort = string.IsNullOrEmpty(sortQuery) ? "name" : sortQuery;
            var sortOrder = qs["sortorder"].EmptyToNull();

            var outline = qs["outline"].EmptyToNull();

            var isDescending = "desc".Equals(sortOrder, StringComparison.OrdinalIgnoreCase);

            var catalogId = actionContext.ActionArguments.ContainsKey("catalog")
                ? actionContext.ActionArguments["catalog"]
                : null;

            string categoryId = null;

            if (!string.IsNullOrWhiteSpace(outline))
            {
                categoryId = outline.Split(new[] { '/' }).Last();
            }

            SearchSort sortObject = null;

            switch (sort.ToLowerInvariant())
            {
                case "price":
                    if (result.Pricelists != null)
                    {
                        sortObject = new SearchSort(
                            result.Pricelists.Select(
                                priceList =>
                                    new SearchSortField(
                                        String.Format(
                                            "price_{0}_{1}",
                                            result.Currency.ToLower(),
                                            priceList.ToLower()))
                                    {
                                        IgnoredUnmapped = true,
                                        IsDescending = isDescending,
                                        DataType = SearchSortField.DOUBLE
                                    })
                                .ToArray());
                    }
                    break;
                case "position":
                    sortObject =
                        new SearchSort(
                            new SearchSortField(string.Format("sort{0}{1}", catalogId, categoryId).ToLower())
                            {
                                IgnoredUnmapped = true,
                                IsDescending = isDescending
                            });
                    break;
                case "name":
                    sortObject = new SearchSort("name", isDescending);
                    break;
                case "rating":
                    sortObject = new SearchSort(result.ReviewsAverageField, isDescending);
                    break;
                case "reviews":
                    sortObject = new SearchSort(result.ReviewsTotalField, isDescending);
                    break;
                default:
					sortObject = CatalogIndexedSearchCriteria.DefaultSortOrder;
                    break;
            }

            result.Sort = sortObject;

            //Use fuzzy search to allow spelling error tolerance
            result.IsFuzzySearch = true;

            bindingContext.Model = result;
            return true;
        }

        public IDictionary<string, string> NvToDict(NameValueCollection nv)
        {
            var d = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var k in nv.AllKeys)
            {
                d[k] = nv[k];
            }
            return d;
        }

        #endregion
    }
}
