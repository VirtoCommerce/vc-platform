using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public abstract class OperationEntity : Entity
	{
		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		public string Number { get; set; }
		public bool IsApproved { get; set; }
		public string Status { get; set; }
		public string SourceStoreId { get; set; }
		public string TargetStoreId { get; set; }
		public string SourceAgentId { get; set; }
		public string TargetAgentId { get; set; }
		public string Comment { get; set; }
		public string Currency { get; set; }
		public bool TaxIncluded { get; set; }
		public decimal Sum { get; set; }
		public decimal Tax { get; set; }
	}
}
