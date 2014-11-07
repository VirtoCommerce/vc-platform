using System;
using System.Net.Http;
using VirtoCommerce.MerchandisingModule.Models;
using System.Web.Http.ModelBinding;
using System.Web.Http.Controllers;

namespace VirtoCommerce.MerchandisingModule.Web.Binders
{
    /// <summary>
    /// Class SearchParametersBinder.
    /// </summary>
    public class SearchParametersBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(SearchParameters))
            {
                return false;
            }

            string key = actionContext.Request.RequestUri.Query as string;

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
