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
using taxCoreModel = VirtoCommerce.Domain.Tax.Model;

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
            retVal.Currency = entity.Currency;

            retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			retVal.Addresses = entity.Addresses.Select(x => x.ToCoreModel()).ToList();
			retVal.Shipments = entity.Shipments.Select(x => x.ToCoreModel()).ToList();
			retVal.Payments = entity.Payments.Select(x => x.ToCoreModel()).ToList();
			retVal.TaxDetails = entity.TaxDetails.Select(x => x.ToCoreModel()).ToList();
            retVal.Discounts = entity.Discounts.Select(x => x.ToCoreModel()).ToList();

            return retVal;
		}

     
        public static ShoppingCartEntity ToDataModel(this ShoppingCart cart, PrimaryKeyResolvingMap pkMap)
		{
			if (cart == null)
				throw new ArgumentNullException("cart");

			var retVal = new ShoppingCartEntity();
            pkMap.AddPair(cart, retVal);

            retVal.InjectFrom(cart);

			retVal.Currency = cart.Currency;

			if (cart.Addresses != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(cart.Addresses.Select(x => x.ToDataModel()));
			}
			if (cart.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(cart.Items.Select(x => x.ToDataModel(pkMap)));
			}
			if (cart.Shipments != null)
			{
				retVal.Shipments = new ObservableCollection<ShipmentEntity>(cart.Shipments.Select(x => x.ToDataModel(retVal, pkMap)));
			}
			if (cart.Payments != null)
			{
				retVal.Payments = new ObservableCollection<PaymentEntity>(cart.Payments.Select(x => x.ToDataModel(pkMap)));
			}
			if (cart.TaxDetails != null)
			{
				retVal.TaxDetails = new ObservableCollection<TaxDetailEntity>();
				retVal.TaxDetails.AddRange(cart.TaxDetails.Select(x => x.ToDataModel()));
			}
            if (cart.Discounts != null)
            {
                retVal.Discounts = new ObservableCollection<DiscountEntity>();
                retVal.Discounts.AddRange(cart.Discounts.Select(x => x.ToDataModel(pkMap)));
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

			var patchInjectionPolicy = new PatchInjection<ShoppingCartEntity>(x => x.Currency, x => x.Name, x=> x.ValidationType,
																						  x => x.CustomerId, x => x.CustomerName,
																						  x => x.IsAnonymous, x => x.IsRecuring, x => x.LanguageCode, x => x.Comment,
																						  x => x.OrganizationId, x => x.Total, x => x.SubTotal, x => x.ShippingTotal,
																						  x => x.HandlingTotal, x => x.DiscountTotal, x => x.TaxTotal, x => x.Coupon);
			target.InjectFrom(patchInjectionPolicy, source);

			if (string.IsNullOrEmpty(source.Coupon))
            {
                target.Coupon = null;
            }

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

			if (!source.TaxDetails.IsNullCollection())
			{
				var taxDetailComparer = AnonymousComparer.Create((TaxDetailEntity x) => x.Name);
				source.TaxDetails.Patch(target.TaxDetails, taxDetailComparer, (sourceTaxDetail, targetTaxDetail) => sourceTaxDetail.Patch(targetTaxDetail));
			}
            if (!source.Discounts.IsNullCollection())
            {
                source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
            }
        }

	}
}
