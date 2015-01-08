using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using coreModel = VirtoCommerce.Domain.Cart.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;

namespace VirtoCommerce.CatalogModule.Web.Converters
{
	public static class ShipmentConverter
	{
		public static webModel.Shipment ToWebModel(this coreModel.Shipment shipment)
		{
			var retVal = new webModel.Shipment();
			retVal.InjectFrom(shipment);
			retVal.Currency = shipment.Currency;
			if(shipment.RecipientAddress != null)
				retVal.RecipientAddress = shipment.RecipientAddress.ToWebModel();
			if(shipment.Discounts != null)
				retVal.Discounts = shipment.Discounts.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.Shipment ToCoreModel(this webModel.Shipment shipment)
		{
			var retVal = new coreModel.Shipment();
			retVal.InjectFrom(shipment);
		
			if (retVal.IsTransient())
			{
				retVal.Id = Guid.NewGuid().ToString();
			}
			retVal.Currency = shipment.Currency;
		
			if (shipment.RecipientAddress != null)
				retVal.RecipientAddress = shipment.RecipientAddress.ToCoreModel();
			if(shipment.Discounts != null)
				retVal.Discounts = shipment.Discounts.Select(x => x.ToCoreModel()).ToList();
			return retVal;
		}


	}
}
