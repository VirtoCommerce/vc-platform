using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using model = VirtoCommerce.Foundation.Orders.Model;
using System.ComponentModel;
using PdfRpt.DataAnnotations;

namespace VirtoCommerce.ManagementClient.Fulfillment.Model
{
	public class Shipment : NotifyPropertyChanged
	{
		public Shipment(model.Shipment item = null)
		{
			if (item != null)
			{
				this.InjectFrom(item);

				var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
				ShipmentId = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();

				Address = item.OrderForm != null ? item.OrderForm.OrderGroup.OrderAddresses.Where(address => address.Name == "Shipping").FirstOrDefault().ToString() : string.Empty;
				Customer = item.OrderForm != null ? item.OrderForm.OrderGroup.CustomerName : string.Empty;
				ShipmentItems = new ObservableCollection<ShipmentItem>();
				item.ShipmentItems.ToList().ForEach(shipItem => this.ShipmentItems.Add(new ShipmentItem(shipItem)));
			}
		}

		private string _shipmentId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[IsVisible(false)]
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

		public DateTime? LastModified
		{
			get;
			set;
		}
		
		private string _shippingMethodName;
		[DisplayName("Method name")]
		public string ShippingMethodName
		{
			get
			{
				return _shippingMethodName;
			}
			set
			{
				_shippingMethodName = value;
				OnPropertyChanged();
			}
		}

		private string _customer;
		[DisplayName("Customer")]
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

		private string _address;
		[DisplayName("Address")]
		public string Address
		{
			get
			{
				return _address;
			}
			set
			{
				_address = value;
				OnPropertyChanged();
			}
		}

		private string _orderId;
		[IsVisible(false)]
		public string Order
		{
			get
			{
				return _orderId;
			}
			set
			{
				_orderId = value;
				OnPropertyChanged();
			}
		}

		private bool _isSelected;
		[IsVisible(false)]
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				OnPropertyChanged();
			}
		}

		private string _status;
		[IsVisible(false)]
		public string Status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
				OnPropertyChanged();
			}
		}

		public decimal OverallItems
		{
			get
			{
				return ShipmentItems.Sum(y => y.Quantity);
			}
		}

		private ObservableCollection<ShipmentItem> _ShipmentItems;
		public ObservableCollection<ShipmentItem> ShipmentItems
		{
			get
			{
				return _ShipmentItems;
			}
			set
			{
				_ShipmentItems = value;
				OnPropertyChanged();
			}
		}
	}
}
