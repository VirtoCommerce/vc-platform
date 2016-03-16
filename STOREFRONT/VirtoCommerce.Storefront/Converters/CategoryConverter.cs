using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
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

            if (category.SeoInfos != null)
            {
                //Select best matched SEO by StoreId and Language
                retVal.SeoInfo = category.SeoInfos.Where(x => store.Id.Equals(x.StoreId, StringComparison.OrdinalIgnoreCase) || x.StoreId == null)
                                                  .OrderByDescending(x => x.StoreId)
                                                  .Select(s => s.ToWebModel())
                                                  .FirstOrDefault(x => x.Language == currentLanguage);
            }

            if (category.Images != null)
            {
                retVal.Images = category.Images.Select(i => i.ToWebModel()).ToArray();
                retVal.PrimaryImage = retVal.Images.FirstOrDefault();
            }

            if (category.Properties != null)
            {
                retVal.Properties = category.Properties.Where(x => string.Equals(x.Type, "Category", StringComparison.InvariantCultureIgnoreCase))
                                                      .Select(p => p.ToWebModel(currentLanguage))
                                                      .ToList();
            }
            return retVal;
        }
    }
}
