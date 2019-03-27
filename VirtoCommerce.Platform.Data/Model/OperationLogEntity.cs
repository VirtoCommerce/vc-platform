using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class OperationLogEntity : AuditableEntity
    {

        [StringLength(50)]
        [Index("IX_ObjectType_ObjectId", 1)]
        public string ObjectType { get; set; }

        [StringLength(200)]
        [Index("IX_ObjectType_ObjectId", 2)]
        public string ObjectId { get; set; }

        [Required]
        [StringLength(20)]
        public string OperationType { get; set; }


        [StringLength(2048)]
        public string Detail { get; set; }
    }

}
