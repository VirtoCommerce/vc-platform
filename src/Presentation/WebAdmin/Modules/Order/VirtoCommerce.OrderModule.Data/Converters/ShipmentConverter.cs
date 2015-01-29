using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using VirtoCommerce.Foundation.Money;

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
			if (entity.Currency != null)
			{
				retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), entity.Currency);
			}

			if (entity.DeliveryAddress != null)
			{
				retVal.DeliveryAddress = entity.DeliveryAddress.ToCoreModel();
			}
			if(entity.Discount != null)
			{
				retVal.Discount = entity.Discount.ToCoreModel();
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
			if(shipment.Items != null)
			{
				retVal.Items = new ObservableCollection<LineItemEntity>(shipment.Items.Select(x=>x.ToEntity()));
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
		
			if (!source.InPayments.IsNullCollection())
			{
				source.InPayments.Patch(target.InPayments, new OperationComparer(), (sourcePayment, targetPayment) => sourcePayment.Patch(targetPayment));
			}
			if(!source.Items.IsNullCollection())
			{
				source.Items.Patch(target.Items, new LineItemComparer(), (sourceItem, targetItem) => sourceItem.Patch(targetItem));
			}
		}
	}
}
