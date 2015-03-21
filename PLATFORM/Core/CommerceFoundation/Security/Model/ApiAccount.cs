using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Security.Model
{
    public class ApiAccount : StorageEntity
    {
        public ApiAccount()
        {
            ApiAccountId = GenerateNewKey();
        }

        [Key]
        public string ApiAccountId { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Required]
        [StringLength(128)]
        [Index(IsUnique = true)]
        public string AppId { get; set; }

        public string SecretKey { get; set; }

        public bool IsActive { get; set; }

        [Parent]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
