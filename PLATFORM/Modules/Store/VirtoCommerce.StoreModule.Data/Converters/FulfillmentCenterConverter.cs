using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.StoreModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Store.Model;
using Omu.ValueInjecter;

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

	}
}
