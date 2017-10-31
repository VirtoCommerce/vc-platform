using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        
        [StringLength(1024)]
        public string Detail { get; set; }
    }

}
