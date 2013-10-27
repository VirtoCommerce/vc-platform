using System;
using System.Activities;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.OrderWorkflow
{
	/// <summary>
	/// Calculates all totals in the order object based on the values entered for line items as well as discounts.
	/// </summary>
	public class CalculateTotalsActivity : OrderActivityBase
	{
		protected override void Execute(CodeActivityContext context)
		{
			base.Execute(context);

			// Get the property, since it is expensive process, make sure to get it once
			if (CurrentOrderGroup != null)
			{
				CalculateTotals(CurrentOrderGroup);
			}
		}

		private void CalculateTotals(OrderGroup orderGroup)
		{
			decimal subTotal = 0;
			decimal shippingTotal = 0;
			decimal handlingTotal = 0;
			decimal taxTotal = 0;
			decimal total = 0;

			// Calculate totals for OrderForms
			foreach (var form in orderGroup.OrderForms)
			{
				// Calculate totals for order form
				CalculateTotalsOrderForms(form);

				subTotal += form.Subtotal;
				shippingTotal += form.ShippingTotal;
				handlingTotal += form.HandlingTotal;
				taxTotal += form.TaxTotal;
				total += form.Total;
			}

			// calculate OrderGroup totals
			orderGroup.Subtotal = subTotal;
			orderGroup.ShippingTotal = shippingTotal;
			orderGroup.TaxTotal = taxTotal;
			orderGroup.Total = total;
			orderGroup.HandlingTotal = handlingTotal;
		}

		/// <summary>
		/// Calculates the totals order forms.
		/// </summary>
		/// <param name="form">The form.</param>
		private void CalculateTotalsOrderForms(OrderForm form)
		{
			decimal subTotal = 0;
			decimal shippingCostTotal = 0;
			decimal discountTotal = 0;

			foreach (var item in form.LineItems)
			{
				// calculate discounts
				item.LineItemDiscountAmount = item.Discounts.Sum(discount => discount.DiscountAmount);
				item.ExtendedPrice = item.PlacedPrice * item.Quantity - item.LineItemDiscountAmount;
				subTotal += item.PlacedPrice * item.Quantity;
				discountTotal += item.LineItemDiscountAmount;
			}

			// calculate form discounts
			form.DiscountAmount = form.Discounts.Sum(discount => discount.DiscountAmount);
			discountTotal += form.DiscountAmount;

			foreach (var shipment in form.Shipments)
			{
				// calculate discounts
				shipment.ShippingDiscountAmount = shipment.Discounts.Sum(discount => discount.DiscountAmount);

				shipment.ItemSubtotal = CalculateShipmentItemSubtotal(form, shipment);
				shipment.Subtotal = shipment.ItemSubtotal + shipment.ShippingCost + shipment.ItemTaxTotal + shipment.ShippingTaxTotal;
				shipment.TotalBeforeTax = shipment.ItemSubtotal + shipment.ShippingCost - shipment.ShippingDiscountAmount;
				shipment.ShipmentTotal = shipment.Subtotal - shipment.ShippingDiscountAmount;
				shippingCostTotal += shipment.ShippingCost;
				discountTotal += shipment.ShippingDiscountAmount;
			}

			form.ShippingTotal = shippingCostTotal;
			form.Subtotal = subTotal;
			form.Total = form.Subtotal + shippingCostTotal + form.TaxTotal - discountTotal;
		}

		private static decimal CalculateShipmentItemSubtotal(OrderForm form, Shipment shipment)
		{
			var retVal = (from shipItem in shipment.ShipmentItems where shipItem.Quantity > 0 
						  from lineItem in form.LineItems where lineItem.LineItemId == shipItem.LineItemId 
						  select lineItem.ExtendedPrice/lineItem.Quantity*shipItem.Quantity).Sum();

			return Math.Floor(retVal * 100) * 0.01m;
		}
	}
}
