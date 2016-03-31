using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CategoryConverter
    {
        public static Category ToWebModel(this VirtoCommerceCatalogModuleWebModelCategory category, Language currentLanguage, Store store, VirtoCommerceCatalogModuleWebModelProduct[] products = null)
        {
            var retVal = new Category();
            retVal.InjectFrom<NullableAndEnumValueInjecter>(category);

            retVal.Url = "~/category/" + category.Id;

            if (category.SeoInfos != null)
            {
                //Select best matched SEO by StoreId and Language
                var bestMatchedSeo = category.SeoInfos.GetBestMatchedSeoInfo(store, currentLanguage);
                if (bestMatchedSeo != null)
                {
                    retVal.SeoInfo = bestMatchedSeo.ToWebModel();

                    if (store.SeoLinksType != SeoLinksType.None)
                    {
                        var seoPath = store.SeoLinksType == SeoLinksType.Short ? retVal.SeoInfo.Slug : category.GetSeoPath(store, currentLanguage, null);
                        if (seoPath != null)
                        {
                            retVal.Url = "~/" + seoPath;
                        }
                    }
                }
            }

            if (category.Images != null)
            {
                retVal.Images = category.Images.Select(i => i.ToWebModel()).ToArray();
                retVal.PrimaryImage = retVal.Images.FirstOrDefault();
            }

            if (category.Properties != null)
            {
                retVal.Properties = category.Properties
                    .Where(x => string.Equals(x.Type, "Category", StringComparison.OrdinalIgnoreCase))
                    .Select(p => p.ToWebModel(currentLanguage))
                    .ToList();
            }

            return retVal;
        }
    }
}
