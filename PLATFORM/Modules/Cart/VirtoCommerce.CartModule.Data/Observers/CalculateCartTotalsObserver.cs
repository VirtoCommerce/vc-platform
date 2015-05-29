using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Cart.Events;
using VirtoCommerce.Domain.Cart.Model;

namespace VirtoCommerce.CartModule.Data.Observers
{
	public class CalculateCartTotalsObserver : IObserver<CartChangeEvent>
	{
		#region IObserver<CustomerOrder> Members

		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(CartChangeEvent value)
		{
			CalculateCartTotals(value);
		}

		#endregion
		private static void CalculateCartTotals(CartChangeEvent cartChangeEvent)
		{
			var cart = cartChangeEvent.ModifiedCart;

			cart.Total = 0;
			cart.SubTotal = 0;
			cart.TaxTotal = 0;
			cart.ShippingTotal = 0;
			cart.DiscountTotal = 0;

			if (cart.Items != null)
			{
				foreach (var item in cart.Items)
				{
					CalculateLineItemTotal(item);

					cart.DiscountTotal += item.DiscountTotal;
					cart.SubTotal += item.PlacedPrice * item.Quantity;
				}
			}

			if (cart.Shipments != null)
			{
				foreach (var shipment in cart.Shipments)
				{
					CalculateShipmentTotals(shipment);

					cart.ShippingTotal += shipment.Total;
					cart.DiscountTotal += shipment.DiscountTotal;
					cart.TaxTotal += shipment.TaxTotal;
				}
			}

			if (cart.Discounts != null)
			{
				foreach (var discount in cart.Discounts)
				{
					cart.DiscountTotal += discount.DiscountAmount;
				}
			}

			cart.Total = cart.SubTotal + cart.ShippingTotal + cart.TaxTotal - cart.DiscountTotal;
		}

		private static void CalculateLineItemTotal(LineItem lineItem)
		{
			lineItem.DiscountTotal = 0;
			lineItem.ExtendedPrice = 0;

			if (lineItem.Discounts != null)
			{
				foreach (var discount in lineItem.Discounts)
				{
					lineItem.DiscountTotal += discount.DiscountAmount;
				}
			}
			lineItem.ExtendedPrice = lineItem.PlacedPrice * lineItem.Quantity;
		
		}

		private static void CalculateShipmentTotals(Shipment shipment)
		{
			shipment.DiscountTotal = 0;
			shipment.ItemSubtotal = 0;
			shipment.Subtotal = 0;
			shipment.Total = 0;

			if (shipment.Discounts != null)
			{
				foreach (var discount in shipment.Discounts)
				{
					shipment.DiscountTotal += discount.DiscountAmount;
				}
			}

			if (shipment.Items != null)
			{
				foreach (var item in shipment.Items)
				{
					shipment.ItemSubtotal += item.PlacedPrice * item.Quantity;
				}
			}

			shipment.Subtotal = shipment.ShippingPrice + shipment.TaxTotal;
			shipment.Total = shipment.Subtotal - shipment.DiscountTotal;
		}
	}
}
