using DotLiquid;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/collection
    /// </summary>
    public class Collection : Drop
    {
   
        /// <summary>
        /// Returns a list of all product types in a collection.
        /// </summary>
        public int AllProductsCount { get; set; }
        /// <summary>
        /// Returns a list of all product types in a collection.
        /// </summary>
        public string[] AllTypes { get; set; }

        /// <summary>
        /// Returns a list of all product vendors in a collection.
        /// </summary>
        public string[] AllVendors { get; set; }

        /// <summary>
        /// Returns the product type on a /collections/types?q=TYPE collection page. For example, you may be on the automatic Shirts collection, which lists all products of type 'Shirts' in the store: myshop.shopify.com/collections/types?q=Shirts.
        /// </summary>
        public string CurrentType { get; set; }

        /// <summary>
        /// Returns the vendor name on a /collections/vendors?q=VENDOR collection page. For example, you may be on the automatic Shopify collection, which lists all products with vendor 'Shopify' in the store: myshop.shopify.com/collections/vendors?q=Shopify.
        /// </summary>
        public string CurrentVendor { get; set; }

        /// <summary>
        /// Returns the sort order of the collection, which is set in the collection pages of the Admin.
        /// </summary>
        public string DefaultSortBy { get;  set; }

        /// <summary>
        /// Returns the description of the collection.
        /// </summary>
        public string Description { get; set; }
        //Returns the handle of a collection.
        public string Handle { get;  set; }
        /// <summary>
        /// Returns the id of the collection.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns the collection image. Use the img_url filter to link it to the image file on the Shopify CDN. Check for the presence of the image first.
        /// </summary>
        public Image Image { get; set; }


        /// <summary>
        /// Returns the URL of the next product in the collection. Returns nil if there is no next product.
        /// </summary>
        public string NextProduct { get; set; }

        /// <summary>
        /// Returns the URL of the previous product in the collection. Returns nil if there is no previous product.
        /// </summary>
        public string PreviousProduct { get; set; }

        public IStorefrontPagedList<Product> Products { get; set; }

        public int ProductsCount { get; set; }


        /// <summary>
        /// Returns all tags of all products in a collection.
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Returns the name of the custom collection template assigned to the collection, without the collection.
        /// </summary>
        public string TemplateSuffix { get; set; }

        /// <summary>
        /// Returns the title of the collection.
        /// </summary>
        public string Title { get;  set; }

        /// <summary>
        /// Returns the URL of the collection.
        /// </summary>
        public string Url { get;  set; }


    }
}
