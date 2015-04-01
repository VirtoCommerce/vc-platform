#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Omu.ValueInjecter;
using Data = VirtoCommerce.ApiClient.DataContracts;

#endregion

namespace VirtoCommerce.Web.Models.Convertors
{
    public static class ProductConverters
    {
        #region Public Methods and Operators
        public static LineItem AsLineItem(this Product product)
        {
            var variant = product.Variants[0];
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

        //public static Product AsWebModel(this Data.Product product, IEnumerable<Price> prices)
        //{
        //    if (product == null) return null;

        //    var productModel = product.AsWebModel();

        //    foreach (var variation in productModel.Variants)
        //    {
        //        var variationPrices =
        //            prices.Where(p => p.ProductId.Equals(variation.Id, StringComparison.OrdinalIgnoreCase));
        //    }

        //    return productModel;
        //}

        public static Product AsWebModel(this Data.Product product, IEnumerable<Data.Price> prices/*, IEnumerable<Data.ItemInventory> inventories*/)
        {
            var productModel = new Product();

            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}");

            productModel.Available = true; // TODO
            productModel.Collections = null; // TODO
            productModel.Description = null; // TODO
            productModel.FeaturedImage = product.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)).AsWebModel();
            productModel.Handle = product.Code;
            productModel.Id = product.Id;
            productModel.Price = prices.FirstOrDefault(p => p.ProductId == product.Id).List;
            productModel.SelectedVariant = null; // TODO
            productModel.Tags = null; // TODO
            productModel.TemplateSuffix = null; // TODO
            productModel.Title = product.Name;
            productModel.Type = null; // TODO
            productModel.Url = string.Format(pathTemplate, product.Code);
            productModel.Vendor = null; // TODO

            if (product.Variations != null)
            {
                foreach (var variation in product.Variations)
                {
                    var price = prices.FirstOrDefault(p => p.ProductId == variation.Id);
                    //var inventory = inventories.FirstOrDefault(i => i.ProductId == variation.Id);

                    if (price != null/* && inventory != null*/)
                    {
                        productModel.Variants.Add(variation.AsWebModel(price/*, inventory*/));
                    }
                }
            }


            //var productModel = new Product
            //                   {
            //                       Id = product.Id,
            //                       Handle = product.Code,
            //                       Title = product.Name,
            //                       Available = true,
            //                       Options = new[] { "Size" },
            //                       Url = String.Format(path, product.Id)
            //                   };

            //// TODO: populate collections product belongs to

            //if (product.Variations != null && product.Variations.Any())
            //{
            //    var variants = product.Variations.Select(variant => variant.AsVariantWebModel());
            //    productModel.Variants = new List<Variant>(variants);
            //}
            //else // main product is a variation itself
            //{
            //    var productVariant = product.AsVariantWebModel();
            //    productVariant.Title += " Default";
            //    productModel.Variants =
            //        new List<Variant>(new[] { productVariant });
            //}

            //if (product.Images != null && product.Images.Any())
            //{
            //    productModel.Images = product.Images.Select(i => i.AsWebModel());
            //}

            //if (product.EditorialReviews != null && product.EditorialReviews.Any())
            //{
            //    var description =
            //        product.EditorialReviews.SingleOrDefault(
            //            e => e.ReviewType.Equals("quickreview", StringComparison.OrdinalIgnoreCase));
            //    if (description != null)
            //    {
            //        productModel.Description = description.Content;
            //    }

            //    var content =
            //        product.EditorialReviews.SingleOrDefault(
            //            e => e.ReviewType.Equals("fullreview", StringComparison.OrdinalIgnoreCase));
            //    if (content != null)
            //    {
            //        productModel.Content = content.Content;
            //    }
            //}

            //// add meta fields
            //if (product.Properties != null && product.Properties.Any())
            //{
            //    var fieldsCollection = new MetafieldsCollection("global", product.Properties);
            //    productModel.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            //}

            //if (product.Seo != null)
            //{
            //    productModel.Keywords = product.Seo.Select(k => k.AsWebModel());
            //}

            //productModel.Url = GetUrl(productModel);

            return productModel;
        }

        public static Variant AsWebModel(this Data.CatalogItem variation, Data.Price price/*, Data.ItemInventory inventory*/)
        {
            var variantModel = new Variant();

            var variationImage =
                variation.Images.FirstOrDefault(i => i.Name.Equals("primaryimage", StringComparison.OrdinalIgnoreCase)) ??
                variation.Images[0];
            var variationOptions = variation.Properties.Skip(0).Take(3).ToArray();
            var variantlUrlParameter = HttpContext.Current.Request.QueryString["variant"];
            var pathTemplate = VirtualPathUtility.ToAbsolute("~/products/{0}?variant={1}");

            variantModel.Barcode = ""; // TODO
            variantModel.CompareAtPrice = price.List;
            variantModel.Id = variation.Id;
            variantModel.Image = variationImage == null ? null : new Image
            {
                Alt = variation.Name,
                AttachedToVariant = true,
                Id = variationImage.Id,
                Position = 0, // TODO
                ProductId = variation.MainProductId,
                Src = variationImage.Src
            };
            variantModel.InventoryManagement = "vc-inventory-service";
            //variantModel.InventoryPolicy = inventory.AllowPreorder ? "continue" : "deny";
            //variantModel.InventoryQuantity = inventory.InStockQuantity;
            variantModel.Option1 = variationOptions.Length >= 1 ? variationOptions[0].Value as string : null;
            variantModel.Option2 = variationOptions.Length >= 2 ? variationOptions[1].Value as string : null;
            variantModel.Option3 = variationOptions.Length == 3 ? variationOptions[2].Value as string : null;
            variantModel.Price = price.Sale.HasValue ? price.Sale.Value : price.List;
            variantModel.Selected = variantlUrlParameter != null;
            variantModel.Sku = variation.Code;
            variantModel.Title = variation.Name;
            variantModel.Url = string.Format(pathTemplate, variation.MainProductId, variation.Id);
            variantModel.Weight = 0; // TODO
            variantModel.WeightInUnit = null; // TODO
            variantModel.WeightUnit = "g"; // TODO

            return variantModel;
        }

        public static Price AsWebModel(this Data.Price price)
        {
            var priceModel = new Price();
            priceModel.InjectFrom(price);
            return priceModel;
        }

        public static Image AsWebModel(this Data.ItemImage image)
        {
            var imageModel = new Image();
            imageModel.InjectFrom(image);
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
        #endregion

        #region Methods

        private static string GetUrl(Product product)
        {
            /*
            var keyword = product.Keywords.SeoKeyword();

            if (keyword != null)
            {
                //return String.Format("/{0}", keyword.Keyword);
                var url = GetUrlHelper();
                return url.ItemUrl(keyword.Keyword, product.CategoryOutline);
            }
             * */

            var path = VirtualPathUtility.ToAbsolute("~/products/{0}");

            return String.Format(path, product.Handle);
        }

        private static UrlHelper GetUrlHelper()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                throw new InvalidOperationException("Invalid HttpContext");
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = RouteTable.Routes.GetRouteData(httpContextBase);
            var requestContext = new RequestContext(httpContextBase, routeData);
            var urlHelper = new UrlHelper(requestContext);
            return urlHelper;
        }
        #endregion
    }
}