using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.Model.Asset
{
    public class AssetEntry : Entity
    {
        public string LanguageCode { get; set; }

        public TenantIdentity Tenant { get; set; }
        public BlobInfo BlobInfo { get; set; }
    }
}
