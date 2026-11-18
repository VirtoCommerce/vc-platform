using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class OAuthAppSearchCriteria : SearchCriteriaBase
    {
        /// <summary>
        /// True: only self-registered applications; false: only the ones an administrator registered; null: all.
        /// </summary>
        public bool? SelfRegistered { get; set; }
    }
}
