using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundationModel = VirtoCommerce.Foundation.Catalogs.Model;
using coreModel = VirtoCommerce.Domain.Pricing.Model;

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
			retVal.Id = dbEntity.PricelistId;
			
			retVal.CreatedDate = dbEntity.Created ?? DateTime.MinValue;
			retVal.ModifiedDate = dbEntity.LastModified;
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

			if (priceList.Id != null)
			{
				retVal.PricelistId = priceList.Id;
			}

			retVal.Prices = new NullCollection<foundationModel.Price>();
			if (priceList.Prices != null)
			{
				retVal.Prices = new ObservableCollection<foundationModel.Price>(priceList.Prices.Select(x=>x.ToFoundation()));
				foreach(var price in retVal.Prices)
				{
					price.PricelistId = retVal.PricelistId;
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
				var priceComparer = AnonymousComparer.Create((foundationModel.Price x) => x.PriceId);
				source.Prices.Patch(target.Prices, priceComparer, (sourcePrice, targetPrice) => sourcePrice.Patch(targetPrice));
			}
		
		} 


	}
}
