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
	public class ShipmentEntity : OperationEntity
	{
		public ShipmentEntity()
		{
			Items = new NullCollection<LineItemEntity>();
			InPayments = new NullCollection<PaymentInEntity>();
		}

		public string OrganizationId { get; set; }
		public string FulfilmentCenterId { get; set; }
		public string EmployeeId { get; set; }

		public string CustomerOrderId { get; set; }
		public virtual CustomerOrderEntity CustomerOrder { get; set; }

		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<PaymentInEntity> InPayments { get; set; }
		public virtual AddressEntity DeliveryAddress { get; set; }

		public string DiscountId { get; set; }
		public virtual DiscountEntity Discount { get; set; }
	}
}
