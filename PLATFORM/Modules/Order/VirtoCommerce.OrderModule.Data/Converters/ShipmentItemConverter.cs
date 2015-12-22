using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;
using Omu.ValueInjecter;
using VirtoCommerce.OrderModule.Data.Model;
using cartCoreModel = VirtoCommerce.Domain.Cart.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.OrderModule.Data.Converters
{
	public static class ShipmentItemConverter
	{
		public static ShipmentItem ToCoreModel(this ShipmentItemEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException("entity");

			var retVal = new ShipmentItem();
			retVal.InjectFrom(entity);

			if(entity.LineItem != null)
			{
				retVal.LineItem = entity.LineItem.ToCoreModel();
			}
			return retVal;
		}

		public static ShipmentItem ToCoreShipmentItemModel(this cartCoreModel.LineItem lineItem)
		{
			if (lineItem == null)
				throw new ArgumentNullException("lineItem");

			var retVal = new ShipmentItem();
			retVal.InjectFrom(lineItem);

			retVal.LineItem = lineItem.ToCoreModel();
			return retVal;
		}


		public static ShipmentItemEntity ToDataModel(this ShipmentItem shipmentItem, CustomerOrderEntity orderEntity, PrimaryKeyResolvingMap pkMap)
		{
			if (shipmentItem == null)
				throw new ArgumentNullException("shipmentItem");

			var retVal = new ShipmentItemEntity();
            pkMap.AddPair(shipmentItem, retVal);
            retVal.InjectFrom(shipmentItem);
			
			if(shipmentItem.LineItem != null)
			{
				retVal.LineItem = orderEntity.Items.FirstOrDefault(x => x.ProductId == shipmentItem.LineItem.ProductId);
			}
			return retVal;
		}

		/// <summary>
		/// Patch CatalogBase type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this ShipmentItemEntity source, ShipmentItemEntity target)
		{
			if (target == null)
				throw new ArgumentNullException("target");


			var patchInjectionPolicy = new PatchInjection<ShipmentItemEntity>(x => x.BarCode, x => x.ShipmentId, x => x.ShipmentPackageId, x=>x.Quantity);
			target.InjectFrom(patchInjectionPolicy, source);
		}

	}
}
