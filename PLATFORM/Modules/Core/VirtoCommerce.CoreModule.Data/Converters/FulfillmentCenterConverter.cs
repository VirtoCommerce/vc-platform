using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CoreModule.Data.Converters
{
	public static class FulfillmentCenterConverter
	{

		public static coreModel.FulfillmentCenter ToCoreModel(this dataModel.FulfillmentCenter center)
		{
			var retVal = new coreModel.FulfillmentCenter();
			retVal.InjectFrom(center);
			return retVal;
		}

		public static dataModel.FulfillmentCenter ToDataModel(this coreModel.FulfillmentCenter center)
		{
			var retVal = new dataModel.FulfillmentCenter();
			retVal.InjectFrom(center);
			return retVal;
		}

	
		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.FulfillmentCenter source, dataModel.FulfillmentCenter target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<dataModel.FulfillmentCenter>(x => x.City, x => x.CountryCode,
																			   x => x.CountryName, x => x.DaytimePhoneNumber,
																			   x => x.Description, x => x.Line1,
																			   x => x.Line2, x => x.MaxReleasesPerPickBatch, x => x.Name,
																			   x => x.PickDelay, x => x.PostalCode, x => x.StateProvince);
			target.InjectFrom(patchInjection, source);
		}


	}
}