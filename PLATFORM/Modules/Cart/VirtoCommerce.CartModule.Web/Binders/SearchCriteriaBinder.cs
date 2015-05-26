using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CartModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.CartModule.Web.Binders
{

	public class SearchCriteriaBinder : IModelBinder
	{

		#region IModelBinder Members

		public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(SearchCriteria))
			{
				return false;
			}

			var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

			var result = new SearchCriteria();

			result.Keyword = qs["q"].EmptyToNull();

			result.StoreId = qs["site"].EmptyToNull();
			result.CustomerId = qs["customer"].EmptyToNull();
			result.Count = qs["count"].TryParse(20);
			result.Start = qs["start"].TryParse(0);
			bindingContext.Model = result;
			return true;
		}

		#endregion
	}
}