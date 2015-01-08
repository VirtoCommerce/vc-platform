using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class ShipmentConverter
	{
		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this Shipment source, Shipment target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			//Simply properties patch
			if (source.ShippingPrice != null)
				target.ShippingPrice = source.ShippingPrice;

			if (source.Weight != null)
				target.Weight = source.Weight;
			if (source.Dimension != null && !source.Dimension.Equals(target.Dimension))
				target.Dimension = source.Dimension;
			if (source.TaxTotal != null)
				target.TaxTotal = source.TaxTotal;
			if (source.RecipientAddress != null)
				target.RecipientAddress = source.RecipientAddress;

			if (source.Discounts != null)
			{
				if (target.Discounts == null)
					target.Discounts = new List<Discount>();

				source.Discounts.Patch(target.Discounts, new DiscountComparer(),
													 (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}

			if (source.Items != null)
			{
				if (target.Items == null)
					target.Items = new List<ShipmentItem>();

				source.Items.Patch(target.Items, new ShipmentItemComparer(), (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

		}

	}

	public class ShipmentComparer : IEqualityComparer<Shipment>
	{
		#region IEqualityComparer<Discount> Members

		public bool Equals(Shipment x, Shipment y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(Shipment obj)
		{
			return obj.Id.GetHashCode();
		}

		#endregion
	}
}
