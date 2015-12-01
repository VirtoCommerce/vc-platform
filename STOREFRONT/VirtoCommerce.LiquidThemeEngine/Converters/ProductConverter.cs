using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ProductConverter
    {
        public static Product ToShopifyModel(this storefrontModel.Catalog.Product product, storefrontModel.WorkContext workContext)
        {
            var result = new Product();
            result.InjectFrom<NullableAndEnumValueInjection>(product);
            result.Variants.Add(product.ToVariant(workContext));

            if (product.Variations != null)
            {
                result.Variants.AddRange(product.Variations.Select(x => x.ToVariant(workContext)));
                
            }

            result.Available = true;// product.IsActive && product.IsBuyable;
         
            result.CompareAtPriceMax = result.Variants.Select(x=>x.CompareAtPrice).Max();
            result.CompareAtPriceMin = result.Variants.Select(x => x.CompareAtPrice).Min();
            result.CompareAtPriceVaries = result.CompareAtPriceMax != result.CompareAtPriceMin;

            result.CompareAtPrice = product.Price.ListPrice.Amount;
            result.Price = product.Price.SalePrice.Amount;
            result.PriceMax = result.Variants.Select(x => x.Price).Max();
            result.PriceMin = result.Variants.Select(x => x.Price).Min();
            result.PriceVaries = result.PriceMax != result.PriceMin;

            result.Content = product.EditorialReviews.Where(x => x.Language.Equals(workContext.CurrentLanguage)).Select(x => x.Content).FirstOrDefault();
            result.Description = result.Content;
            result.FeaturedImage = product.PrimaryImage != null ? product.PrimaryImage.ToShopifyModel() : null;
            if(result.FeaturedImage != null)
            {
                result.FeaturedImage.ProductId = product.Id;
                result.FeaturedImage.AttachedToVariant = false;
            }
            result.FirstAvailableVariant = result.Variants.FirstOrDefault(x => x.Available);
            result.Handle = product.SeoInfo != null ? product.SeoInfo.Slug : product.Id;
            result.Images = product.Images.Select(x => x.ToShopifyModel()).ToArray();
            foreach (var image in result.Images)
            {
                image.ProductId = product.Id;
                image.AttachedToVariant = false;
            }
            if(product.Properties != null)
            {
                result.Options = product.Properties.Where(x => string.Equals(x.Type, "variation", System.StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Name).ToArray();
            }
            result.SelectedVariant = result.Variants.First();
            result.Title = product.Name;
            result.Type = product.ProductType;
            result.Url = "~/product/" + product.Id;
            if (product.SeoInfo != null)
            {
                result.Url = "~/" + product.SeoInfo.Slug;
            }
           
            return result;
        }

        public static Variant ToVariant(this storefrontModel.Catalog.Product product, storefrontModel.WorkContext workContext)
        {
            var result = new Variant();
            result.Available = true; //product.IsActive && product.IsBuyable;
            result.Barcode = product.Gtin;
            result.CompareAtPrice = product.Price.ListPrice.Amount;
            result.FeaturedImage = product.PrimaryImage != null ? product.PrimaryImage.ToShopifyModel() : null;
            if (result.FeaturedImage != null)
            {
                result.FeaturedImage.ProductId = product.Id;
                result.FeaturedImage.AttachedToVariant = true;
                result.FeaturedImage.Variants = new[] { result };
            }
            result.InventoryPolicy = "continue";
            result.InventoryQuantity = product.Inventory != null ? product.Inventory.InStockQuantity ?? 0 : 0;
            result.Options = product.Properties.Where(x => string.Equals(x.Type, "variation", System.StringComparison.InvariantCultureIgnoreCase) && x.Value != null)
                                               .Select(x => string.Join(";", x.Value)).ToArray();

            result.Price = product.Price.SalePrice.Amount;
            result.Selected = false;
            result.Sku = product.Sku;
            result.Title = product.Name;

            result.Url = "~/product/" + product.Id;
            if (product.SeoInfo != null)
            {
                result.Url = "~/" + product.SeoInfo.Slug;
            }
            result.Weight = product.Weight;
            result.WeightUnit = product.WeightUnit;
            return result;
        }

      
    }
}

