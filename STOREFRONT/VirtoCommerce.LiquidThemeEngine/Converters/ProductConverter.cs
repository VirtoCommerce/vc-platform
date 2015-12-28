﻿using Omu.ValueInjecter;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Platform.Core.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ProductConverter
    {
        public static Product ToShopifyModel(this StorefrontModel.Catalog.Product product, StorefrontModel.WorkContext workContext)
        {
            var result = new Product();
            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(product);
            result.Variants.Add(product.ToVariant(workContext));

            if (product.Variations != null)
            {
                result.Variants.AddRange(product.Variations.Select(x => x.ToVariant(workContext)));
                
            }

            result.Available = true;// product.IsActive && product.IsBuyable;

            result.CatalogId = product.CatalogId;
            result.CategoryId = product.CategoryId;

            result.CompareAtPriceMax = result.Variants.Select(x=>x.CompareAtPrice).Max();
            result.CompareAtPriceMin = result.Variants.Select(x => x.CompareAtPrice).Min();
            result.CompareAtPriceVaries = result.CompareAtPriceMax != result.CompareAtPriceMin;

            result.CompareAtPrice = product.Price.ListPrice.Amount * 100;
            result.Price = product.Price.SalePrice.Amount * 100;
            if(product.Price.ActiveDiscount != null)
            {
                result.Price = result.Price - product.Price.ActiveDiscount.Amount.Amount * 100;
            }
            result.PriceMax = result.Variants.Select(x => x.Price).Max();
            result.PriceMin = result.Variants.Select(x => x.Price).Min();
            result.PriceVaries = result.PriceMax != result.PriceMin;

            result.Content = product.Description;
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
            if(product.VariationProperties != null)
            {
                result.Options = product.VariationProperties.Select(x => x.Name).ToArray();
            }
            if(product.Properties != null)
            {
                result.Properties = product.Properties.Select(x => x.ToShopifyModel()).ToList();
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

        public static Variant ToVariant(this StorefrontModel.Catalog.Product product, StorefrontModel.WorkContext workContext)
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
            result.Options = product.VariationProperties.Select(p => p.Value).ToArray();
            result.CompareAtPrice = product.Price.ListPrice.Amount * 100;
            result.Price = product.Price.SalePrice.Amount * 100;
            if (product.Price.ActiveDiscount != null)
            {
                result.Price = result.Price - product.Price.ActiveDiscount.Amount.Amount * 100;
            }

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