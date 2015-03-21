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

        public static Product AsWebModel(this Data.Product product, IEnumerable<Price> prices)
        {
            if (product == null) return null;

            var productModel = product.AsWebModel();

            foreach (var variation in productModel.Variants)
            {
                var variationPrices =
                    prices.Where(p => p.ProductId.Equals(variation.Id, StringComparison.OrdinalIgnoreCase));

                if (variationPrices.Any())
                    variation.Prices = variationPrices.ToArray();
            }

            return productModel;
        }

        public static Product AsWebModel(this Data.Product product)
        {
            if (product == null) return null;

            var productModel = new Product
                               {
                                   Id = product.Id,
                                   Handle = product.Code,
                                   Title = product.Name,
                                   Available = true,
                                   Options = new[] { "Size" },
                                   Url = String.Format("/products/{0}", product.Id)
                               };

            // TODO: populate collections product belongs to

            if (product.Variations != null && product.Variations.Any())
            {
                var variants = product.Variations.Select(variant => variant.AsVariantWebModel());
                productModel.Variants = new List<Variant>(variants);
            }
            else // main product is a variation itself
            {
                var productVariant = product.AsVariantWebModel();
                productVariant.Title += " Default";
                productModel.Variants =
                    new List<Variant>(new[] { productVariant });
            }

            if (product.Images != null && product.Images.Any())
            {
                productModel.Images = product.Images.Select(i => i.AsWebModel());
            }

            if (product.EditorialReviews != null && product.EditorialReviews.Any())
            {
                var description =
                    product.EditorialReviews.SingleOrDefault(
                        e => e.ReviewType.Equals("quickreview", StringComparison.OrdinalIgnoreCase));
                if (description != null)
                {
                    productModel.Description = description.Content;
                }

                var content =
                    product.EditorialReviews.SingleOrDefault(
                        e => e.ReviewType.Equals("fullreview", StringComparison.OrdinalIgnoreCase));
                if (content != null)
                {
                    productModel.Content = content.Content;
                }
            }

            // add meta fields
            if (product.Properties != null && product.Properties.Any())
            {
                var fieldsCollection = new MetafieldsCollection("global", product.Properties);
                productModel.Metafields = new MetaFieldNamespacesCollection(new[] { fieldsCollection });
            }

            if (product.Seo != null)
            {
                productModel.Keywords = product.Seo.Select(k => k.AsWebModel());
            }

            productModel.Url = GetUrl(productModel);

            return productModel;
        }

        public static Variant AsVariantWebModel(this Data.CatalogItem variation)
        {
            var variantModel = new Variant();

            variantModel.InjectFrom(variation);
            variantModel.Id = variation.Id;
            variantModel.Sku = variation.Code;
            variantModel.Title = variation.Name;
            variantModel.Available = true;

            if (variation.Images != null && variation.Images.Any())
            {
                variantModel.Images = variation.Images.Select(i => i.AsWebModel());
            }

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

            return String.Format("/products/{0}", product.Handle);
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