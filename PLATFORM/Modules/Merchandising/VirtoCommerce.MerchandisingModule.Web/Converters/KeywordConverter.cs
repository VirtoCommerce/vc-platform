using Omu.ValueInjecter;
using VirtoCommerce.Domain.Catalog.Model;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class KeywordConverter
    {
        #region Public Methods and Operators

        public static webModel.SeoKeyword ToWebModel(this foundation.SeoUrlKeyword keyword)
        {
            var retVal = new webModel.SeoKeyword();
            retVal.InjectFrom(keyword);
            return retVal;
        }

        public static webModel.SeoKeyword ToWebModel(this SeoInfo keyword)
        {
            var retVal = new webModel.SeoKeyword
                         {
                             ImageAltDescription = keyword.ImageAltDescription,
                             Keyword = keyword.SemanticUrl,
                             Language = keyword.LanguageCode,
                             Title = keyword.PageTitle,
                             MetaDescription = keyword.MetaDescription,
                             MetaKeywords = keyword.MetaKeywords
                         };

            return retVal;
        }

        #endregion
    }
}
