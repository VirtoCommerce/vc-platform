using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Client.Model;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductConverter
    {
        public static Product ToWebModel(this VirtoCommerceCatalogModuleWebModelProduct product, Language currentLanguage, Currency currentCurrency)
        {
            var retVal = new Product();
            retVal.Price = new ProductPrice(currentCurrency);

            retVal.InjectFrom(product);

            retVal.Sku = product.Code;

            if(product.Category != null)
            {
                retVal.Category = product.Category.ToWebModel();
            }

            if (product.Properties != null)
            {
                retVal.Properties = product.Properties.Where(x => !String.Equals(x.Type, "Variation", StringComparison.InvariantCultureIgnoreCase))
                                                      .Select(p => p.ToWebModel(currentLanguage))
                                                      .ToList();
                retVal.VariationProperties = product.Properties.Where(x => String.Equals(x.Type, "Variation", StringComparison.InvariantCultureIgnoreCase))
                                                      .Select(p => p.ToWebModel(currentLanguage))
                                                      .ToList();
            }
            if (product.Images != null)
            {
                retVal.Images = product.Images.Select(i => i.ToWebModel()).ToArray();
                retVal.PrimaryImage = retVal.Images.FirstOrDefault(x => String.Equals(x.Url, product.ImgSrc, StringComparison.InvariantCultureIgnoreCase));
            }

            if (product.Assets != null)
            {
                retVal.Assets = product.Assets.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(v => v.ToWebModel(currentLanguage, currentCurrency)).ToList();
            }

            if (product.SeoInfos != null)
                retVal.SeoInfo = product.SeoInfos.Select(s => s.ToWebModel()).FirstOrDefault();

            if (product.Reviews != null)
            {
                retVal.Descriptions = product.Reviews.Select(r => new LocalizedString(new Language(r.LanguageCode), r.Content)).ToList();
                retVal.Description = retVal.Descriptions.Where(x => x.Language.Equals(currentLanguage))
                                                        .Select(x => x.Value)
                                                        .FirstOrDefault();
            }

            return retVal;
        }

        public static VirtoCommerceDomainMarketingModelProductPromoEntry ToPromotionItem(this Product product)
        {
            var promoItem = new VirtoCommerceDomainMarketingModelProductPromoEntry();

            promoItem.InjectFrom(product);

            if (product.Price != null)
            {
                promoItem.Discount = product.Price.ActiveDiscount != null ? (double)product.Price.ActiveDiscount.Amount.Amount : 0;
                promoItem.Price = (double)product.Price.SalePrice.Amount;
            }

            promoItem.ProductId = product.Id;
            promoItem.Quantity = 1;
            promoItem.Variations = product.Variations.Select(v => v.ToPromotionItem()).ToList();

            return promoItem;
        }
    }
}