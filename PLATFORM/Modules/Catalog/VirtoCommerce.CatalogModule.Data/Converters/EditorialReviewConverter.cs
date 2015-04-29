using System;
using System.Collections.Generic;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using module = VirtoCommerce.Domain.Catalog.Model;
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
		public static module.EditorialReview ToModuleModel(this foundation.EditorialReview dbReview)
		{
			if (dbReview == null)
				throw new ArgumentNullException("dbReview");

			var retVal = new module.EditorialReview();
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
		public static foundation.EditorialReview ToFoundation(this module.EditorialReview review, module.CatalogProduct product)
		{
			if (review == null)
				throw new ArgumentNullException("review");

			var retVal = new foundation.EditorialReview();
			retVal.InjectFrom(review);
			retVal.ItemId = product.Id;
			retVal.Source = review.ReviewType;
			retVal.ReviewState = (int)module.ReviewState.Active;
			retVal.Locale = review.LanguageCode;

			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.EditorialReview source, foundation.EditorialReview target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<foundation.EditorialReview>(x => x.Content, x => x.Locale, x=>x.Source);
			target.InjectFrom(patchInjectionPolicy, source);

		}
	}

}
