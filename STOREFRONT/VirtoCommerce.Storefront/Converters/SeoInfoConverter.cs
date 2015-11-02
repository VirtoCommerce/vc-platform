using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class SeoInfoConverter
    {
        public static SeoInfo ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceDomainCommerceModelSeoInfo seoDto)
        {
            var retVal = new SeoInfo();
            retVal.InjectFrom(seoDto);
            retVal.Slug = seoDto.SemanticUrl;
            retVal.Title = seoDto.PageTitle;
            retVal.Language = seoDto.LanguageCode;
            retVal.ImageAltDescription = seoDto.ImageAltDescription;
            return retVal;
        }
    }
}