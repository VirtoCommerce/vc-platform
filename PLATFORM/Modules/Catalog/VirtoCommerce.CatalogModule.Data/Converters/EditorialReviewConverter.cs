using System;
using System.Collections.Generic;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.Domain.Catalog.Model;

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

			var retVal = new module.EditorialReview
			{
				Id = dbReview.EditorialReviewId,
				Content = dbReview.Content,
				LanguageCode = dbReview.Locale,
				ReviewType = dbReview.Source
			};
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

			var retVal = new foundation.EditorialReview
			{
				ItemId = product.Id,
				Content = review.Content,
				Source = review.ReviewType,
				ReviewState = (int)foundation.ReviewState.Active,
				Locale = review.LanguageCode
			};

            if (review.Id != null)
			{
				retVal.EditorialReviewId = review.Id;
			}
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

			//Simply properties patch
			if (source.Content != null)
				target.Content = source.Content;
			if (source.Locale != null)
				target.Locale = source.Locale;
			if (source.Source != null)
				target.Source = source.Source;
		}
	}

	public class EditorialReviewComparer : IEqualityComparer<foundation.EditorialReview>
	{
		#region IEqualityComparer<EditorialReview> Members

		public bool Equals(foundation.EditorialReview x, foundation.EditorialReview y)
		{
			return x.EditorialReviewId == y.EditorialReviewId;
		}

		public int GetHashCode(foundation.EditorialReview obj)
		{
			return obj.EditorialReviewId.GetHashCode();
		}

		#endregion
	}
}
