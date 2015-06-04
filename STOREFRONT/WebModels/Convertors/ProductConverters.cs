using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Omu.ValueInjecter;
using VirtoCommerce.Web.Extensions;
using VirtoCommerce.Web.Models;
using Data = VirtoCommerce.ApiClient.DataContracts;

namespace VirtoCommerce.Web.Convertors
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
                               Price = variant.NumericPrice,
                               RequiresShipping = true,
                               Quantity = 1,
                               Url = product.Url,
                               Title = product.Title,
                               Image = product.FeaturedImage.Src
                           };

            return lineItem;
        }

        public static Product AsWebModel(
            this Data.Product product, IEnumerable<Data.Price> prices,
            IEnumerable<Data.Marketing.PromotionReward> rewards, IEnumerable<Data.InventoryInfo> inventories, Collection collection = null)
        {
            var productModel = new Product();

            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");
            var description = product.EditorialReviews != null ?
                product.EditorialReviews.FirstOrDefault(er => er.ReviewType.Equals("quickreview", StringComparison.OrdinalIgnoreCase)) : null;

            var fieldsCollection = new MetafieldsCollection("global", product.Properties);
            var options = GetOptions(product.Properties).Select(o => o.Key).ToArray();

            var keywords = product.Seo != null ? product.Seo.Select(k => k.AsWebModel()) : null;

            productModel.Description = description != null ? description.Content : null;
            productModel.Handle = product.Code;
            productModel.Id = product.Id;
            productModel.Images = new ItemCollection<Image>(product.Images.Select(i => i.AsWebModel(product.Name, product.Id)));
            productModel.Keywords = keywords;
            productModel.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            productModel.Options = options;
            productModel.Tags = null; // TODO
            productModel.TemplateSuffix = null; // TODO
            productModel.Title = product.Name;
            productModel.Type = null; // TODO
            productModel.Url = string.Format(pathTemplate, product.Code);
            productModel.Vendor = fieldsCollection.ContainsKey("brand") ? fieldsCollection["brand"] as string : null;

            // form url
            // "/products/code" or "/en-us/store/collection/outline" 

            // specify SEO based url
            var urlHelper = GetUrlHelper();
            var url = String.Empty;
            if (urlHelper != null && productModel.Keywords != null && productModel.Keywords.Any())
            {
                var keyword = productModel.Keywords.SeoKeyword(Thread.CurrentThread.CurrentUICulture.Name);
                if (keyword != null)
                {
                    url = urlHelper.ItemUrl(keyword.Keyword, collection == null ? "" : collection.Outline);
                    if (!String.IsNullOrEmpty(url))
                        productModel.Url = url;
                }
            }

            if (String.IsNullOrEmpty(url) && urlHelper != null)
            {
                url = urlHelper.ItemUrl(productModel.Handle, collection == null ? "" : collection.Outline);
                if (!String.IsNullOrEmpty(url))
                    productModel.Url = url;
            }

            var productRewards = rewards.Where(r => r.RewardType == "CatalogItemAmountReward" && r.ProductId == product.Id);

            if (product.Variations != null)
            {
                foreach (var variation in product.Variations)
                {
                    var price = prices.FirstOrDefault(p => p.ProductId == variation.Id);

                    var variantInventory = inventories != null ?
                        inventories.FirstOrDefault(i => i.ProductId == variation.Id) : null;

                    productModel.Variants.Add(variation.AsWebModel(price, options, productRewards, variantInventory));
                }
            }

            var productPrice = prices.FirstOrDefault(p => p.ProductId == product.Id);

            var productInventory = inventories != null ?
                inventories.FirstOrDefault(i => i.ProductId == product.Id) : null;

            var variant = product.AsVariantWebModel(productPrice, options, productRewards, productInventory);

            productModel.Variants.Add(variant);

            return productModel;
        }

        public static Variant AsWebModel(this Data.CatalogItem variation, Data.Price price, string[] options,
            IEnumerable<Data.Marketing.PromotionReward> rewards, Data.InventoryInfo inventory)
        {
            var variantModel = new Variant();

            var variationImage =
                variation.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)) ??
                variation.Images.FirstOrDefault();
            var variationOptions = variation.Properties.Skip(0).Take(3).ToArray();
            string variantlUrlParameter = null;// HttpContext.Current.Request.QueryString["variant"];
            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}?variant={1}");

            var reward = rewards.FirstOrDefault();

            variantModel.Barcode = null; // TODO
            variantModel.CompareAtPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            //variantModel.Id = variation.Id;
            variantModel.Id = variation.Code;
            variantModel.Image = variationImage != null ? variationImage.AsWebModel(variation.Name, variation.MainProductId) : null;

            PopulateInventory(ref variantModel, variation, inventory);
            variantModel.Option1 = options.Length >= 1 ? variation.Properties[options[0]] as string : null;
            variantModel.Option2 = options.Length >= 2 ? variation.Properties[options[1]] as string : null;
            variantModel.Option3 = options.Length >= 3 ? variation.Properties[options[2]] as string : null;

            variantModel.NumericPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            if (reward != null)
            {
                variantModel.NumericPrice -= reward.Amount;
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

        private static void PopulateInventory(ref Variant variant, Data.CatalogItem item, Data.InventoryInfo inventory)
        {
            if (item.IsBuyable.HasValue && item.IsBuyable.Value &&
                item.StartDate < DateTime.UtcNow &&
                (!item.EndDate.HasValue || item.EndDate.Value > DateTime.UtcNow))
            {
                if (item.TrackInventory.HasValue && item.TrackInventory.Value)
                {
                    if (inventory != null && inventory.Status == Data.InventoryStatus.Enabled)
                    {
                        variant.InventoryManagement = inventory.FulfillmentCenterId;
                        variant.InventoryPolicy = "deny";
                        variant.InventoryQuantity = inventory.InStockQuantity - inventory.ReservedQuantity;

                        if (inventory.AllowBackorder && inventory.BackorderAvailabilityDate.HasValue &&
                            inventory.BackorderAvailabilityDate.Value > DateTime.UtcNow)
                        {
                            variant.InventoryPolicy = "continue";
                        }

                        if (inventory.AllowPreorder && inventory.PreorderAvailabilityDate.HasValue &&
                            inventory.PreorderAvailabilityDate.Value > DateTime.UtcNow)
                        {
                            variant.InventoryPolicy = "continue";
                        }
                    }
                }
            }
        }

        public static Variant AsVariantWebModel(this Data.Product product, Data.Price price, string[] options, IEnumerable<Data.Marketing.PromotionReward> rewards,
            Data.InventoryInfo inventory)
        {
            var variantModel = new Variant();

            var variationImage =
                product.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)) ??
                product.Images.FirstOrDefault();

            string variantlUrlParameter = null;// HttpContext.Current.Request.QueryString["variant"];
            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");

            var reward = rewards.FirstOrDefault();

            variantModel.Barcode = null; // TODO
            variantModel.CompareAtPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            //variantModel.Id = product.Id;
            variantModel.Id = product.Code;
            variantModel.Image = variationImage != null ? variationImage.AsWebModel(product.Name, product.Id) : null;
            
            PopulateInventory(ref variantModel, product, inventory);

            variantModel.Option1 = options.Length >= 1 ? product.Properties[options[0]] as string : null;
            variantModel.Option2 = options.Length >= 2 ? product.Properties[options[1]] as string : null;
            variantModel.Option3 = options.Length >= 3 ? product.Properties[options[2]] as string : null;

            variantModel.NumericPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            if (reward != null)
            {
                variantModel.NumericPrice -= reward.Amount;
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

        private static UrlHelper GetUrlHelper()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            var urlHelper = new UrlHelper(requestContext);
            return urlHelper;

        }
    }
}