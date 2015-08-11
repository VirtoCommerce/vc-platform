using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Model
{
	/// <summary>
	/// Represent information about quantity and line item belongs to shipment
	/// </summary>
	public class ShipmentItem : AuditableEntity
	{
		public string LineItemId { get; set; }
		public LineItem LineItem { get; set; }

		public string BarCode { get; set; }

		public int Quantity { get; set; }

	}
}
