using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Binders
{
	/// <summary>
	/// Class SearchCriteriaBinder.
	/// </summary>
    public class SearchParametersBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(SearchParameters))
            {
                return false;
            }

            var key = actionContext.Request.RequestUri.Query;

            SearchParameters result;
            if (SearchParameters.TryParse(key, out result))
            {
                bindingContext.Model = result;
                return true;
            }

            bindingContext.ModelState.AddModelError(
            bindingContext.ModelName, "Cannot convert value to SearchParameters");
            return false;
        }
    }
}
