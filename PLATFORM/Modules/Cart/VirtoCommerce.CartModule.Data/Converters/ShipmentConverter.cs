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
	public static class ShipmentConverter
	{
		public static Shipment ToCoreModel(this ShipmentEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new Shipment();
			retVal.InjectFrom(entity);
			
			retVal.Currency = entity.Currency;
			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.DeliveryAddress = entity.Addresses.First().ToCoreModel();
			}
					
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
			retVal.TaxDetails = entity.TaxDetails.Select(x => x.ToCoreModel()).ToList();
            retVal.Discounts = entity.Discounts.Select(x => x.ToCoreModel()).ToList();
            return retVal;
		}

		public static ShipmentEntity ToDataModel(this Shipment shipment, ShoppingCartEntity cartEntity, PrimaryKeyResolvingMap pkMap)
		{
			if (shipment == null)
				throw new ArgumentNullException("shipment");

			var retVal = new ShipmentEntity();
            pkMap.AddPair(shipment, retVal);

            retVal.InjectFrom(shipment);
	
			retVal.Currency = shipment.Currency.ToString();
		
			if (shipment.DeliveryAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { shipment.DeliveryAddress.ToDataModel() });
			}
			if (shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<ShipmentItemEntity>(shipment.Items.Select(x => x.ToDataModel(cartEntity, pkMap)));
			}
			if (shipment.TaxDetails != null)
			{
				retVal.TaxDetails = new ObservableCollection<TaxDetailEntity>();
				retVal.TaxDetails.AddRange(shipment.TaxDetails.Select(x => x.ToDataModel()));
			}
            if (shipment.Discounts != null)
            {
                retVal.Discounts = new ObservableCollection<DiscountEntity>();
                retVal.Discounts.AddRange(shipment.Discounts.Select(x => x.ToDataModel(pkMap)));
            }
            return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShipmentEntity source, ShipmentEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjection = new PatchInjection<ShipmentEntity>(x => x.ShipmentMethodCode, x => x.Total,
                                                                    x => x.ShippingPrice, x=> x.DiscountTotal, x => x.TaxTotal,
                                                                    x => x.TaxIncluded, x => x.Currency,
                                                                    x => x.WeightUnit, x => x.WeightValue,
                                                                    x => x.DimensionHeight, x => x.DimensionLength, x => x.DimensionUnit,
                                                                    x => x.DimensionWidth, x => x.TaxType);
			target.InjectFrom(patchInjection, source);

		
			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
			if (source.Items != null)
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
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
