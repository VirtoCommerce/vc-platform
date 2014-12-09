using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Binders
{
	/// <summary>
	/// Class SearchCriteriaBinder.
	/// </summary>
	public class CatalogItemSearchCriteriaBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(CatalogItemSearchCriteria))
			{
				return false;
			}

			var key = actionContext.Request.RequestUri.Query;
			var qs = HttpUtility.ParseQueryString(key);

			// parse facets
			// var facets = qs.Where(k => FacetRegex.IsMatch(k.Key)).Select(k => k.WithKey(FacetRegex.Replace(k.Key, ""))).ToDictionary(x => x.Key, y => y.Value.Split(','));

			// parse facets
			// var terms = qsDict.Where(k => TermRegex.IsMatch(k.Key)).Select(k => k.WithKey(TermRegex.Replace(k.Key, ""))).ToDictionary(x => x.Key, y => y.Value.Split(','));

			var result = new CatalogItemSearchCriteria
			{
			    SearchPhrase = qs["q"].EmptyToNull(),
			    RecordsToRetrieve = qs["take"].TryParse(20),
			    StartingRecord = qs["skip"].TryParse(0)
			};


			// apply vendor filter if one specified
			//if (parameters.Terms != null && parameters.Terms.Count > 0)
			//{
			//	foreach (var term in parameters.Terms)
			//	{
			//		var termFilter = new AttributeFilter()
			//		{
			//			Key = term.Key,
			//			Values = term.Value.Select(x => new AttributeFilterValue() { Id = x.ToLowerInvariant(), Value = x.ToLowerInvariant() }).ToArray()
			//		};

			//		criteria.Apply(termFilter);
			//	}
			//}


		    result.ClassTypes.Add("Product");

		    var startDateFromStr = qs["startdatefrom"].EmptyToNull();

            if (!string.IsNullOrWhiteSpace(startDateFromStr))
		    {
                DateTime startDateFrom;

                if (DateTime.TryParse(startDateFromStr, out startDateFrom))
		        {
                    result.StartDateFrom = startDateFrom;
		        }
		    }

			result.Pricelists = null;

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

            SearchSort sortObject;

            switch (sort.ToLowerInvariant())
            {
                case "price":
                    sortObject = new SearchSort("price", isDescending);
                    break;
                case "position":
                    sortObject = new SearchSort(new SearchSortField(string.Format("sort{0}{1}", catalogId, categoryId).ToLower())
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
                    sortObject = CatalogItemSearchCriteria.DefaultSortOrder;
                    break;

            }

            result.Sort = sortObject;

            //Use fuzzy search to allow spelling error tolerance
		    result.IsFuzzySearch = true;


			bindingContext.Model = result;
			return true;
		}
	}
}
