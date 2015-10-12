using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreModel = VirtoCommerce.Domain.Quote.Model;
using dataModel = VirtoCommerce.QuoteModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.QuoteModule.Data.Converters
{
	public static class TierPriceConverter
	{
		public static coreModel.TierPrice ToCoreModel(this dataModel.TierPriceEntity dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.TierPrice();
			retVal.InjectFrom(dbEntity);

			return retVal;
		}


		public static dataModel.TierPriceEntity ToDataModel(this coreModel.TierPrice tierPrice)
		{
			if (tierPrice == null)
				throw new ArgumentNullException("tierPrice");

			var retVal = new dataModel.TierPriceEntity();
			retVal.InjectFrom(tierPrice);

			return retVal;
		}
	}
}
