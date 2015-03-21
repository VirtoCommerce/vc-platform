using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Fulfillment.Model;
using webModel = VirtoCommerce.CoreModule.Web.Model;
using foundation = VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;


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

		public static foundation.FulfillmentCenter ToFoundation(this coreModel.FulfillmentCenter center)
		{
			var retVal = new foundation.FulfillmentCenter();
			retVal.InjectFrom(center);

			if(center.Id != null)
				retVal.FulfillmentCenterId = center.Id;

			return retVal;
		}

		public static coreModel.FulfillmentCenter ToCoreModel(this foundation.FulfillmentCenter center)
		{
			var retVal = new coreModel.FulfillmentCenter();
			retVal.InjectFrom(center);
			retVal.Id = center.FulfillmentCenterId;
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.FulfillmentCenter source, foundation.FulfillmentCenter target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundation.FulfillmentCenter>(x => x.City, x => x.CountryCode,
																			   x => x.CountryName, x => x.DaytimePhoneNumber,
																			   x => x.Description, x => x.Line1,
																			   x => x.Line2, x => x.MaxReleasesPerPickBatch, x => x.Name,
																			   x => x.PickDelay, x => x.PostalCode, x => x.StateProvince);
			target.InjectFrom(patchInjection, source);
		}


	}
}