using System.Web.Mvc;
using VirtoCommerce.ApiWebClient.Extensions.Routing;

namespace VirtoCommerce.Web.Models.Binders
{
    public class CategoryPathModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var parameters = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var sp = parameters != null ? parameters.RawValue as CategoryPathModel : null;
            if (sp == null)
            {
                sp = new CategoryPathModel();

                var routeData = controllerContext.RouteData;
                if (routeData.Values.ContainsKey(Constants.Category) && routeData.Values[Constants.Category] != null)
                {
                    var category = routeData.Values[Constants.Category].ToString();
                    sp.Url = category;
                }
            }
            return sp;
        }
    }
}