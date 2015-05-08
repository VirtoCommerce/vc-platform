using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
	public class OperationLogEntity : AuditableEntity
	{

		[StringLength(50)]
		public string ObjectType { get; set; }

		[StringLength(200)]
		public string ObjectId { get; set; }

		[Required]
		[StringLength(20)]
		public string OperationType { get; set; }
	}

}
