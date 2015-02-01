using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using System.Collections.ObjectModel;

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
			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}

			if (entity.DeliveryAddress != null)
			{
				retVal.DeliveryAddress = entity.DeliveryAddress.ToCoreModel();
			}
		
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
		
			return retVal;
		}

		public static ShipmentEntity ToEntity(this Shipment shipment)
		{
			if (shipment == null)
				throw new ArgumentNullException("shipment");

			var retVal = new ShipmentEntity();
			retVal.InjectFrom(shipment);
			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}
			if (shipment.Currency != null)
			{
				retVal.Currency = shipment.Currency.ToString();
			}
			if (shipment.DeliveryAddress != null)
			{
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToEntity();
			}
			if (shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(shipment.Items.Select(x => x.ToEntity()));
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

			//Simply properties patch
			target.ShippingPrice = source.ShippingPrice;
			target.TaxTotal = source.TaxTotal;

			target.TaxIncluded = source.TaxIncluded;

			if(source.Currency != null)
				target.Currency = source.Currency;

			if (source.WeightUnit != null)
				target.WeightUnit = source.WeightUnit;
			if (source.WeightValue != null)
				target.WeightValue = source.WeightValue;
			if (source.DimensionHeight != null)
				target.DimensionHeight = source.DimensionHeight;
			if (source.DimensionLength != null)
				target.DimensionLength = source.DimensionLength;
			if (source.DimensionUnit != null)
				target.DimensionUnit = source.DimensionUnit;
			if (source.DimensionWidth != null)
				target.DimensionWidth = source.DimensionWidth;

			var addressComparer = new AddressComparer();
			if (source.DeliveryAddress != null && !addressComparer.Equals(target.DeliveryAddress, source.DeliveryAddress))
				target.DeliveryAddress = source.DeliveryAddress;

			if (source.Items != null)
			{
				if (target.Items == null)
					target.Items = new ObservableCollection<LineItemEntity>();

				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}

		}

	}
}
