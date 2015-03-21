using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Model
{
	public class CustomerOrderEntity : OperationEntity, IOperation
	{
		public CustomerOrderEntity()
		{
			Addresses = new NullCollection<AddressEntity>();
			InPayments = new NullCollection<PaymentInEntity>();
			Items = new NullCollection<LineItemEntity>();
			Shipments = new NullCollection<ShipmentEntity>();
			Discounts = new NullCollection<DiscountEntity>();
		}

		[Required]
		[StringLength(64)]
		public string CustomerId { get; set; }
		[Required]
		[StringLength(64)]
		public string StoreId { get; set; }
		
		[StringLength(64)]
		public string ChannelId { get; set; }
		[StringLength(64)]
		public string OrganizationId { get; set; }
		[StringLength(64)]
		public string EmployeeId { get; set; }



		public virtual ObservableCollection<AddressEntity> Addresses { get; set; }
		public virtual ObservableCollection<PaymentInEntity> InPayments { get; set; }

		public virtual ObservableCollection<LineItemEntity> Items { get; set; }
		public virtual ObservableCollection<ShipmentEntity> Shipments { get; set; }

		public virtual ObservableCollection<DiscountEntity> Discounts { get; set; }
	}
}
