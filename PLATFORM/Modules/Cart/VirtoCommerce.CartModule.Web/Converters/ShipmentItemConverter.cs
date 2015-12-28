using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CartModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class ShipmentItemConverter
	{
		public static webModel.ShipmentItem ToWebModel(this coreModel.ShipmentItem shipmentItem)
		{
			var retVal = new webModel.ShipmentItem();
			retVal.InjectFrom(shipmentItem);

			if(shipmentItem.LineItem != null)
			{
				retVal.LineItem = shipmentItem.LineItem.ToWebModel();
			}
		
			return retVal;
		}

		public static coreModel.ShipmentItem ToCoreModel(this webModel.ShipmentItem shipmentItem)
		{
			var retVal = new coreModel.ShipmentItem();
			retVal.InjectFrom(shipmentItem);

			if (shipmentItem.LineItem != null)
			{
				retVal.LineItem = shipmentItem.LineItem.ToCoreModel();
			}
			
			return retVal;
		}


	}
}