using System;
using System.Collections.Generic;
using module = VirtoCommerce.Domain.Catalog.Model;
using foundation = VirtoCommerce.CatalogModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.CatalogModule.Data.Converters
{
	public static class SeoInfoConverter
	{
        /// <summary>
        /// Converting to model type
        /// </summary>
        /// <param name="dbSeoInfo">The database seo information.</param>
        /// <returns></returns>
		public static module.SeoInfo ToModuleModel(this foundation.SeoUrlKeyword dbSeoInfo)
		{
			var retVal = new module.SeoInfo();
			retVal.InjectFrom(dbSeoInfo);

			retVal.SemanticUrl = dbSeoInfo.Keyword;
			retVal.LanguageCode = dbSeoInfo.Language;
			retVal.PageTitle = dbSeoInfo.Title;

			return retVal;
		}

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="seoInfo">The seo information.</param>
        /// <param name="product">The product.</param>
        /// <returns></returns>
		public static foundation.SeoUrlKeyword ToFoundation(this module.SeoInfo seoInfo, module.CatalogProduct product)
		{
			var retVal = seoInfo.ToFoundation();
			retVal.KeywordValue = product.Id;
			retVal.KeywordType = (int)module.SeoUrlKeywordTypes.Item;
			return retVal;
		}

        /// <summary>
        /// Converting to foundation type
        /// </summary>
        /// <param name="seoInfo">The seo information.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
		public static foundation.SeoUrlKeyword ToFoundation(this module.SeoInfo seoInfo, module.Category category)
		{
			var retVal = seoInfo.ToFoundation();
			retVal.KeywordValue = category.Id;
			retVal.KeywordType = (int)module.SeoUrlKeywordTypes.Category;
			return retVal;
		}

		private static foundation.SeoUrlKeyword ToFoundation(this module.SeoInfo seoInfo)
		{
			var retVal = new foundation.SeoUrlKeyword();
			retVal.InjectFrom(seoInfo);
			retVal.Keyword = seoInfo.SemanticUrl;
			retVal.Language = seoInfo.LanguageCode;
			retVal.Title = seoInfo.PageTitle;
			retVal.IsActive = true;
		
			return retVal;
		}

		/// <summary>
		/// Patch CatalogLanguage type
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundation.SeoUrlKeyword source, foundation.SeoUrlKeyword target)
		{
			if (target == null)
				throw new ArgumentNullException("target");

			var patchInjectionPolicy = new PatchInjection<foundation.SeoUrlKeyword>(x => x.Keyword, x => x.MetaDescription,
																			  x => x.ImageAltDescription, x => x.Title);
			target.InjectFrom(patchInjectionPolicy, source);
	
		}

	}

}
