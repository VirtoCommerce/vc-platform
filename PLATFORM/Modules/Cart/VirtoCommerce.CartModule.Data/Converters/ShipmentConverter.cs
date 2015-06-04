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
			
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);

			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.DeliveryAddress = entity.Addresses.First().ToCoreModel();
			}
					
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
		
			return retVal;
		}

		public static ShipmentEntity ToDataModel(this Shipment shipment)
		{
			if (shipment == null)
				throw new ArgumentNullException("shipment");

			var retVal = new ShipmentEntity();
			retVal.InjectFrom(shipment);
	
			retVal.Currency = shipment.Currency.ToString();
		
			if (shipment.DeliveryAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { shipment.DeliveryAddress.ToDataModel() });
			}
			if (shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(shipment.Items.Select(x => x.ToDataModel()));
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

			var patchInjection = new PatchInjection<ShipmentEntity>(x => x.ShippingPrice, x => x.TaxTotal,
																		   x => x.TaxIncluded, x => x.Currency,
																		   x => x.WeightUnit, x => x.WeightValue,
																		   x => x.DimensionHeight, x => x.DimensionLength, x => x.DimensionUnit,
																		   x => x.DimensionWidth);
			target.InjectFrom(patchInjection, source);

		
			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}

			if (source.Items != null)
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

		}

	}
}
