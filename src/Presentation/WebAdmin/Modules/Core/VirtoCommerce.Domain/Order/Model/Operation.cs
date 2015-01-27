using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.Domain.Order.Model
{
	public abstract class Operation : Entity, IAuditable
	{
		#region IAuditable Members

		public DateTime CreatedDate	{ get; set;	}
		public string CreatedBy	{ get; set;	}
		public DateTime ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		#endregion

		public string Number { get; set; }
		public bool? IsApproved { get; set; }
		public string StatusId { get; set; }
		/// <summary>
		/// source warehouse or store
		/// </summary>
		public string SourceStoreId { get; set; }
		/// <summary>
		/// Target warehouse or store
		/// </summary>
		public string TargetStoreId { get; set;	}
		/// <summary>
		/// Source organization or agent
		/// </summary>
		public string SourceAgentId { get; set; }
		/// <summary>
		/// Target organization or agent
		/// </summary>
		public string TargetAgentId	{ get; set;	}
		/// <summary>
		/// Currecy code
		/// </summary>
		public string Comment { get; set; }
		public CurrencyCodes? Currency { get; set; }
		public bool? TaxIncluded { get; set;	}
		public decimal Sum { get; set; }
		public decimal Tax { get; set; }
	}
}
