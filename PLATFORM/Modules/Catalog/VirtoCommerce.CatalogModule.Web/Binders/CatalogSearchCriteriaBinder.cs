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

            var respGroup = qs["criteria.responseGroup"].EmptyToNull();
            result.ResponseGroup = EnumUtility.SafeParse<coreModel.SearchResponseGroup>(respGroup, coreModel.SearchResponseGroup.Full);

            result.Keyword = qs["criteria.keyword"].EmptyToNull();

            result.SearchInChildren = qs["criteria.searchInChildren"].TryParse(false);

            result.CatalogId = qs["criteria.catalogId"].EmptyToNull();
            result.CategoryId = qs["criteria.categoryId"].EmptyToNull();
            result.LanguageCode = qs["criteria.languageCode"].EmptyToNull();
            result.Currency = qs["criteria.currency"].EmptyToNull();
 
            result.Count = qs["criteria.count"].TryParse(20);
            result.Start = qs["criteria.start"].TryParse(0);
            bindingContext.Model = result;
            return true;
        } 
        #endregion
    }
}