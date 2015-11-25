using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;
using PagedList;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/collection
    /// </summary>
    public class Collection : Drop
    {
        private readonly Storefront.Model.WorkContext _context;
        private readonly Storefront.Model.Common.IStorefrontUrlBuilder _urlBuilder;
        private readonly Storefront.Model.Catalog.Category _category;
        private readonly IStorefrontPagedList<Storefront.Model.Catalog.Product> _products;
        public Collection(Storefront.Model.Catalog.Category category, Storefront.Model.Common.IStorefrontUrlBuilder urlBuilder, Storefront.Model.WorkContext context)
        {
            _context = context;
            _urlBuilder = urlBuilder;
            _category = category;

            Description = _category.Name;
            Handle = _category.SeoInfo != null ? _category.SeoInfo.Slug : _category.Name;
            Image = _category.PrimaryImage != null ? new Image(_category.PrimaryImage) : null;
            Title = _category.Name;
            Url = _category.SeoInfo != null ? _category.SeoInfo.Slug : category.Id;

        }
        public Collection(IStorefrontPagedList<Storefront.Model.Catalog.Product> products, Storefront.Model.Common.IStorefrontUrlBuilder urlBuilder, Storefront.Model.WorkContext context)
        {
            _context = context;
            _urlBuilder = urlBuilder;
            _products = products;
            Products =  new StorefrontPagedList<Product>(_products.Select(x => new Product(x, _urlBuilder, _context)), _products, _products.GetPageUrl);
            ProductsCount = _products.TotalItemCount;
            AllProductsCount = _products.TotalItemCount;
        }
        /// <summary>
        /// Returns a list of all product types in a collection.
        /// </summary>
        public int AllProductsCount { get; private set; }
        /// <summary>
        /// Returns a list of all product types in a collection.
        /// </summary>
        public string[] AllTypes { get; private set; }

        /// <summary>
        /// Returns a list of all product vendors in a collection.
        /// </summary>
        public string[] AllVendors { get; private set; }

        /// <summary>
        /// Returns the product type on a /collections/types?q=TYPE collection page. For example, you may be on the automatic Shirts collection, which lists all products of type 'Shirts' in the store: myshop.shopify.com/collections/types?q=Shirts.
        /// </summary>
        public string CurrentType { get; private set; }

        /// <summary>
        /// Returns the vendor name on a /collections/vendors?q=VENDOR collection page. For example, you may be on the automatic Shopify collection, which lists all products with vendor 'Shopify' in the store: myshop.shopify.com/collections/vendors?q=Shopify.
        /// </summary>
        public string CurrentVendor { get; private set; }

        /// <summary>
        /// Returns the sort order of the collection, which is set in the collection pages of the Admin.
        /// </summary>
        public string DefaultSortBy { get; private set; }

        /// <summary>
        /// Returns the description of the collection.
        /// </summary>
        public string Description { get; private set; }
        //Returns the handle of a collection.
        public string Handle { get; private set; }
        /// <summary>
        /// Returns the id of the collection.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Returns the collection image. Use the img_url filter to link it to the image file on the Shopify CDN. Check for the presence of the image first.
        /// </summary>
        public Image Image { get; private set; }


        /// <summary>
        /// Returns the URL of the next product in the collection. Returns nil if there is no next product.
        /// </summary>
        public string NextProduct { get; private set; }

        /// <summary>
        /// Returns the URL of the previous product in the collection. Returns nil if there is no previous product.
        /// </summary>
        public string PreviousProduct { get; private set; }

        public IStorefrontPagedList<Product> Products { get; private set; }

        public int ProductsCount { get; private set; }


        /// <summary>
        /// Returns all tags of all products in a collection.
        /// </summary>
        public string[] Tags { get; private set; }

        /// <summary>
        /// Returns the name of the custom collection template assigned to the collection, without the collection.
        /// </summary>
        public string TemplateSuffix { get; private set; }

        /// <summary>
        /// Returns the title of the collection.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Returns the URL of the collection.
        /// </summary>
        public string Url { get; private set; }


    }
}
