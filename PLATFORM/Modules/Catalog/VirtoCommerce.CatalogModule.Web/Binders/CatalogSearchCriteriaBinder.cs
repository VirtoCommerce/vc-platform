using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Binders
{
    public class CatalogSearchCriteriaBinder : IModelBinder
    {
      
        #region IModelBinder methods
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(coreModel.SearchCriteria))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query as string);

            var result = new coreModel.SearchCriteria();

            var respGroup = qs["respGroup"].EmptyToNull();
            result.ResponseGroup = EnumUtility.SafeParse<coreModel.ResponseGroup>(respGroup, coreModel.ResponseGroup.Full);

            result.Keyword = qs["q"].EmptyToNull();

            result.CatalogId = qs["catalog"].EmptyToNull();
            result.CategoryId = qs["category"].EmptyToNull();
            result.LanguageCode = qs["language"].EmptyToNull();
            result.Currency = qs["currency"].EmptyToNull();
 
            result.Count = qs["count"].TryParse(20);
            result.Start = qs["start"].TryParse(0);
            bindingContext.Model = result;
            return true;
        } 
        #endregion
    }
}