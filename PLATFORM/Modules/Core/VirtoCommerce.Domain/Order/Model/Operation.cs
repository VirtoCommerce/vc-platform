using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Operation : AuditableEntity, IOperation, ISupportCancellation, IHasDynamicProperties
	{
		public string OperationType { 
			get
			{
				return this.GetType().Name;
			}
		}

		public string ParentOperationId { get; set; }
		public string Number { get; set; }
		public bool IsApproved { get; set; }
		public string Status { get; set; }

		public string Comment { get; set; }
		public string Currency { get; set; }
		public bool TaxIncluded { get; set;	}
		public decimal Sum { get; set; }
		public decimal Tax { get; set; }

		#region ISupportCancelation Members

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }

		#endregion


		#region IHasDynamicProperties Members
		public string ObjectType { get; set; }
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
		#endregion
	}
}
