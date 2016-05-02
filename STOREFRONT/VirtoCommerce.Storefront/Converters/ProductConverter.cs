using System;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;
using VirtoCommerce.Storefront.Model.Quote;

namespace VirtoCommerce.Storefront.Converters
{
    public static class ProductConverter
    {
        public static Product ToWebModel(this VirtoCommerceCatalogModuleWebModelProduct product, Language currentLanguage, Currency currentCurrency, Store store)
        {
            var retVal = new Product();

            retVal.Currency = currentCurrency;
            retVal.Price = new ProductPrice(currentCurrency);

            retVal.InjectFrom<NullableAndEnumValueInjecter>(product);

            retVal.Sku = product.Code;


            if (product.Properties != null)
            {
                retVal.Properties = product.Properties
                    .Where(x => string.Equals(x.Type, "Product", StringComparison.InvariantCultureIgnoreCase))
                    .Select(p => p.ToWebModel(currentLanguage))
                    .ToList();

                retVal.VariationProperties = product.Properties
                    .Where(x => string.Equals(x.Type, "Variation", StringComparison.InvariantCultureIgnoreCase))
                    .Select(p => p.ToWebModel(currentLanguage))
                    .ToList();
            }

            if (product.Images != null)
            {
                retVal.Images = product.Images.Select(i => i.ToWebModel()).ToArray();
                retVal.PrimaryImage = retVal.Images.FirstOrDefault(x => string.Equals(x.Url, product.ImgSrc, StringComparison.InvariantCultureIgnoreCase));
            }

            if (product.Assets != null)
            {
                retVal.Assets = product.Assets.Select(x => x.ToWebModel()).ToList();
            }

            if (product.Variations != null)
            {
                retVal.Variations = product.Variations.Select(v => v.ToWebModel(currentLanguage, currentCurrency, store)).ToList();
            }

            if(!product.Associations.IsNullOrEmpty())
            {
                retVal.Associations.AddRange(product.Associations.Select(x => x.ToWebModel()));
            }

            retVal.SeoInfo = product.SeoInfos.GetBestMatchedSeoInfo(store, currentLanguage).ToWebModel();
            retVal.Url = "~/" + product.Outlines.GetSeoPath(store, currentLanguage, "product/" + product.Id);

            if (product.Reviews != null)
            {
                retVal.Descriptions = product.Reviews.Select(r => new EditorialReview
                {
                    Language = new Language(r.LanguageCode),
                    ReviewType = r.ReviewType,
                    Value = r.Content
                }).Where(x=>x.Language.Equals(currentLanguage)).ToList();
                retVal.Description = retVal.Descriptions.FindWithLanguage(currentLanguage, x => x.Value, null);
            }

            return retVal;
        }
        
        public static QuoteItem ToQuoteItem(this Product product, long quantity)
        {
            var quoteItem = new QuoteItem();

            quoteItem.InjectFrom<NullableAndEnumValueInjecter>(product);

            quoteItem.Id = null;
            quoteItem.ImageUrl = product.PrimaryImage != null ? product.PrimaryImage.Url : null;
            quoteItem.ListPrice = product.Price.ListPrice;
            quoteItem.ProductId = product.Id;
            quoteItem.SalePrice = product.Price.SalePrice;
            quoteItem.ProposalPrices.Add(new TierPrice(product.Price.SalePrice, quantity));          
            quoteItem.SelectedTierPrice = quoteItem.ProposalPrices.First();

            return quoteItem;
        }

        public static PromotionProductEntry ToPromotionItem(this Product product)
        {
            var promoItem = new PromotionProductEntry();

            promoItem.InjectFrom(product);

            if (product.Price != null)
            {
                promoItem.Discount = new Money(product.Price.ActiveDiscount != null ? product.Price.ActiveDiscount.Amount.Amount : 0m, product.Price.Currency);
                promoItem.Price = product.Price.SalePrice;
            }

            promoItem.ProductId = product.Id;
            promoItem.Quantity = 1;
            promoItem.Variations = product.Variations.Select(v => v.ToPromotionItem()).ToList();

            return promoItem;
        }
    }
}
