using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace VirtoCommerce.CatalogModule.Web.Binders
{
    /// <summary>
    /// Because Swagger generated API client passed arrays as joined string need parse query string by binder
    /// </summary>
    public class IdsStringArrayBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(string[]))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

            var flattenedString = qs.GetValues("ids");
            if (flattenedString != null)
            {
                bindingContext.Model = flattenedString.SelectMany(s => s.Split(',')).ToArray();
            }

            return true;
        }
    }
}