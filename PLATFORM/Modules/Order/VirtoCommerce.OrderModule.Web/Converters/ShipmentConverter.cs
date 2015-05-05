using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Order.Model;
using webModel = VirtoCommerce.OrderModule.Web.Model;

namespace VirtoCommerce.OrderModule.Web.Converters
{
	public static class ShipmentConverter
	{
		public static webModel.Shipment ToWebModel(this coreModel.Shipment shipment)
		{
			var retVal = new webModel.Shipment();
			retVal.InjectFrom(shipment);

			retVal.Organization = retVal.OrganizationId;
			retVal.FulfillmentCenter = retVal.FulfillmentCenterId;
			retVal.Employee = retVal.EmployeeId;

			if (shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToWebModel();

			if (shipment.InPayments != null)
				retVal.InPayments = shipment.InPayments.Select(x => x.ToWebModel()).ToList();

			if(shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToWebModel()).ToList();

			if (shipment.Discount != null)
			{
				retVal.Discount = shipment.Discount.ToWebModel();
				retVal.DiscountAmount = shipment.Discount.DiscountAmount;
			}

			retVal.ChildrenOperations = shipment.ChildrenOperations.Select(x => x.ToWebModel()).ToList();

			return retVal;
		}

		public static coreModel.Shipment ToCoreModel(this webModel.Shipment shipment)
		{
			var retVal = new coreModel.Shipment();
			retVal.InjectFrom(shipment);

			if (shipment.DeliveryAddress != null)
				retVal.DeliveryAddress = shipment.DeliveryAddress.ToCoreModel();
			if (shipment.InPayments != null)
				retVal.InPayments = shipment.InPayments.Select(x => x.ToCoreModel()).ToList();
			if (shipment.Discount != null)
				retVal.Discount = shipment.Discount.ToCoreModel();
			if (shipment.Items != null)
				retVal.Items = shipment.Items.Select(x => x.ToCoreModel()).ToList();

			return retVal;
		}


	}
}