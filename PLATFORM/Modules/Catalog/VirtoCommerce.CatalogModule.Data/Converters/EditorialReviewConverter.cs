using System;
using System.Collections.Generic;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class EditorialReviewConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.EditorialReview ToCoreModel(this dataModel.EditorialReview dbReview)
		{
			if (dbReview == null)
				throw new ArgumentNullException("dbReview");

			var retVal = new coreModel.EditorialReview();
			retVal.InjectFrom(dbReview);
			retVal.LanguageCode = dbReview.Locale;
			retVal.ReviewType = dbReview.Source;
			return retVal;

		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="itemAsset">The item asset.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">itemAsset</exception>
		public static dataModel.EditorialReview ToDataModel(this coreModel.EditorialReview review, dataModel.Item product)
		{
			if (review == null)
				throw new ArgumentNullException("review");

			var retVal = new dataModel.EditorialReview();
			var id = retVal.Id;
			retVal.InjectFrom(review);
			if(review.Id == null)
			{
				retVal.Id = id;
			}
			retVal.ItemId = product.Id;
			retVal.Source = review.ReviewType;
			retVal.ReviewState = (int)coreModel.ReviewState.Active;
			retVal.Locale = review.LanguageCode;

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.EditorialReview source, dataModel.EditorialReview target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<dataModel.EditorialReview>(x => x.Content, x => x.Locale, x=>x.Source);
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}

}
