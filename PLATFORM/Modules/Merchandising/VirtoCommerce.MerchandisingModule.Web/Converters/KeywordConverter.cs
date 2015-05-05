using Omu.ValueInjecter;
using VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class KeywordConverter
    {
        #region Public Methods and Operators

        public static webModel.SeoKeyword ToWebModel(this SeoInfo seoInfo)
        {
            var retVal = new webModel.SeoKeyword();

			retVal.InjectFrom(seoInfo);
			retVal.Keyword = seoInfo.SemanticUrl;
			retVal.Language = seoInfo.LanguageCode;
			retVal.Title = seoInfo.PageTitle;
            return retVal;
        }

   
        #endregion
    }
}
