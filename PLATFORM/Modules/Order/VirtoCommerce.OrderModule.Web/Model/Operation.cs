using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent base class for all order module documents (operations)
	/// contains shared set of properties
	/// </summary>
	public class Operation : AuditableEntity, ISupportCancellation
	{
		/// <summary>
		/// Operation type string representation (CustomerOrder, Shipment etc)
		/// </summary>
		public string OperationType { get; set; }

		/// <summary>
		/// Unique user friendly document number (generate automatically based on special algorithm realization)
		/// </summary>
		public string Number { get; set; }
		/// <summary>
		/// Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status
		/// (Order completion, Shipment send etc)
		/// </summary>
		public bool IsApproved { get; set; }
		/// <summary>
		/// Current operation status may have any values defined by concrete business process
		/// </summary>
		public string Status { get; set; }


		public string Comment { get; set; }
		/// <summary>
		/// Currency code
		/// </summary>
		public string Currency { get; set; }
		public bool TaxIncluded { get; set; }
		/// <summary>
		/// Money amount without tax
		/// </summary>
		public decimal Sum { get; set; }
		/// <summary>
		/// Tax total
		/// </summary>
		public decimal Tax { get; set; }

		#region ISupportCancelation Members

		public bool IsCancelled { get; set; }
		public DateTime? CancelledDate { get; set; }
		public string CancelReason { get; set; }

		#endregion

		/// <summary>
		/// Used for construct hierarchy of operation and represent parent operation id
		/// </summary>
		public string ParentOperationId { get; set; }

		public IEnumerable<Operation> ChildrenOperations { get; set; }

		/// <summary>
		/// Used for dynamic properties management, contains object type string
		/// </summary>
		public string ObjectType { get; set; }
		/// <summary>
		/// Dynamic properties collections
		/// </summary>
		public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
	}
}