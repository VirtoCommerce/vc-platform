using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Operation : Entity, IAuditable, IOperation
	{
		#region IAuditable Members

		public DateTime CreatedDate { get; set;	}
		public string CreatedBy	{ get; set;	}
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

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
		public CurrencyCodes Currency { get; set; }
		public bool TaxIncluded { get; set;	}
		public decimal Sum { get; set; }
		public decimal Tax { get; set; }

		public abstract IEnumerable<Operation> ChildrenOperations
		{
			get;
		}
	
	}
}
