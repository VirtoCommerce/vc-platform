using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using System.Collections.ObjectModel;
using foundationModel = VirtoCommerce.PricingModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.PricingModule.Data.Converters
{
	public static class PricelistConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Pricelist ToCoreModel(this foundationModel.Pricelist dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.Pricelist();
			retVal.InjectFrom(dbEntity);
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), dbEntity.Currency);
			retVal.Prices = dbEntity.Prices.Select(x => x.ToCoreModel()).ToList();
		
			return retVal;

		}


		public static foundationModel.Pricelist ToFoundation(this coreModel.Pricelist priceList)
		{
			if (priceList == null)
				throw new ArgumentNullException("priceList");

			var retVal = new foundationModel.Pricelist();

			retVal.InjectFrom(priceList);
			retVal.Currency = priceList.Currency.ToString();

			retVal.Prices = new NullCollection<foundationModel.Price>();
			if (priceList.Prices != null)
			{
				retVal.Prices = new ObservableCollection<foundationModel.Price>(priceList.Prices.Select(x=>x.ToFoundation()));
				foreach(var price in retVal.Prices)
				{
					price.PricelistId = retVal.Id;
				}
			}

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Pricelist source, foundationModel.Pricelist target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundationModel.Pricelist>(x => x.Name, x => x.Currency,
																		   x => x.Description);
			target.InjectFrom(patchInjection, source);
		
			if (!source.Prices.IsNullCollection())
			{
				source.Prices.Patch(target.Prices, (sourcePrice, targetPrice) => sourcePrice.Patch(targetPrice));
			}
		
		} 


	}
}
