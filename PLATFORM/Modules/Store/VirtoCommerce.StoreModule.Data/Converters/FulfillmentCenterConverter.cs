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
			retVal.Id = dbEntity.FulfillmentCenterId;
		
			return retVal;

		}


		public static foundationModel.FulfillmentCenter ToFoundation(this coreModel.FulfillmentCenter fulfillCenter)
		{
			if (fulfillCenter == null)
				throw new ArgumentNullException("fulfillCenter");

			var retVal = new foundationModel.FulfillmentCenter();
			retVal.InjectFrom(fulfillCenter);

			if (fulfillCenter.Id != null)
			{
				retVal.FulfillmentCenterId = fulfillCenter.Id;
			}

			return retVal;
		}
	}
}
