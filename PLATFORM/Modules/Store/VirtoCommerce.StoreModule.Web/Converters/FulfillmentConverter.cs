using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using webModel = VirtoCommerce.StoreModule.Web.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.StoreModule.Web.Converters
{
	public static class FulfillmentConverter
	{
		public static webModel.FulfillmentCenter ToWebModel(this coreModel.FulfillmentCenter fulfillment)
		{
			var retVal = new webModel.FulfillmentCenter();
			retVal.InjectFrom(fulfillment);

			return retVal;
		}

		public static coreModel.FulfillmentCenter ToCoreModel(this webModel.FulfillmentCenter fulfillment)
		{
			var retVal = new coreModel.FulfillmentCenter();
			retVal.InjectFrom(fulfillment);

			return retVal;
		}


	}
}