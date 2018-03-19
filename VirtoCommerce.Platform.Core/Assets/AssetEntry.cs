using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class AssetEntry : AuditableEntity
    {
        public AssetEntry()
        {
            BlobInfo = new BlobInfo();
            Tenant = new TenantIdentity();
        }

        /// <summary>
        /// Asset language
        /// </summary>
        public string LanguageCode { get; set; }

        public TenantIdentity Tenant { get; set; }
        public BlobInfo BlobInfo { get; set; }

        /// <summary>
        /// User defined grouping (optional)
        /// </summary>
        public string Group { get; set; }
    }
}
