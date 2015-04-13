using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.CustomerModule.Web.Binders
{
	public class MarketingSearchCriteriaBinder : IModelBinder
	{
		#region IModelBinder Members

		public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(MarketingSearchCriteria))
			{
				return false;
			}

			var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);
			
			var result = new MarketingSearchCriteria();
			var respGroup = qs["respGroup"].EmptyToNull();
			result.ResponseGroup = SearchResponseGroup.Full;
			if (respGroup != null)
			{
				result.ResponseGroup = (SearchResponseGroup)Enum.Parse(typeof(SearchResponseGroup), respGroup, true);
			}
			result.Keyword = qs["q"].EmptyToNull();

			result.Count = qs["count"].TryParse(20);
			result.Start = qs["start"].TryParse(0);
			bindingContext.Model = result;
			return true;
		}

		#endregion
	}
}