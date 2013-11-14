using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using model = VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Model;
using System.ComponentModel;
using PdfRpt.DataAnnotations;

namespace VirtoCommerce.ManagementClient.Fulfillment.Model
{
	public class ShipmentItem : NotifyPropertyChanged
	{
		public ShipmentItem(model.ShipmentItem item = null)
		{
			if (item != null)
			{
				this.InjectFrom(item);

				var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
				ShipmentItemId = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();
				ItemCode = item.LineItem != null ? item.LineItem.CatalogItemCode : null;
				ItemName = item.LineItem != null ? item.LineItem.DisplayName : null;
				UnitPrice = item.LineItem != null ? item.LineItem.PlacedPrice : 0;
				ShippingAddress = item.Shipment != null && item.Shipment.OrderForm != null ? item.Shipment.OrderForm.OrderGroup.OrderAddresses.Where(address => address.Name == "Shipping").FirstOrDefault().ToString() : string.Empty;
				BillingAddress = item.Shipment != null && item.Shipment.OrderForm != null ? item.Shipment.OrderForm.OrderGroup.OrderAddresses.Where(address => address.Name == "Billing").FirstOrDefault().ToString() : string.Empty;
				Customer = item.Shipment != null && item.Shipment.OrderForm != null ? item.Shipment.OrderForm.OrderGroup.CustomerName : string.Empty;
				Order = item.Shipment != null && item.Shipment.OrderForm != null ? ((Order) item.Shipment.OrderForm.OrderGroup).TrackingNumber : string.Empty;
				OrderDate = item.Shipment != null && item.Shipment.OrderForm != null ? item.Shipment.OrderForm.OrderGroup.Created : null;
				ShippingTaxTotal = item.Shipment != null ? item.Shipment.ShippingTaxTotal : 0;
				ItemTaxTotal = item.Shipment != null ? item.Shipment.ItemTaxTotal : 0;
				ItemSubtotal = item.Shipment != null ? item.Shipment.ItemSubtotal : 0;
				Subtotal = item.Shipment != null ? item.Shipment.Subtotal : 0;
				TotalBeforeTax = item.Shipment != null ? item.Shipment.TotalBeforeTax : 0;
				BillingCurrency = item.Shipment != null && item.Shipment.OrderForm != null ? item.Shipment.OrderForm.OrderGroup.BillingCurrency : string.Empty;
				ShippingTotal = item.Shipment != null ? item.Shipment.ShipmentTotal : 0;
				DiscountAmount = item.Shipment != null ? item.Shipment.ShippingDiscountAmount : 0;
			}
		}

		private string _shipmentItemId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[IsVisible(false)]
		public string ShipmentItemId
		{
			get
			{
				return _shipmentItemId;
			}
			set
			{
				_shipmentItemId = value;
				OnPropertyChanged();
			}
		}

		private string _shipmentId;
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string ShipmentId
		{
			get
			{
				return _shipmentId;
			}
			set
			{
				_shipmentId = value;
				OnPropertyChanged();
			}
		}

		private string _customer;
		public string Customer
		{
			get
			{
				return _customer;
			}
			set
			{
				_customer = value;
				OnPropertyChanged();
			}
		}

		private string _billingCurrency;
		public string BillingCurrency
		{
			get
			{
				return _billingCurrency;
			}
			set
			{
				_billingCurrency = value;
				OnPropertyChanged();
			}
		}

		private string _order;
		public string Order
		{
			get
			{
				return _order;
			}
			set
			{
				_order = value;
				OnPropertyChanged();
			}
		}

		private DateTime? _orderDate;
		public DateTime? OrderDate
		{
			get
			{
				return _orderDate;
			}
			set
			{
				_orderDate = value;
				OnPropertyChanged();
			}
		}
		
		private string _itemName;
		[DisplayName("Name")]
		public string ItemName
		{
			get
			{
				return _itemName;
			}
			set
			{
				_itemName = value;
				OnPropertyChanged();
			}
		}

		private string _itemCode;
		[DisplayName("Code")]
		public string ItemCode
		{
			get
			{
				return _itemCode;
			}
			set
			{
				_itemCode = value;
				OnPropertyChanged();
			}
		}

		private string _billingAddress;
		[DisplayName("Billing Address")]
		public string BillingAddress
		{
			get
			{
				return _billingAddress;
			}
			set
			{
				_billingAddress = value;
				OnPropertyChanged();
			}
		}

		private string _shippingAddress;
		[DisplayName("Shipping Address")]
		public string ShippingAddress
		{
			get
			{
				return _shippingAddress;
			}
			set
			{
				_shippingAddress = value;
				OnPropertyChanged();
			}
		}

		private decimal _qty;
		[DisplayName("Quantity")]
		public decimal Quantity
		{
			get
			{
				return _qty;
			}
			set
			{
				_qty = value;
				OnPropertyChanged();
			}
		}

		private decimal _shippingTaxTotal;
		public decimal ShippingTaxTotal
		{
			get
			{
				return _shippingTaxTotal;
			}
			set
			{
				_shippingTaxTotal = value;
				OnPropertyChanged();
			}
		}
		private decimal _totalBeforeTax;
		public decimal TotalBeforeTax
		{
			get
			{
				return _totalBeforeTax;
			}
			set
			{
				_totalBeforeTax = value;
				OnPropertyChanged();
			}
		}


		private decimal _itemTaxTotal;
		public decimal ItemTaxTotal
		{
			get
			{
				return _itemTaxTotal;
			}
			set
			{
				_itemTaxTotal = value;
				OnPropertyChanged();
			}
		}

		private decimal _shippingTotal;
		public decimal ShippingTotal
		{
			get
			{
				return _shippingTotal;
			}
			set
			{
				_shippingTotal = value;
				OnPropertyChanged();
			}
		}
		
		private decimal _subtotal;
		public decimal Subtotal
		{
			get
			{
				return _subtotal;
			}
			set
			{
				_subtotal = value;
				OnPropertyChanged();
			}
		}

		private decimal _itemSubtotal;
		public decimal ItemSubtotal
		{
			get
			{
				return _itemSubtotal;
			}
			set
			{
				_itemSubtotal = value;
				OnPropertyChanged();
			}
		}

		private decimal _discountAmount;
		public decimal DiscountAmount
		{
			get
			{
				return _discountAmount;
			}
			set
			{
				_discountAmount = value;
				OnPropertyChanged();
			}
		}

		private decimal _unitPrice;
		[DisplayName("Unit price")]
		public decimal UnitPrice
		{
			get
			{
				return _unitPrice;
			}
			set
			{
				_unitPrice = value;
				OnPropertyChanged();
			}
		}

		[DisplayName("Total")]
		public decimal Total
		{
			get
			{
				return Math.Round(UnitPrice * Quantity, 2);
			}
		}
	}
}
