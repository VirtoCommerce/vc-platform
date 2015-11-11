using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.LiquidThemeEngine.Binders
{
    public abstract class BaseModelBinder<T> : DefaultModelBinder
        where T : class
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.BindModel(controllerContext, bindingContext);

            var model = result as T;
            if (model != null)
            {
                var request = controllerContext.HttpContext.Request;
                ComplementModel(model, request);
            }

            return result;
        }

        protected abstract void ComplementModel(T model, HttpRequestBase request);
    }
}
