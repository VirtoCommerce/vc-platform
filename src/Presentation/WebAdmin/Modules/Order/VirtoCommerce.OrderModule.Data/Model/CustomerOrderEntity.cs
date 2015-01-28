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
		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<PaymentInEntity> InPayments { get; set; }

		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<ShipmentEntity> Shipments { get; set; }

		public virtual DiscountEntity Discount { get; set; }
	}
}
