using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;

namespace VirtoCommerce.MarketingModule.Web.Binders
{
    public class DynamicContentEvalContextBinder : IModelBinder
    {
        #region IModelBinder methods
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(DynamicContentEvaluationContext))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

            var result = new DynamicContentEvaluationContext();

            result.StoreId = qs.Get("storeId") ?? qs.Get("evalContext.storeId");
            result.PlaceName= qs.Get("placeHolder") ?? qs.Get("evalContext.placeHolder");
            result.Tags = qs.GetValues("tags");
            if (result.Tags == null)
            {
                var tags = qs.Get("evalContext.tags");
                if (!String.IsNullOrEmpty(tags))
                {
                    result.Tags = tags.Split(',');
                }
            }

            var toDate = qs.Get("toDate") ?? qs.Get("evalContext.toDate");
            if (toDate != null)
            {
                result.ToDate = Convert.ToDateTime(toDate, CultureInfo.InvariantCulture);
            }

            bindingContext.Model = result;
            return true;
        }
        #endregion

    }
}