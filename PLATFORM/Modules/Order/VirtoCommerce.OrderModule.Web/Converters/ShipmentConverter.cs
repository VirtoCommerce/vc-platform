using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class ShipmentConverter
	{
		public static webModel.Shipment ToWebModel(this coreModel.Shipment shipment)
		{
			var retVal = new webModel.Shipment();
			retVal.InjectFrom(shipment);


			if (shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();

			if (shipment.InPayments != null)
				retVal.InPayments = shipment.InPayments.Select(x => x.ToWebModel()).ToList();

			if(shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToWebModel()).ToList();

			if (shipment.Packages != null)
				retVal.Packages = shipment.Packages.Select(x => x.ToWebModel()).ToList();

			if (shipment.Discount != null)
			{
				retVal.Discount = shipment.Discount.ToWebModel();
		    }

            //Populate shipment method for shipment
            retVal.ShippingMethod = new webModel.ShippingMethod();
            retVal.ShippingMethod.Code = shipment.ShipmentMethodCode;
            retVal.ShippingMethod.Description = shipment.ShipmentMethodCode;
            retVal.ShippingMethod.OptionDescription = shipment.ShipmentMethodOption;
            retVal.ShippingMethod.OptionName = shipment.ShipmentMethodOption;

            if (shipment.ShippingMethod != null)
            {
                retVal.ShippingMethod = shipment.ShippingMethod.ToWebModel();
            }

            retVal.ChildrenOperations = shipment.GetFlatObjectsListWithInterface<coreModel.IOperation>().Except(new[] { shipment }).Select(x => x.ToWebModel()).ToList();

			retVal.TaxDetails = shipment.TaxDetails;

			if (shipment.DynamicProperties != null)
				retVal.DynamicProperties = shipment.DynamicProperties;

			return retVal;
		}

		public static coreModel.Shipment ToCoreModel(this webModel.Shipment shipment)
		{
			var retVal = new coreModel.Shipment();
			retVal.InjectFrom(shipment);

	
			if (shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToCoreModel();
            if(shipment.ShippingMethod != null)
            {
                retVal.ShipmentMethodCode = shipment.ShippingMethod.Code;
                retVal.ShipmentMethodOption = shipment.ShippingMethod.OptionName;
            }
			if (shipment.InPayments != null)
				retVal.InPayments = shipment.InPayments.Select(x => x.ToCoreModel()).ToList();
			if (shipment.Discount != null)
				retVal.Discount = shipment.Discount.ToCoreModel();
			if (shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToCoreModel()).ToList();
			if (shipment.Packages != null)
				retVal.Packages = shipment.Packages.Select(x => x.ToCoreModel()).ToList();
			if (shipment.DynamicProperties != null)
				retVal.DynamicProperties = shipment.DynamicProperties;

			retVal.TaxDetails = shipment.TaxDetails;
			return retVal;
		}


	}
}