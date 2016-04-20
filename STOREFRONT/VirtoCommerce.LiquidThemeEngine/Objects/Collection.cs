using DotLiquid;
using System.Runtime.Serialization;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents shopping cart object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/collection
    /// </remarks>
    [DataContract]
    public class Collection : Drop
    {
        /// <summary>
        /// Returns collection total products count
        /// </summary>
        [DataMember]
        public int AllProductsCount { get { return Products.GetTotalCount(); } }

        /// <summary>
        /// Returns all tags of all products in a collection.
        /// </summary>
        public TagCollection AllTags { get { return Tags; } }

        /// <summary>
        /// Returns a list of all product types in a collection.
        /// </summary>
        [DataMember]
        public string[] AllTypes { get; set; }

        /// <summary>
        /// Returns a list of all product vendors in a collection.
        /// </summary>
        [DataMember]
        public string[] AllVendors { get; set; }

        /// <summary>
        /// Returns the product type on a /collections/types?q=TYPE collection page.
        /// For example, you may be on the automatic Shirts collection, which lists all
        /// products of type 'Shirts' in the store: myshop.shopify.com/collections/types?q=Shirts.
        /// </summary>
        [DataMember]
        public string CurrentType { get; set; }

        /// <summary>
        /// Returns the vendor name on a /collections/vendors?q=VENDOR collection page.
        /// For example, you may be on the automatic Shopify collection, which lists all
        /// products with vendor 'Shopify' in the store: myshop.shopify.com/collections/vendors?q=Shopify.
        /// </summary>
        [DataMember]
        public string CurrentVendor { get; set; }

        /// <summary>
        /// Returns the default sort order of the collection.
        /// </summary>
        [DataMember]
        public string DefaultSortBy { get; set; }

        /// <summary>
        /// Returns the actual sort order of the collection.
        /// </summary>
        [DataMember]
        public string SortBy { get; set; }

        /// <summary>
        /// Returns the description of the collection.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Returns the handle of a collection.
        /// </summary>
        [DataMember]
        public string Handle { get; set; }

        /// <summary>
        /// Returns the id of the collection.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Returns the collection image. Use the img_url filter to link it to the image file
        /// on the Shopify CDN. Check for the presence of the image first.
        /// </summary>
        [DataMember]
        public Image Image { get; set; }

        /// <summary>
        /// Returns the URL of the next product in the collection. Returns nil if there is no next product.
        /// </summary>
        [DataMember]
        public string NextProduct { get; set; }

        /// <summary>
        /// Returns the URL of the previous product in the collection. Returns nil if there is
        /// no previous product.
        /// </summary>
        [DataMember]
        public string PreviousProduct { get; set; }

        /// <summary>
        /// Returns paged collection of products
        /// </summary>
        public IMutablePagedList<Product> Products { get; set; }

        /// <summary>
        /// Returns collection total products count
        /// </summary>
        [DataMember]
        public int ProductsCount { get { return Products.GetTotalCount(); } }

        /// <summary>
        /// Returns all tags of all products in a collection.
        /// </summary>
        public TagCollection Tags { get; set; }

        /// <summary>
        /// Returns the name of the custom collection template assigned to the collection,
        /// without the collection.
        /// </summary>
        [DataMember]
        public string TemplateSuffix { get; set; }

        /// <summary>
        /// Returns the title of the collection.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Returns the URL of the collection.
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Shop metafields
        /// </summary>
        public MetaFieldNamespacesCollection Metafields { get; set; }
    }
}
