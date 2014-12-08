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

			string key = actionContext.Request.RequestUri.Query as string;

			var qs = HttpUtility.ParseQueryString(key);

			// parse facets
			// var facets = qs.Where(k => FacetRegex.IsMatch(k.Key)).Select(k => k.WithKey(FacetRegex.Replace(k.Key, ""))).ToDictionary(x => x.Key, y => y.Value.Split(','));

			// parse facets
			// var terms = qsDict.Where(k => TermRegex.IsMatch(k.Key)).Select(k => k.WithKey(TermRegex.Replace(k.Key, ""))).ToDictionary(x => x.Key, y => y.Value.Split(','));

			var result = new CatalogItemSearchCriteria();


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

			
			result.SearchPhrase = qs["q"].EmptyToNull();
			result.ClassTypes.Add("Product");
			result.RecordsToRetrieve = qs["take"].TryParse(20);
			result.StartingRecord = qs["skip"].TryParse(0);
			result.Pricelists = null;

		    var sortQuery = qs["sort"].EmptyToNull();
            var sort = string.IsNullOrEmpty(sortQuery) ? "name" : sortQuery;
		    var sortOrder = qs["sortorder"].EmptyToNull();

            var isDescending = "desc".Equals(sortOrder, StringComparison.OrdinalIgnoreCase);

            SearchSort sortObject = null;

            switch (sort.ToLowerInvariant())
            {
                case "price":
                    sortObject = new SearchSort("price", isDescending);
                    break;
                case "position":
                    //TODO: need to get catalogId and categoryId
                    //sortObject = new SearchSort(new SearchSortField(string.Format("sort{0}{1}", session.CatalogId, session.CategoryId).ToLower())
                    //{
                    //    IgnoredUnmapped = true,
                    //    IsDescending = isDescending
                    //});
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


			bindingContext.Model = result;
			return true;
		}
	}
}
