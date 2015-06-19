using System;
using System.Collections.Generic;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class SeoInfoConverter
	{
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="seoUrlKeyword">The database seo information.</param>
        /// <returns></returns>
		public static coreModel.SeoInfo ToCoreModel(this SeoUrlKeyword seoUrlKeyword)
		{
			var retVal = new coreModel.SeoInfo();
			retVal.InjectFrom(seoUrlKeyword);

			retVal.SemanticUrl = seoUrlKeyword.Keyword;
			retVal.LanguageCode = seoUrlKeyword.Language;
			retVal.PageTitle = seoUrlKeyword.Title;

			return retVal;
		}

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="seoInfo">The seo information.</param>
        /// <param name="product">The product.</param>
        /// <returns></returns>
		public static SeoUrlKeyword ToCoreModel(this coreModel.SeoInfo seoInfo, dataModel.Item product)
		{
			var retVal = seoInfo.ToCoreModel();
			retVal.KeywordValue = product.Id;
			retVal.KeywordType = (int)coreModel.SeoUrlKeywordTypes.Item;
			return retVal;
		}

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="seoInfo">The seo information.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
		public static SeoUrlKeyword ToCoreModel(this coreModel.SeoInfo seoInfo, dataModel.CategoryBase category)
		{
			var retVal = seoInfo.ToCoreModel();
			retVal.KeywordValue = category.Id;
			retVal.KeywordType = (int)coreModel.SeoUrlKeywordTypes.Category;
			return retVal;
		}

		/// <summary>
		/// Converting to foundation type
		/// </summary>
		/// <param name="seoInfo">The seo information.</param>
		/// <param name="product">The product.</param>
		/// <returns></returns>
		public static SeoUrlKeyword ToCoreModel(this coreModel.SeoInfo seoInfo, dataModel.CatalogBase catalog)
		{
			var retVal = seoInfo.ToCoreModel();
			retVal.KeywordValue = catalog.Id;
            //retVal.KeywordType = (int)coreModel.SeoUrlKeywordTypes.Catalog;
			return retVal;
		}

		private static SeoUrlKeyword ToCoreModel(this coreModel.SeoInfo seoInfo)
		{
			var retVal = new SeoUrlKeyword();
			var id = retVal.Id;
			retVal.InjectFrom(seoInfo);
			if(seoInfo.Id == null)
			{
				retVal.Id = id;
			}
			retVal.Keyword = seoInfo.SemanticUrl;
			retVal.Language = seoInfo.LanguageCode;
			retVal.Title = seoInfo.PageTitle;
			retVal.IsActive = true;
		
			return retVal;
		}
				
	}

}
