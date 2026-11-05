using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Model.Security
{
    public class OAuthResourceInfo : OAuthResource
    {
        /// <summary>
        /// Whether the resource is listed in Authorization:Resources. Tokens are issued only for listed resources.
        /// </summary>
        public bool IsRegistered { get; set; }
    }
}
