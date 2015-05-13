using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using dataModel = VirtoCommerce.PricingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.PricingModule.Data.Converters
{
	public static class PricelistConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Pricelist ToCoreModel(this dataModel.Pricelist dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.Pricelist();
			retVal.InjectFrom(dbEntity);
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), dbEntity.Currency);
			retVal.Prices = dbEntity.Prices.Select(x => x.ToCoreModel()).ToList();
		
			return retVal;

		}


		public static dataModel.Pricelist ToDataModel(this coreModel.Pricelist priceList)
		{
			if (priceList == null)
				throw new ArgumentNullException("priceList");

			var retVal = new dataModel.Pricelist();

			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency.ToString();

			retVal.Prices = new NullCollection<dataModel.Price>();
			if (priceList.Prices != null)
			{
				retVal.Prices = new ObservableCollection<dataModel.Price>(priceList.Prices.Select(x=>x.ToDataModel()));
			}

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.Pricelist source, dataModel.Pricelist target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<dataModel.Pricelist>(x => x.Name, x => x.Currency,
																		   x => x.Description);
			target.InjectFrom(patchInjection, source);
		
			if (!source.Prices.IsNullCollection())
			{
				source.Prices.Patch(target.Prices, (sourcePrice, targetPrice) => sourcePrice.Patch(targetPrice));
			}
		
		} 


	}
}
