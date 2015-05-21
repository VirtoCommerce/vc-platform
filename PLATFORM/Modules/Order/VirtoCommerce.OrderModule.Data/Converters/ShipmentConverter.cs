using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
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

			if (entity.Properties != null)
			{
				retVal.Properties = entity.Properties.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.Addresses != null && entity.Addresses.Any())
			{
				retVal.DeliveryAddress = entity.Addresses.First().ToCoreModel();
			}
			if(entity.Discounts != null && entity.Discounts.Any())
			{
				retVal.Discount = entity.Discounts.First().ToCoreModel();
			}
			if (entity.Items != null)
			{
				retVal.Items = entity.Items.Select(x => x.ToCoreModel()).ToList();
			}
			if (entity.InPayments != null)
			{
				retVal.InPayments = entity.InPayments.Select(x => x.ToCoreModel()).ToList();
			}

			return retVal;
		}

		public static Shipment ToCoreModel(this coreModel.Shipment shipment)
		{

			var retVal = new Shipment();
			retVal.InjectFrom(shipment);
			retVal.Currency = shipment.Currency;
			retVal.Sum = shipment.ShippingPrice;
			retVal.Tax = shipment.TaxTotal;

			if (shipment.Weight != null)
			{
				retVal.Weight = new Weight
				{
					Unit = shipment.Weight.Unit,
					Value = shipment.Weight.Value
				};
			}

			if (shipment.Dimension != null)
			{
				retVal.Dimension = new Dimension
				{
					Height = shipment.Dimension.Height,
					Length = shipment.Dimension.Length,
					Unit = shipment.Dimension.Unit,
					Width = shipment.Dimension.Width
				};
			}
		
			if(shipment.DeliveryAddress != null)
			{
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToCoreModel();
			}
			if(shipment.Items != null)
			{
				retVal.Items = shipment.Items.Select(x => x.ToCoreModel()).ToList();
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

			if (shipment.Properties != null)
			{
				retVal.Properties = new ObservableCollection<OperationPropertyEntity>(shipment.Properties.Select(x => x.ToDataModel()));
			}

			//Allow to empty address
			retVal.Addresses = new ObservableCollection<AddressEntity>();
			if (shipment.DeliveryAddress != null)
			{
				retVal.Addresses = new ObservableCollection<AddressEntity>(new AddressEntity[] { shipment.DeliveryAddress.ToDataModel() });
			}
			if(shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(shipment.Items.Select(x=>x.ToDataModel()));
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

			source.Patch((OperationEntity)target);

			var patchInjectionPolicy = new PatchInjection<ShipmentEntity>(x => x.FulfillmentCenterId, x => x.OrganizationId, x => x.EmployeeId);
			target.InjectFrom(patchInjectionPolicy, source);

			if (!source.InPayments.IsNullCollection())
			{
				source.InPayments.Patch(target.InPayments, (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
			if(!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}
			if (!source.Discounts.IsNullCollection())
			{
				source.Discounts.Patch(target.Discounts, new DiscountComparer(), (sourceDiscount, targetDiscount) => sourceDiscount.Patch(targetDiscount));
			}
			if (!source.Addresses.IsNullCollection())
			{
				source.Addresses.Patch(target.Addresses, new AddressComparer(), (sourceAddress, targetAddress) => sourceAddress.Patch(targetAddress));
			}
		}
	}
}
