using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class UserApiKey : AuditableEntity
    {
        public string ApiKey { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
