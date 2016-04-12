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

            retVal.SeoInfo = category.SeoInfos.GetBestMatchedSeoInfo(store, currentLanguage).ToWebModel();
            retVal.Url = "~/" + category.Outlines.GetSeoPath(store, currentLanguage, "category/" + category.Id);

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
