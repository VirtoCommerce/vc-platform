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
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using cart = VirtoCommerce.Domain.Cart.Model;

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

			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			
			if(entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
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

		public static CustomerOrder ToCustomerOrder(this cart.ShoppingCart cart)
		{
			if (cart == null)
				throw new ArgumentNullException("cart");

			var retVal = new CustomerOrder()
			{
				Currency = cart.Currency,
				CustomerId = cart.CustomerId,
				StoreId = cart.StoreId,
				OrganizationId = cart.OrganizationId
			};

			if(cart.Items != null)
			{
				retVal.Items = cart.Items.Select(x => x.ToCoreModel()).ToList();
			}
			if (cart.Discounts != null)
			{
				retVal.Discount = cart.Discounts.First().ToCoreModel();
			}
			if (cart.Addresses != null)
			{
				retVal.Addresses = cart.Addresses.Select(x => x.ToCoreModel()).ToList();
			}
			if (cart.Shipments != null)
			{
				retVal.Shipments = cart.Shipments.Select(x => x.ToCoreModel()).ToList();
			}
			if (cart.Payments != null)
			{
				retVal.InPayments = new List<PaymentIn>();
				foreach(var payment in cart.Payments)
				{
					var paymentIn = payment.ToCoreModel();
					paymentIn.CustomerId = cart.CustomerId;
					retVal.InPayments.Add(paymentIn);
				}
			}
			return retVal;
		}

		public static CustomerOrderEntity ToEntity(this CustomerOrder order)
		{
			if (order == null)
				throw new ArgumentNullException("order");

			var retVal = new CustomerOrderEntity();
			retVal.InjectFrom(order);

			retVal.Currency = order.Currency.ToString();
		
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
				foreach(var address in retVal.Shipments.SelectMany(x=>x.Addresses))
				{
					address.CustomerOrder = retVal;
				}
			}
			if(order.InPayments != null)
			{
				retVal.InPayments = new ObservableCollection<PaymentInEntity>(order.InPayments.Select(x => x.ToEntity()));
				foreach (var address in retVal.InPayments.SelectMany(x => x.Addresses))
				{
					address.CustomerOrder = retVal;
				}
			}
			if(order.Discount != null)
			{
				retVal.Discounts = new ObservableCollection<DiscountEntity>(new DiscountEntity[] { order.Discount.ToEntity() });
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

			var patchInjectionPolicy = new PatchInjection<CustomerOrderEntity>(x => x.CustomerId, x => x.StoreId,
																		       x => x.OrganizationId, x => x.EmployeeId);
			target.InjectFrom(patchInjectionPolicy, source);

			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (!source.Shipments.IsNullCollection())
			{
				source.Shipments.Patch(target.Shipments, (sourceShipment, targetShipment) => sourceShipment.Patch(targetShipment));
			}

			if (!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (!source.InPayments.IsNullCollection())
			{
				source.InPayments.Patch(target.InPayments, (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}

			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
		}
	}
}
