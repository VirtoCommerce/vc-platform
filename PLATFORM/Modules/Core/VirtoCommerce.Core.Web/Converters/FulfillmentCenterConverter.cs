using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Fulfillment.Model;
using webModel = VirtoCommerce.CoreModule.Web.Model;


namespace VirtoCommerce.CoreModule.Web.Converters
{
	public static class FulfillmentCenterConverter
	{
		public static webModel.FulfillmentCenter ToWebModel(this coreModel.FulfillmentCenter center)
		{
			var retVal = new webModel.FulfillmentCenter();
			retVal.InjectFrom(center);

			return retVal;
		}

		public static coreModel.FulfillmentCenter ToCoreModel(this webModel.FulfillmentCenter center)
		{
			var retVal = new coreModel.FulfillmentCenter();
			retVal.InjectFrom(center);
			return retVal;
		}

	}
}