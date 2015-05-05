using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CartModule.Web.Converters
{
	public static class ShipmentConverter
	{
		public static webModel.Shipment ToWebModel(this coreModel.Shipment shipment)
		{
			var retVal = new webModel.Shipment();
			retVal.InjectFrom(shipment);
			retVal.Currency = shipment.Currency;
			if(shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();
			if(shipment.Discounts != null)
				retVal.Discounts = shipment.Discounts.Select(x => x.ToWebModel()).ToList();
			if (shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.Shipment ToCoreModel(this webModel.Shipment shipment)
		{
			var retVal = new coreModel.Shipment();
			retVal.InjectFrom(shipment);
		
			retVal.Currency = shipment.Currency;
		
			if (shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToCoreModel();
			if(shipment.Discounts != null)
				retVal.Discounts = shipment.Discounts.Select(x => x.ToCoreModel()).ToList();
			if (shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}


	}
}
