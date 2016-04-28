using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class SeoInfoConverter
    {
        public static SeoInfo ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceDomainCommerceModelSeoInfo seoDto)
        {
            SeoInfo retVal = null;

            if (seoDto != null)
            {
                retVal = new SeoInfo();
                retVal.InjectFrom(seoDto);

                retVal.Slug = seoDto.SemanticUrl;
                retVal.Title = seoDto.PageTitle;
                retVal.Language = string.IsNullOrEmpty(seoDto.LanguageCode) ? Language.InvariantLanguage : new Language(seoDto.LanguageCode);
            }

            return retVal;
        }
    }
}
