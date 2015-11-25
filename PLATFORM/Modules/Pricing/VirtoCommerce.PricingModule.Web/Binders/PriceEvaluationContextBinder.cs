using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using webModel = VirtoCommerce.PricingModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Binders
{
    public class PriceEvaluationContextBinder : IModelBinder
    {
      
        #region IModelBinder methods
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(coreModel.PriceEvaluationContext))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

            var result = new coreModel.PriceEvaluationContext();

            result.StoreId = qs.Get("store") ?? qs.Get("evalContext.storeId");

            result.CatalogId = qs.Get("catalog") ?? qs.Get("evalContext.catalogId");
            result.ProductIds = qs.GetValues("products");
            if(result.ProductIds == null)
            {
                var productIds = qs.Get("evalContext.productIds");
                if(!String.IsNullOrEmpty(productIds))
                {
                    result.ProductIds = productIds.Split(',');
                }
            }
         
            result.PricelistIds = qs.GetValues("pricelists");
            if (result.PricelistIds == null)
            {
                var pricelistIds = qs.Get("evalContext.pricelistIds");
                if (!String.IsNullOrEmpty(pricelistIds))
                {
                    result.PricelistIds = pricelistIds.Split(',');
                }
            }

            var currency = qs.Get("currency") ?? qs.Get("evalContext.currency");
            if (currency != null)
            {
                result.Currency = EnumUtility.SafeParse(currency, CurrencyCodes.USD);
            }
            result.Quantity = qs.GetValue<decimal>("quantity", 0);
            result.CustomerId = qs.Get("customer") ?? qs.Get("evalContext.customerId");
            result.OrganizationId = qs.Get("organization");
            var certainDate = qs.Get("date");
            if(certainDate != null)
            {
                result.CertainDate = Convert.ToDateTime(certainDate, CultureInfo.InvariantCulture);
            }

            bindingContext.Model = result;
            return true;
        } 
        #endregion
    }
}