using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Order.Model
{
	public class ShipmentItem : AuditableEntity, IPosition
	{
		public ShipmentItem()
		{
		}

		public ShipmentItem(LineItem lineItem)
		{
			LineItem = lineItem;
			LineItemId = lineItem.Id;

			Quantity = lineItem.Quantity;
		}

		public string LineItemId { get; set; }
		public LineItem LineItem { get; set; }

		public string BarCode { get; set; }

		public int Quantity { get; set; }


		#region IPosition Members

		public string ProductId
		{
			get { return LineItem.ProductId; }
		}

		public string CatalogId
		{
			get { return LineItem.CatalogId; }
		}

		public string CategoryId
		{
			get { return LineItem.CategoryId; }
		}

		public string Name
		{
			get { return LineItem.Name; }
		}

		public string ImageUrl
		{
			get { return LineItem.ImageUrl; }
		}

		#endregion
	}
}
