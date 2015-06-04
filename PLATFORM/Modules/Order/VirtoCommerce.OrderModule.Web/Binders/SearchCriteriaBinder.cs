using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using coreModel = VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Binders
{

	public class SearchCriteriaBinder : IModelBinder
	{

		#region IModelBinder Members

		public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(coreModel.SearchCriteria))
			{
				return false;
			}

			var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

			var result = new coreModel.SearchCriteria();

			result.Keyword = qs["q"].EmptyToNull();
			var respGroup = qs["respGroup"].EmptyToNull();
			if (respGroup != null)
			{
				result.ResponseGroup = EnumUtility.SafeParse<coreModel.ResponseGroup>(respGroup, coreModel.ResponseGroup.Default);
			}
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