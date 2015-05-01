using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CartModule.Data.Converters
{
	public static class ShoppingCartConverter
	{
		public static ShoppingCart ToCoreModel(this ShoppingCartEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new ShoppingCart();
			retVal.InjectFrom(entity);
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);

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
			if (entity.Payments != null)
			{
				retVal.Payments = entity.Payments.Select(x => x.ToCoreModel()).ToList();
			}

			return retVal;
		}

		public static ShoppingCartEntity ToDataModel(this ShoppingCart cart)
		{
			if (cart == null)
				throw new ArgumentNullException("cart");

			var retVal = new ShoppingCartEntity();
			retVal.InjectFrom(cart);

			retVal.Currency = cart.Currency.ToString();

			if (cart.Addresses != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(cart.Addresses.Select(x => x.ToDataModel()));
			}
			if (cart.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(cart.Items.Select(x => x.ToDataModel()));
			}
			if (cart.Shipments != null)
			{
				retVal.Shipments = new ObservableCollection<ShipmentEntity>(cart.Shipments.Select(x => x.ToDataModel()));
			}
			if (cart.Payments != null)
			{
				retVal.Payments = new ObservableCollection<PaymentEntity>(cart.Payments.Select(x => x.ToDataModel()));
			}

			return retVal;
		}


		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShoppingCartEntity source, ShoppingCartEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<ShoppingCartEntity>(x => x.Currency, x => x.Name,
																						  x => x.CustomerId, x => x.CustomerName,
																						  x => x.IsAnonymous, x => x.IsRecuring, x => x.LanguageCode, x => x.Comment,
																						  x => x.OrganizationId, x => x.Total, x => x.SubTotal, x => x.ShippingTotal,
																						  x => x.HandlingTotal, x => x.DiscountTotal, x => x.TaxTotal);
			target.InjectFrom(patchInjectionPolicy, source);

			
			if (!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

			if (!source.Payments.IsNullCollection())
			{
				source.Payments.Patch(target.Payments, (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}

			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}

			if (!source.Shipments.IsNullCollection())
			{
				source.Shipments.Patch(target.Shipments, (sourceShipment, targetShipment) => sourceShipment.Patch(targetShipment));
			}
		}

	}
}
