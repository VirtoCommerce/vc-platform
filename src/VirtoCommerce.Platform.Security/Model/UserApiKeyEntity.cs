using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model
{
    public class UserApiKeyEntity : AuditableEntity
    {
        public string UserName { get; set; }

        /// <summary>
        /// Plaintext of a key from before key hashing. The AddUserApiKeyHash migration hashes and clears it, and new
        /// keys never set it. Kept so the migration's Down step and older platform versions still map the column.
        /// </summary>
        public string ApiKey { get; set; }

        public string ApiKeyHash { get; set; }

        public string ApiKeyHint { get; set; }

        public string UserId { get; set; }
        public bool IsActive { get; set; }

        public virtual UserApiKeyEntity FromModel(UserApiKey apiKey, PrimaryKeyResolvingMap pkMap)
        {
            if (apiKey == null)
                throw new ArgumentNullException(nameof(apiKey));


            pkMap.AddPair(apiKey, this);

            Id = apiKey.Id;
            ApiKey = apiKey.ApiKey;
            ApiKeyHash = apiKey.ApiKeyHash;
            ApiKeyHint = apiKey.ApiKeyHint;
            UserName = apiKey.UserName;
            UserId = apiKey.UserId;
            IsActive = apiKey.IsActive;

            return this;
        }

        public virtual UserApiKey ToModel(UserApiKey apiKey)
        {
            if (apiKey == null)
                throw new ArgumentNullException(nameof(apiKey));

            apiKey.Id = Id;
            apiKey.CreatedDate = CreatedDate;
            apiKey.CreatedBy = CreatedBy;
            apiKey.ModifiedDate = ModifiedDate;
            apiKey.ModifiedBy = ModifiedBy;
            // A hashed key has no secret to hand out; only a row not hashed yet still has its plaintext.
            apiKey.ApiKey = ApiKeyHash == null ? ApiKey : null;
            apiKey.ApiKeyHash = ApiKeyHash;
            apiKey.ApiKeyHint = ApiKeyHint;
            apiKey.UserName = UserName;
            apiKey.UserId = UserId;
            apiKey.IsActive = IsActive;

            return apiKey;
        }

        public virtual void Patch(UserApiKeyEntity target)
        {
            target.UserName = UserName;
            target.ApiKey = ApiKey;
            target.ApiKeyHash = ApiKeyHash;
            target.ApiKeyHint = ApiKeyHint;
            target.IsActive = IsActive;
           
        }
    }
}
