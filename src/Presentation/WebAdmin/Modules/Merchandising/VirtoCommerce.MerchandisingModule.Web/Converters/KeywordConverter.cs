using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using webModel = VirtoCommerce.MerchandisingModule.Web.Model;
using foundation = VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.MerchandisingModule.Web.Converters
{
    public static class KeywordConverter
    {
        public static webModel.SeoKeyword ToWebModel(this foundation.SeoUrlKeyword keyword)
        {
            var retVal = new webModel.SeoKeyword
            {
                Id = keyword.SeoUrlKeywordId
            };

            retVal.InjectFrom(keyword);

            return retVal;
        }
        public static webModel.SeoKeyword ToWebModel(this SeoInfo keyword)
        {
            var retVal = new webModel.SeoKeyword
            {
                Id = keyword.Id,
                ImageAltDescription = keyword.ImageAltDescription,
                Keyword = keyword.SemanticUrl,
                Language = keyword.LanguageCode,
                Title = keyword.PageTitle,
                MetaDescription = keyword.MetaDescription,
                MetaKeywords = keyword.MetaKeywords              
            };

            return retVal;
        }



    }
}
