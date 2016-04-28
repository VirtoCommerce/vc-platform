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

		public static coreModel.SeoInfo ToCoreModel(this dataModel.SeoUrlKeyword urlKeyword)
		{
			var retVal = new coreModel.SeoInfo();
			retVal.InjectFrom(urlKeyword);
			retVal.LanguageCode = urlKeyword.Language;
			retVal.SemanticUrl = urlKeyword.Keyword;
			retVal.PageTitle = urlKeyword.Title;
			return retVal;
		}

		public static dataModel.SeoUrlKeyword ToDataModel(this coreModel.SeoInfo seo)
		{
			var retVal = new dataModel.SeoUrlKeyword();
			retVal.InjectFrom(seo);

			retVal.Keyword = seo.SemanticUrl;
			retVal.Language = seo.LanguageCode;
			retVal.Title = seo.PageTitle;

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
			var patchInjection = new PatchInjection<dataModel.SeoUrlKeyword>(true, x => x.ImageAltDescription, x => x.IsActive,
																			 x => x.Keyword,  x => x.Language, x=> x.StoreId,
																			 x => x.MetaDescription, x => x.MetaKeywords, x => x.Title);
			target.InjectFrom(patchInjection, source);
		}


	}

    public class SeoUrlKeywordComparer : IEqualityComparer<dataModel.SeoUrlKeyword>
    {
        #region IEqualityComparer<dataModel.SeoUrlKeyword> Members

        public bool Equals(dataModel.SeoUrlKeyword x, dataModel.SeoUrlKeyword y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(dataModel.SeoUrlKeyword obj)
        {
            var result = obj.Id;
            if (String.IsNullOrEmpty(result))
            {
                result = String.Join(":", obj.StoreId, obj.ObjectId, obj.ObjectType, obj.Language);
            }

            return result.GetHashCode();
        }


        #endregion
    }
}