using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.Foundation.Stores.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;

namespace VirtoCommerce.StoreModule.Data.Converters
{
	public static class FulfillmentCenterConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.FulfillmentCenter ToCoreModel(this foundationModel.FulfillmentCenter dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.FulfillmentCenter();
			retVal.InjectFrom(dbEntity);
		
			return retVal;

		}


		public static foundationModel.FulfillmentCenter ToFoundation(this coreModel.FulfillmentCenter fulfillCenter)
		{
			if (fulfillCenter == null)
				throw new ArgumentNullException("fulfillCenter");

			var retVal = new foundationModel.FulfillmentCenter();
			retVal.InjectFrom(fulfillCenter);
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.FulfillmentCenter source, foundationModel.FulfillmentCenter target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<foundationModel.FulfillmentCenter>(x => x.City, x => x.CountryCode, x => x.CountryName, x => x.DaytimePhoneNumber,
																							x => x.Description, x => x.Line2, x => x.Line1, x => x.MaxReleasesPerPickBatch, x => x.Name,
																							x => x.PickDelay, x => x.PostalCode);
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}
}
