using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class CustomerOrderEntity : OperationEntity
	{
		public CustomerOrderEntity()
		{
			Addresses = new NullCollection<AddressEntity>();
			InPayments = new NullCollection<PaymentInEntity>();
			Items = new NullCollection<LineItemEntity>();
			Shipments = new NullCollection<ShipmentEntity>();
		}

		public string CustomerId { get; set; }
		public string SiteId { get; set; }
		public string OrganizationId { get; set; }
		public string EmployeeId { get; set; }

		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<PaymentInEntity> InPayments { get; set; }

		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<ShipmentEntity> Shipments { get; set; }

		public string DiscountId { get; set; }
		public virtual DiscountEntity Discount { get; set; }
	}
}
