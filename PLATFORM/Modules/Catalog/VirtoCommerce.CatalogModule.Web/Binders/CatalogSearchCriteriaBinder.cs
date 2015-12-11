using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Platform.Core.Common;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Web.Binders
{
    public class CatalogSearchCriteriaBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(coreModel.SearchCriteria))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);

            var result = new coreModel.SearchCriteria();

            var respGroup = qs["criteria.responseGroup"].EmptyToNull();
            result.ResponseGroup = EnumUtility.SafeParse(respGroup, coreModel.SearchResponseGroup.Full);

            result.Keyword = qs["criteria.keyword"].EmptyToNull();

            result.SearchInChildren = qs["criteria.searchInChildren"].TryParse(false);

            result.CatalogId = qs["criteria.catalogId"].EmptyToNull();
            result.CategoryId = qs["criteria.categoryId"].EmptyToNull();
            result.LanguageCode = qs["criteria.languageCode"].EmptyToNull();
            result.Currency = qs["criteria.currency"].EmptyToNull();

            result.Take = qs["criteria.take"].TryParse(20);
            result.Skip = qs["criteria.skip"].TryParse(0);
            bindingContext.Model = result;

            return true;
        }
    }
}
