using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.MerchandisingModule.Web2.Model;

namespace VirtoCommerce.MerchandisingModule.Web2.Binders
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

			var outline = qs["outline"].EmptyToNull();
			if (outline != null)
			{
				result.Outlines.Add(String.Format("{0}*", outline));
			}
			result.SearchPhrase = qs["q"].EmptyToNull();
			//result.ClassTypes.Add("Product");
			result.RecordsToRetrieve = qs["take"].TryParse(20);
			result.StartingRecord = qs["skip"].TryParse(1);
			result.Pricelists = null;


			bindingContext.Model = result;
			return true;
		}
	}
}
