using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;


namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ProductConverter
    {
        public static Product ToShopifyModel(this StorefrontModel.Catalog.Product product)
        {
            var result = new Product();
            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(product);

            if (product.IsBuyable)
            {
                result.Variants.Add(product.ToVariant());
            }

            if (product.Variations != null)
            {
                result.Variants.AddRange(product.Variations.Select(x => x.ToVariant()));
            }

            result.Available = true;// product.IsActive && product.IsBuyable;

            result.CatalogId = product.CatalogId;
            result.CategoryId = product.CategoryId;

            result.CompareAtPriceMax = result.Variants.Select(x => x.CompareAtPrice).Max();
            result.CompareAtPriceMin = result.Variants.Select(x => x.CompareAtPrice).Min();
            result.CompareAtPriceVaries = result.CompareAtPriceMax != result.CompareAtPriceMin;

            result.CompareAtPrice = product.Price.ListPrice.Amount * 100;
            result.Price = product.Price.SalePrice.Amount * 100;
            if (product.Price.ActiveDiscount != null)
            {
                result.Price = result.Price - product.Price.ActiveDiscount.Amount.Amount * 100;
            }
            result.PriceMax = result.Variants.Select(x => x.Price).Max();
            result.PriceMin = result.Variants.Select(x => x.Price).Min();
            result.PriceVaries = result.PriceMax != result.PriceMin;

            result.Content = product.Description;
            result.Description = result.Content;
            result.Descriptions = new Descriptions(product.Descriptions.Select(d => new Description
            {
                Content = d.Value,
                Type = d.ReviewType
            }));
            result.FeaturedImage = product.PrimaryImage != null ? product.PrimaryImage.ToShopifyModel() : null;
            if (result.FeaturedImage != null)
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

            if (product.VariationProperties != null)
            {
                result.Options = product.VariationProperties.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Name).ToArray();
            }
            if (product.Properties != null)
            {
                result.Properties = product.Properties.Select(x => x.ToShopifyModel()).ToList();
                result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("properties", product.Properties) });
            }
            result.SelectedVariant = result.Variants.First();
            result.Title = product.Name;
            result.Type = product.ProductType;
            result.Url = product.Url;

            if (product.Associations.Any())
            {
                result.RelatedProducts = product.Associations.Where(a => a.Product != null)
                    .OrderBy(a => a.Priority)
                    .Select(a => a.Product.ToShopifyModel()).ToList();
            }

        
            return result;
        }

        public static Variant ToVariant(this StorefrontModel.Catalog.Product product)
        {
            var result = new Variant();
            result.Available = true; //product.IsActive && product.IsBuyable;
            result.Barcode = product.Gtin;

            result.CatalogId = product.CatalogId;
            result.CategoryId = product.CategoryId;

            result.FeaturedImage = product.PrimaryImage != null ? product.PrimaryImage.ToShopifyModel() : null;
            if (result.FeaturedImage != null)
            {
                result.FeaturedImage.ProductId = product.Id;
                result.FeaturedImage.AttachedToVariant = true;
                result.FeaturedImage.Variants = new[] { result };
            }
            result.Id = product.Id;
            result.InventoryPolicy = "continue";
            result.InventoryQuantity = product.Inventory != null ? product.Inventory.InStockQuantity ?? 0 : 0;
            result.Options = product.VariationProperties.Where(p => !string.IsNullOrEmpty(p.Value)).Select(p => p.Value).ToArray();
            result.CompareAtPrice = product.Price.ListPrice.Amount * 100;
            result.Price = product.Price.SalePrice.Amount * 100;
            if (product.Price.ActiveDiscount != null)
            {
                result.Price = result.Price - product.Price.ActiveDiscount.Amount.Amount * 100;
            }

            result.Selected = false;
            result.Sku = product.Sku;
            result.Title = product.Name;
            result.Url = product.Url;
            result.Weight = product.Weight;
            result.WeightUnit = product.WeightUnit;
            return result;
        }
    }
}