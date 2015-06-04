using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Web.Binders
{
	/// <summary>
	/// Class SearchCriteriaBinder.
	/// </summary>
	public class ListEntrySearchCriteriaBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(ListEntrySearchCriteria))
			{
				return false;
			}
			
			var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

			var result = new ListEntrySearchCriteria();

			result.ResponseGroup = ResponseGroup.WithCatalogs | ResponseGroup.WithCategories | ResponseGroup.WithProducts;

			var respGroup = qs["respGroup"].EmptyToNull();
			if(respGroup != null)
			{
				result.ResponseGroup = (ResponseGroup)Enum.Parse(typeof(ResponseGroup), respGroup, true);
			}
			result.Keyword = qs["q"].EmptyToNull();
		
			result.CatalogId = qs["catalog"].EmptyToNull();
			result.CategoryId = qs["category"].EmptyToNull();
			result.Count = qs["count"].TryParse(20);
			result.Start = qs["start"].TryParse(0);
			bindingContext.Model = result;
			return true;
		}
	}
}
