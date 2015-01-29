using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.OrderModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class CustomerOrderConverter
	{
		public static CustomerOrder ToCoreModel(this CustomerOrderEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new CustomerOrder();
			retVal.InjectFrom(entity);

			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}

			if(entity.Discount != null)
			{
				retVal.Discount = entity.Discount.ToCoreModel();
			}
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.Addresses != null)
			{
				retVal.Addresses = entity.Addresses.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.Shipments != null)
			{
				retVal.Shipments = entity.Shipments.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.InPayments != null)
			{
				retVal.InPayments = entity.InPayments.Select(x => x.ToCoreModel()).ToList();
			}

			return retVal;
		}

		public static CustomerOrderEntity ToEntity(this CustomerOrder order)
		{
			if (order == null)
				throw new ArgumentNullException("order");

			var retVal = new CustomerOrderEntity();
			retVal.InjectFrom(order);

			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}

			if (order.Currency != null)
			{
				retVal.Currency = order.Currency.ToString();
			}

			if(order.Addresses != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(order.Addresses.Select(x=>x.ToEntity()));
			}
			if(order.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(order.Items.Select(x=>x.ToEntity()));
			}
			if(order.Shipments != null)
			{
				retVal.Shipments = new ObservableCollection<ShipmentEntity>(order.Shipments.Select(x=>x.ToEntity()));
			}
			if(order.InPayments != null)
			{
				retVal.InPayments = new ObservableCollection<PaymentInEntity>(order.InPayments.Select(x => x.ToEntity()));
			}
			if(order.Discount != null)
			{
				retVal.Discount = order.Discount.ToEntity();
			}
			return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this CustomerOrderEntity source, CustomerOrderEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			source.Patch((OperationEntity)target);

			if (source.CustomerId != null)
				target.CustomerId = source.CustomerId;
			if (source.SiteId != null)
				target.SiteId = source.SiteId;
			if (source.OrganizationId != null)
				target.OrganizationId = source.OrganizationId;
			if (source.EmployeeId != null)
				target.EmployeeId = source.EmployeeId;

			if (!source.Shipments.IsNullCollection())
			{
				source.Shipments.Patch(target.Shipments, new OperationComparer(),
													 (sourceShipment, targetShipment) => sourceShipment.Patch(targetShipment));
			}

			if (!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, new LineItemComparer(), (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (!source.InPayments.IsNullCollection())
			{
				source.InPayments.Patch(target.InPayments, new OperationComparer(), (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
		}
	}
}
