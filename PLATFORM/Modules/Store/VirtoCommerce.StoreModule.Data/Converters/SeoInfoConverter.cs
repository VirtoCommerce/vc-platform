using System;
using System.Collections.Generic;
using coreModel = VirtoCommerce.Domain.Store.Model;
using dataModel = VirtoCommerce.StoreModule.Data.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.StoreModule.Data.Converters
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
				
	}

}
