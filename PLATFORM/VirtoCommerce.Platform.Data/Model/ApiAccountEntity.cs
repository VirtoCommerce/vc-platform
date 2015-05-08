using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class ApiAccountEntity : AuditableEntity
    {
        public ApiAccountType ApiAccountType { get; set; }
        public string AccountId { get; set; }
        public string AppId { get; set; }
        public string SecretKey { get; set; }
        public bool IsActive { get; set; }

		public virtual AccountEntity Account { get; set; }
    }
}
