using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Extensions.Routing;
using VirtoCommerce.Web.Client.Helpers;

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
                    var lang = routeData.Values.ContainsKey(Constants.Language)
                        ? routeData.Values[Constants.Language] as string
                        : null;
                    sp.Url = SettingsHelper.SeoDecode(category, SeoUrlKeywordTypes.Category, lang);
                }
            }
            return sp;
        }
    }
}