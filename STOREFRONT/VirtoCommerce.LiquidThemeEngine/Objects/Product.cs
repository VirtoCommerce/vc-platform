using DotLiquid;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents the product object
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/product
    /// </remarks>
    [DataContract]
    public class Product : Drop
    {
        public Product()
        {
            Variants = new List<Variant>();
            Properties = new List<ProductProperty>();
        }

        /// <summary>
        /// Returns the value of product category ID
        /// </summary>
        [DataMember]
        public string CategoryId { get; set; }

        /// <summary>
        /// Returns the value of product catalog ID
        /// </summary>
        [DataMember]
        public string CatalogId { get; set; }

        /// <summary>
        /// Returns true if a product is available for purchase.
        /// Returns false if all of the products variants' inventory_quantity values are zero or less,
        /// and their inventory_policy is not set to "Allow users to purchase this item,
        /// even if it is no longer in stock."
        /// </summary>
        [DataMember]
        public bool Available { get; set; }

        /// <summary>
        /// Returns the compare at price. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal CompareAtPrice { get; set; }

        /// <summary>
        /// Returns the highest compare at price. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal CompareAtPriceMax { get; set; }

        /// <summary>
        /// Returns the lowest compare at price. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal CompareAtPriceMin { get; set; }

        /// <summary>
        /// Returns true if the compare_at_price_min is different from the compare_at_price_max.
        /// Returns false if they are the same.
        /// </summary>
        [DataMember]
        public bool CompareAtPriceVaries { get; set; }

        /// <summary>
        /// Returns the description of the product. Alias for product.description.
        /// </summary>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Returns the description of the product.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of product descriptions: FullReview, QuickReview, etc.
        /// </summary>
        public Descriptions Descriptions { get; set; }

        /// <summary>
        /// The main product image 
        /// </summary>
        [DataMember]
        public Image FeaturedImage { get; set; }

        /// <summary>
        /// Returns the variant object of the first product variant that is available for purchase. 
        /// In order for a variant to be available, its variant.inventory_quantity must be greater than zero or
        /// variant.inventory_policy must be set to continue. 
        /// A variant with no inventory_policy is considered available.
        /// </summary>
        [DataMember]
        public Variant FirstAvailableVariant { get; set; }

        /// <summary>
        /// Returns the handle of a product.
        /// </summary>
        [DataMember]
        public string Handle { get; set; }

        /// <summary>
        /// Returns the id of the product.
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Returns an array of the product's images. Use the product_img_url filter to link to the product image.
        /// </summary>
        [DataMember]
        public Image[] Images { get; set; }

        public MetaFieldNamespacesCollection Metafields { get; set; }

        /// <summary>
        /// Returns an array of the product's options. (Color Size Material)
        /// </summary>
        [DataMember]
        public string[] Options { get; set; }

        /// <summary>
        /// Returns the price of the product. Use one of the money filters to return the value in a
        /// monetary format.
        /// </summary>
        [DataMember]
        public decimal Price { get; set; }

        /// <summary>
        /// Returns the highest price of the product. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal PriceMax { get; set; }

        /// <summary>
        /// Returns the lowest price of the product. Use one of the money filters to return
        /// the value in a monetary format.
        /// </summary>
        [DataMember]
        public decimal PriceMin { get; set; }

        /// <summary>
        /// Returns true if the product's variants have varying prices.
        /// Returns false if all of the product's variants have the same price.
        /// </summary>
        [DataMember]
        public bool PriceVaries { get; set; }

        /// <summary>
        /// Returns the variant object of the currently-selected variant if there is a valid
        /// ?variant=query parameter in the URL. If there is no selected variant,
        /// the first available variant is returned. In order for a variant to be available,
        /// its variant.inventory_quantity must be greater than zero or variant.inventory_policy
        /// must be set to continue. A variant with no inventory_management is considered available.
        /// </summary>
        [DataMember]
        public Variant SelectedOrFirstAvailableVariant
        {
            get
            {
                return SelectedVariant ?? FirstAvailableVariant;
            }
        }

        /// <summary>
        /// Returns the variant object of the currently-selected variant if there is a valid
        /// ?variant= parameter in the URL. Returns nil if there is not.
        /// </summary>
        [DataMember]
        public Variant SelectedVariant { get; set; }

        /// <summary>
        /// Returns an array of all of the product's tags. The tags are returned in alphabetical order.
        /// </summary>
        [DataMember]
        public string[] Tags { get; set; }

        /// <summary>
        /// Returns the name of the custom product template assigned to the product, without the product.
        /// prefix nor the .liquid suffix. Returns nil if a custom template is not assigned to the product.
        /// </summary>
        [DataMember]
        public string TemplateSuffix { get; set; }

        /// <summary>
        /// Returns the title of the product.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Returns the type of the product.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Returns the relative URL of the product.
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// Returns an array the product's variants.
        /// </summary>
        [DataMember]
        public ICollection<Variant> Variants { get; set; }

        /// <summary>
        /// Returns a collection of product properties
        /// </summary>
        public ICollection<ProductProperty> Properties { get; set; }

        /// <summary>
        /// Returns the vendor of the product. 
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        /// Gets or sets the sign of product can be added to quote request
        /// </summary>
        [DataMember]
        public bool IsQuotable { get; set; }

        /// <summary>
        /// Gets or sets the collection of related products
        /// </summary>
        public ICollection<Product> RelatedProducts { get; set; }
    }
}