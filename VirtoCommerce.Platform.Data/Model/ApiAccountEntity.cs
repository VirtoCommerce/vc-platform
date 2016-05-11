using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Data.Model
{
    public class ApiAccountEntity : AuditableEntity
    {
        [StringLength(128)]
        public string Name { get; set; }
        public ApiAccountType ApiAccountType { get; set; }
        public string AccountId { get; set; }

        [StringLength(128)]
        [Required]
        [Index("IX_AppId", IsUnique = true)]
        public string AppId { get; set; }
        public string SecretKey { get; set; }
        public bool IsActive { get; set; }

        public virtual AccountEntity Account { get; set; }
    }
}
