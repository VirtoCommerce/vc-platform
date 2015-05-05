using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using coreModel = VirtoCommerce.Domain.Commerce.Model;
using dataModel = VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common.ConventionInjections;

namespace VirtoCommerce.CoreModule.Data.Converters
{
	public static class SeoUrlKeywordConverter
	{

		public static coreModel.SeoUrlKeyword ToCoreModel(this dataModel.SeoUrlKeyword center)
		{
			var retVal = new coreModel.SeoUrlKeyword();
			retVal.InjectFrom(center);
			return retVal;
		}

		public static dataModel.SeoUrlKeyword ToDataModel(this coreModel.SeoUrlKeyword center)
		{
			var retVal = new dataModel.SeoUrlKeyword();
			retVal.InjectFrom(center);
			return retVal;
		}

	
		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this dataModel.SeoUrlKeyword source, dataModel.SeoUrlKeyword target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<dataModel.SeoUrlKeyword>(x => x.ImageAltDescription, x => x.IsActive,
																			   x => x.Keyword, x => x.KeywordType,
																			   x => x.KeywordValue, x => x.Language,
																			   x => x.MetaDescription, x => x.MetaKeywords, x => x.Title);
			target.InjectFrom(patchInjection, source);
		}


	}
}