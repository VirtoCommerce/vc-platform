using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Binders
{
    /// <summary>
    ///     Class SearchCriteriaBinder.
    /// </summary>
    public class SearchParametersBinder : IModelBinder
    {
        #region Public Methods and Operators

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
                bindingContext.ModelName,
                "Cannot convert value to SearchParameters");
            return false;
        }

        #endregion
    }
}
