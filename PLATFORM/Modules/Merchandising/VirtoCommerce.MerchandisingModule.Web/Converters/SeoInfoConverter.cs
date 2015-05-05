using System;
using System.Collections.Generic;
using coreModel = VirtoCommerce.Domain.Store.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
	public static class SeoInfoConverter
	{
		public static webModel.SeoKeyword ToWebModel(this coreModel.SeoInfo seoInfo)
		{
			var retVal = new webModel.SeoKeyword();
			retVal.InjectFrom(seoInfo);
			retVal.Keyword = seoInfo.SemanticUrl;
			retVal.Language = seoInfo.LanguageCode;
			retVal.Title = seoInfo.PageTitle;
			
			return retVal;
		}
				
	}

}
