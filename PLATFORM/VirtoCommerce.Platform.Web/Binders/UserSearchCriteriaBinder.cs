using System.Web;
using System.Web.Http.ModelBinding;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Web.Model.Security;

namespace VirtoCommerce.Platform.Web.Binders
{
    public class UserSearchCriteriaBinder : IModelBinder
    {
        #region IModelBinder Members

        public bool BindModel(System.Web.Http.Controllers.HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(UserSearchCriteria))
            {
                return false;
            }

            var qs = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);

            var result = new UserSearchCriteria
            {
                Keyword = qs["q"].EmptyToNull(),
                Count = qs["count"].TryParse(20),
                Start = qs["start"].TryParse(0)
            };

            bindingContext.Model = result;

            return true;
        }

        #endregion
    }
}
