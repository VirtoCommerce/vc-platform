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
	public static class PriceConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.Price ToCoreModel(this foundationModel.Price dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.Price();
			retVal.InjectFrom(dbEntity);
			retVal.Id = dbEntity.PriceId;
			retVal.Currency = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), dbEntity.Pricelist.Currency);
			retVal.ProductId = dbEntity.ItemId;
			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;
			retVal.MinQuantity = (int)dbEntity.MinQuantity;
			return retVal;

		}


		public static foundationModel.Price ToFoundation(this coreModel.Price price)
		{
			if (price == null)
				throw new ArgumentNullException("price");

			var retVal = new foundationModel.Price();

			retVal.InjectFrom(price);
			retVal.ItemId = price.ProductId;
			if (price.Id != null)
			{
				retVal.PriceId = price.Id;
			}
			retVal.MinQuantity = price.MinQuantity;
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Price source, foundationModel.Price target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundationModel.Price>(x => x.ItemId, x => x.List,
																		   x => x.MinQuantity, x=>x.Sale);
			target.InjectFrom(patchInjection, source);
		}


	}
}
