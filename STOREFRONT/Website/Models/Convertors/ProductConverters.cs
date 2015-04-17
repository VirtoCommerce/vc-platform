using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Omu.ValueInjecter;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class ProductConverters
    {
        public static LineItem AsLineItem(this Product product)
        {
            var variant = product.Variants.First();
            var lineItem = new LineItem
                           {
                               Product = product,
                               Variant = variant,
                               ProductId = product.Id,
                               VariantId = variant.Id,
                               Handle = product.Handle,
                               Price = variant.Price,
                               Quantity = 1,
                               Url = product.Url,
                               Title = product.Title,
                               Image = product.FeaturedImage.Src
                           };

            return lineItem;
        }

        public static Product AsWebModel(
            this Data.Product product, IEnumerable<Data.Price> prices,
            IEnumerable<Data.Marketing.PromotionReward> rewards)
        {
            var productModel = new Product();

            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");
            var description = product.EditorialReviews != null ?
                product.EditorialReviews.FirstOrDefault(er => er.ReviewType.Equals("quickreview", StringComparison.OrdinalIgnoreCase)) : null;
            var fieldsCollection = new MetafieldsCollection("global", product.Properties);
            var options = GetOptions(product.Properties).Select(o => o.Key).ToArray();

            productModel.Description = description != null ? description.Content : null;
            productModel.Handle = product.Code;
            productModel.Id = product.Id;
            productModel.Images = new ItemCollection<Image>(product.Images.Select(i => i.AsWebModel(product.Name, product.Id)));
            productModel.Keywords = product.Seo != null ? product.Seo.Select(k => k.AsWebModel()) : null;
            productModel.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            productModel.Options = options;
            productModel.Tags = null; // TODO
            productModel.TemplateSuffix = null; // TODO
            productModel.Title = product.Name;
            productModel.Type = null; // TODO
            productModel.Url = string.Format(pathTemplate, product.Code);
            productModel.Vendor = fieldsCollection.ContainsKey("brand") ? fieldsCollection["brand"] as string : null;

            var productRewards = rewards.Where(r => r.RewardType == "CatalogItemAmountReward" && r.ProductId == product.Id);

            if (product.Variations == null)
            {
                var price = prices.FirstOrDefault(p => p.ProductId == product.Id);

                if (price != null)
                {
                    var variant = product.AsVariantWebModel(price, options, productRewards);

                    productModel.Variants.Add(variant);
                }
            }
            else
            {
                foreach (var variation in product.Variations)
                {
                    var price = prices.FirstOrDefault(p => p.ProductId == variation.Id);

                    if (price != null)
                    {
                        productModel.Variants.Add(variation.AsWebModel(price, options, productRewards));
                    }
                }
            }

            return productModel;
        }

        public static Variant AsWebModel(this Data.CatalogItem variation, Data.Price price, string[] options, IEnumerable<Data.Marketing.PromotionReward> rewards)
        {
            var variantModel = new Variant();

            var variationImage =
                variation.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)) ??
                variation.Images.FirstOrDefault();
            var variationOptions = variation.Properties.Skip(0).Take(3).ToArray();
            var variantlUrlParameter = HttpContext.Current.Request.QueryString["variant"];
            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}?variant={1}");

            var reward = rewards.FirstOrDefault();

            variantModel.Barcode = null; // TODO
            variantModel.CompareAtPrice = price.Sale.HasValue ? price.Sale.Value : price.List;
            variantModel.Id = variation.Id;
            variantModel.Image = variationImage != null ? variationImage.AsWebModel(variation.Name, variation.MainProductId) : null;
            variantModel.InventoryManagement = null; // TODO
            variantModel.InventoryPolicy = null; // TODO
            variantModel.InventoryQuantity = 0; // TODO
            variantModel.Option1 = options.Length >= 1 ? variation.Properties[options[0]] as string : null;
            variantModel.Option2 = options.Length >= 2 ? variation.Properties[options[1]] as string : null;
            variantModel.Option3 = options.Length >= 3 ? variation.Properties[options[2]] as string : null;

            variantModel.Price = price.Sale.HasValue ? price.Sale.Value : price.List;
            if (reward != null)
            {
                variantModel.Price -= reward.Amount;
            }

            variantModel.Selected = variantlUrlParameter != null;
            variantModel.Sku = variation.Properties.ContainsKey("sku") ? variation.Properties["sku"] as string : variation.Code;
            variantModel.Title = variation.Name;
            variantModel.Url = string.Format(pathTemplate, variation.MainProductId, variation.Id);
            variantModel.Weight = variation.Properties.ContainsKey("weight") ? (int)variation.Properties["weight"] : 0;
            variantModel.WeightInUnit = null; // TODO
            variantModel.WeightUnit = null; // TODO

            return variantModel;
        }

        public static Variant AsVariantWebModel(this Data.Product product, Data.Price price, string[] options, IEnumerable<Data.Marketing.PromotionReward> rewards)
        {
            var variantModel = new Variant();

            var variationImage =
                product.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)) ??
                product.Images.FirstOrDefault();
            var variantlUrlParameter = HttpContext.Current.Request.QueryString["variant"];
            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");

            var reward = rewards.FirstOrDefault();

            variantModel.Barcode = null; // TODO
            variantModel.CompareAtPrice = price.Sale.HasValue ? price.Sale.Value : price.List;
            variantModel.Id = product.Id;
            variantModel.Image = variationImage != null ? variationImage.AsWebModel(product.Name, product.Id) : null;
            variantModel.InventoryManagement = null; // TODO
            variantModel.InventoryPolicy = null; // TODO
            variantModel.InventoryQuantity = 0; // TODO
            variantModel.Option1 = options.Length >= 1 ? product.Properties[options[0]] as string : null;
            variantModel.Option2 = options.Length >= 2 ? product.Properties[options[1]] as string : null;
            variantModel.Option3 = options.Length >= 3 ? product.Properties[options[2]] as string : null;

            variantModel.Price = price.Sale.HasValue ? price.Sale.Value : price.List;
            if (reward != null)
            {
                variantModel.Price -= reward.Amount;
            }

            variantModel.Selected = variantlUrlParameter != null;
            variantModel.Sku = product.Properties.ContainsKey("sku") ? product.Properties["sku"] as string : product.Code;
            variantModel.Title = product.Name;
            variantModel.Url = string.Format(pathTemplate, product.Id);
            variantModel.Weight = product.Properties.ContainsKey("weight") ? (int)product.Properties["weight"] : 0;
            variantModel.WeightInUnit = null; // TODO
            variantModel.WeightUnit = null; // TODO

            return variantModel;
        }

        public static Price AsWebModel(this Data.Price price)
        {
            var priceModel = new Price();
            priceModel.InjectFrom(price);
            return priceModel;
        }

        public static Image AsWebModel(this Data.ItemImage image, string alt, string productId, int position = 0, ICollection<Variant> variants = null)
        {
            var imageModel = new Image();

            imageModel.Alt = alt;
            imageModel.AttachedToVariant = true;
            imageModel.Id = image.Id;
            imageModel.Name = image.Name;
            imageModel.Position = position;
            imageModel.ProductId = productId;
            imageModel.Src = image.Src;
            imageModel.Variants = variants;

            return imageModel;
        }

        public static Review AsWebModel(this Data.Review review)
        {
            var webReview = new Review();

            webReview.Author = review.AuthorName;
            webReview.Created = review.Created;
            webReview.Id = review.Id;
            webReview.ProductRating = review.Rating;
            webReview.Text = review.ReviewText;
            webReview.Title = null;

            return webReview;
        }

        private static IDictionary<string, object> GetOptions(IDictionary<string, object> itemProperties)
        {
            var options = new Dictionary<string, object>();

            if (itemProperties.ContainsKey("size"))
            {
                options.Add("size", itemProperties["size"]);
            }
            if (itemProperties.ContainsKey("color"))
            {
                options.Add("color", itemProperties["color"]);
            }
            if (itemProperties.ContainsKey("material"))
            {
                options.Add("material", itemProperties["material"]);
            }

            return options;
        }
    }
}