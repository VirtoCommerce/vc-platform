using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using VirtoCommerce.CoreModule.Web.Security.Models;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CoreModule.Web.Controllers.Api
{
	public class UserSearchCriteriaBinder : IModelBinder
	{

		#region IModelBinder Members

		public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType != typeof(UserSearchCriteria))
			{
				return false;
			}

			var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

			var result = new UserSearchCriteria();

			result.Keyword = qs["q"].EmptyToNull();

			result.Count = qs["count"].TryParse(20);
			result.Start = qs["start"].TryParse(0);
			bindingContext.Model = result;
			return true;
		}

		#endregion
	}
}