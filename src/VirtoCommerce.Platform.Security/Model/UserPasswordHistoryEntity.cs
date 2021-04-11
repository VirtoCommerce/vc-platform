using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Security.Model
{
    public class UserPasswordHistoryEntity : AuditableEntity
    {
        [StringLength(128)]
        public string UserId { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
