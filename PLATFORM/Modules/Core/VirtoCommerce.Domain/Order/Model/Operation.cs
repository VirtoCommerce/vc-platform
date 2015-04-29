using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Operation : AuditableEntity, IOperation
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
