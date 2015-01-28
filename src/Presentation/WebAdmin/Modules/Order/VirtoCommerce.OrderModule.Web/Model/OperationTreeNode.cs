using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public abstract class OperationTreeNode
	{
		public string Id { get; set; }
		public string OperationType { get; set; }

		public DateTime CreatedDate { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string ModifiedBy { get; set; }

		public string Number { get; set; }
		public bool IsApproved { get; set; }
		public string StatusId { get; set; }
		/// <summary>
		/// source warehouse or store
		/// </summary>
		public string SourceStoreId { get; set; }
		/// <summary>
		/// Target warehouse or store
		/// </summary>
		public string TargetStoreId { get; set; }
		/// <summary>
		/// Source organization or agent
		/// </summary>
		public string SourceAgentId { get; set; }
		/// <summary>
		/// Target organization or agent
		/// </summary>
		public string TargetAgentId { get; set; }

		public string Comment { get; set; }
		/// <summary>
		/// Currecy code
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public CurrencyCodes? Currency { get; set; }
		public bool TaxIncluded { get; set; }
		public decimal Sum { get; set; }
		public decimal Tax { get; set; }

		public string ParentOperationId { get; set; }
		public ICollection<OperationTreeNode> Childrens { get; set; }
		
	}
}