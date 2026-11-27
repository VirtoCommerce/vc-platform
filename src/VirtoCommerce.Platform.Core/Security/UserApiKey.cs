using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class UserApiKey : AuditableEntity
    {
        /// <summary>
        /// The key itself. Set it to create or replace a key; leave it empty to keep the stored one.
        /// The platform stores only <see cref="ApiKeyHash"/>, so a key read back is empty, except in the
        /// response that created it and on a row from before key hashing that is not hashed yet.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// The key as hashed by the registered IApiKeyHasher (SHA-256, lowercase hex, by default). Carried by backups so a restore keeps the keys working.
        /// </summary>
        public string ApiKeyHash { get; set; }

        /// <summary>
        /// A short, non-secret reminder of the key for display, such as <c>vc_A…Qz91</c>.
        /// </summary>
        public string ApiKeyHint { get; set; }

        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
