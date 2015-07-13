using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Omu.ValueInjecter;
using VirtoCommerce.ApiClient.Extensions;
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
                               RequiresShipping = String.IsNullOrEmpty(product.Type) ||
                                    !String.IsNullOrEmpty(product.Type) && product.Type.Equals("Physical", StringComparison.OrdinalIgnoreCase),
                               Quantity = 1,
                               Sku = variant.Sku,
                               Url = product.Url,
                               Title = product.Title,
                               Image = product.FeaturedImage.Src,
                               TaxType = product.TaxType
                           };

            return lineItem;
        }

        public static Product AsWebModel(
            this Data.Product product, IEnumerable<Data.Price> prices,
            IEnumerable<Data.Marketing.PromotionReward> rewards, Collection collection = null)
        {
            var productModel = new Product();

            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");
            var description = product.EditorialReviews != null ?
                product.EditorialReviews.FirstOrDefault(er => er.ReviewType != null && er.ReviewType.Equals("quickreview", StringComparison.OrdinalIgnoreCase)) : null;

            var fieldsCollection = new MetafieldsCollection("global", product.Properties);
            var options = GetOptions(product.VariationProperties);

            var keywords = product.Seo != null ? product.Seo.Select(k => k.AsWebModel()) : null;

            var primaryImage = product.PrimaryImage ?? (product.Images != null ? product.Images.FirstOrDefault() : null);

            productModel.Description = description != null ? description.Content : null;
            productModel.Handle = product.Code;
            productModel.Id = product.Id;
            productModel.Images = product.Images != null ?
                new ItemCollection<Image>(product.Images.Select(i => i.AsWebModel(product.Name, product.Id))) : null;
            productModel.FeaturedImage = primaryImage != null ?
                primaryImage.AsWebModel(primaryImage.Name, product.Id) : null;
            productModel.Keywords = keywords;
            productModel.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            productModel.Options = options;
            productModel.Tags = null; // TODO
            productModel.TemplateSuffix = null; // TODO
            productModel.Title = product.Name;
            productModel.Type = product.ProductType;
            productModel.Url = string.Format(pathTemplate, product.Code);
            productModel.Vendor = fieldsCollection.ContainsKey("brand") ? fieldsCollection["brand"] as string : null;
            productModel.TaxType = product.TaxType;
            // form url
            // "/products/code" or "/en-us/store/collection/outline" 

            // specify SEO based url
            var urlHelper = GetUrlHelper();
            var url = String.Empty;
            if (urlHelper != null && collection != null && productModel.Keywords != null && productModel.Keywords.Any())
            {
                var keyword = productModel.Keywords.SeoKeyword(Thread.CurrentThread.CurrentUICulture.Name);
                if (keyword != null)
                {
                    url = urlHelper.ItemUrl(keyword.Keyword, collection == null ? "" : collection.Outline);
                    if (!String.IsNullOrEmpty(url))
                        productModel.Url = url;
                }
            }

            if (String.IsNullOrEmpty(url) && urlHelper != null && collection != null)
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

                    productModel.Variants.Add(variation.AsVariantWebModel(price, options, productRewards));
                }
            }

            var productPrice = prices.FirstOrDefault(p => p.ProductId == product.Id);

            var variant = product.AsVariantWebModel(productPrice, options, productRewards);

            variant.Title = "Default Title";

            productModel.Variants.Add(variant);

            return productModel;
        }

        public static Variant AsVariantWebModel(this Data.CatalogItem variation, Data.Price price, string[] options,
            IEnumerable<Data.Marketing.PromotionReward> rewards)
        {
            var variantModel = new Variant();

            var variationImage = variation.PrimaryImage ?? (variation.Images != null ? variation.Images.FirstOrDefault() : null);

            string variantlUrlParameter = null;// HttpContext.Current.Request.QueryString["variant"];
            string pathTemplate;

            if (variation is Data.Product)
            {
                pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");
            }
            else
            {
                pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}?variant={1}");
            }

            var reward = rewards.FirstOrDefault();

            variantModel.Barcode = null; // TODO
            variantModel.CompareAtPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            //variantModel.Id = variation.Id;
            variantModel.Id = variation.Code;
            variantModel.Image = variationImage != null ? variationImage.AsWebModel(variation.Name, variation.MainProductId) : null;

            PopulateInventory(ref variantModel, variation);
            variantModel.Options = GetOptionValues(options, variation.VariationProperties);

            variantModel.NumericPrice = price != null ? (price.Sale.HasValue ? price.Sale.Value : price.List) : 0M;
            if (reward != null)
            {
                variantModel.NumericPrice -= reward.Amount;
            }

            variantModel.Selected = variantlUrlParameter != null;
            variantModel.Sku = variation.Properties.ContainsKey("sku") ? variation.Properties["sku"] as string : variation.Code;
            variantModel.Title = variation.Name;
            variantModel.Url = string.Format(pathTemplate, variation.MainProductId, variation.Id);
            variantModel.Weight = variation.Weight.HasValue ? variation.Weight.Value : 0;
            variantModel.WeightInUnit = null; // TODO
            variantModel.WeightUnit = variation.WeightUnit;

            return variantModel;
        }

        private static void PopulateInventory(ref Variant variant, Data.CatalogItem item)
        {
            if (item.IsBuyable.HasValue && item.IsBuyable.Value &&
                item.StartDate < DateTime.UtcNow &&
                (!item.EndDate.HasValue || item.EndDate.Value > DateTime.UtcNow))
            {
                if (item.TrackInventory.HasValue && item.TrackInventory.Value)
                {
                    var inventory = item.Inventory;
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

        public static Price AsWebModel(this Data.Price price)
        {
            var priceModel = new Price();
            priceModel.InjectFrom(price);
            return priceModel;
        }

        public static Image AsWebModel(this Data.ItemImage image, string alt, string productId, int position = 0, ICollection<Variant> variants = null)
        {
            var imageModel = new Image { Alt = alt, AttachedToVariant = true, Name = image.Name, Position = position, ProductId = productId, Src = image.Src, Variants = variants };

            return imageModel;
        }

        public static Review AsWebModel(this Data.Review review)
        {
            var webReview = new Review { Author = review.AuthorName, Created = review.Created, Id = review.Id, ProductRating = review.Rating, Text = review.ReviewText, Title = null };

            return webReview;
        }

        #region Option Methods

        private static string DefaultOption = "Default Title";
        private static string[] GetOptions(IDictionary<string, object> itemProperties)
        {
            if (itemProperties == null || !itemProperties.Any())
            {
                return new []{ "Title" };
            }

            var options = itemProperties.Select(o => o.Key).ToArray();
            if (options == null || !options.Any())
            {
                options = new[] { "Title" };
            }

            return options;
        }

        private static string[] GetOptionValues(IEnumerable<string> options, IDictionary<string, object> itemProperties)
        {
            if (options != null && options.Count() == 1 && options.ElementAt(0) == "Title")
            {
                return new[] { DefaultOption };
            }

            if (itemProperties == null || !itemProperties.Any() || options == null)
            {
                return null;
            }

            var variationOptions = options.Select(option => itemProperties.ContainsKey(option) ? itemProperties[option].ToNullOrString() : null).ToList();
            return variationOptions.ToArray();
        }
        #endregion

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