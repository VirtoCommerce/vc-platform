using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Model
{
    public class UserApiKeyEntity : AuditableEntity
    {
        public string UserName { get; set; }
        public string ApiKey { get; set; }

        public string UserId { get; set; }
        public bool IsActive { get; set; }

        public virtual UserApiKeyEntity FromModel(UserApiKey apiKey, PrimaryKeyResolvingMap pkMap)
        {
            if (apiKey == null)
                throw new ArgumentNullException(nameof(apiKey));


            pkMap.AddPair(apiKey, this);

            Id = apiKey.Id;
            ApiKey = apiKey.ApiKey;
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
            apiKey.ApiKey = ApiKey;
            apiKey.UserName = UserName;
            apiKey.UserId = UserId;
            apiKey.IsActive = IsActive;

            return apiKey;
        }

        public virtual void Patch(UserApiKeyEntity target)
        {
            target.UserName = UserName;
            target.ApiKey = ApiKey;
            target.IsActive = IsActive;
           
        }
    }
}
