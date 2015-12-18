using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/product
    /// </summary>
    public class Product : Drop
    {
   
        public Product()
        {
            Variants = new List<Variant>();
            Properties = new List<ProductProperty>();
 
        }

        public string CategoryId { get; set; }

        public string CatalogId { get; set; }

        /// <summary>
        /// Returns true if a product is available for purchase. Returns false if all of the products variants' inventory_quantity values are zero or less, 
        /// and their inventory_policy is not set to "Allow users to purchase this item, even if it is no longer in stock."
        /// </summary>
        public bool Available { get; set; }
        public decimal CompareAtPrice { get; set; }
        /// <summary>
        /// Returns the highest compare at price. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal CompareAtPriceMax { get; set; }

        /// <summary>
        /// Returns the lowest compare at price. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal CompareAtPriceMin { get; set; }

        /// <summary>
        /// Returns true if the compare_at_price_min is different from the compare_at_price_max. Returns false if they are the same.
        /// </summary>
        public bool CompareAtPriceVaries { get; set; }

        /// <summary>
        /// Returns the description of the product. Alias for product.description.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Returns the description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The main product image 
        /// </summary>
        public Image FeaturedImage { get; set; }

        /// <summary>
        /// Returns the variant object of the first product variant that is available for purchase. 
        /// In order for a variant to be available, its variant.inventory_quantity must be greater than zero or variant.inventory_policy must be set to continue. 
        /// A variant with no inventory_policy is considered available.
        /// </summary>
        public Variant FirstAvailableVariant { get; set; }

        public string Handle { get; set; }

        public string Id { get; set; }

        public Image[] Images { get; set; }

        public MetaFieldNamespacesCollection Metafields { get; set; }

        /// <summary>
        /// Returns an array of the product's options. (Color Size Material)
        /// </summary>
        public string[] Options { get; set; }

        /// <summary>
        /// Returns the price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Returns the highest price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal PriceMax { get; set; }

        /// <summary>
        /// Returns the lowest price of the product. Use one of the money filters to return the value in a monetary format.
        /// </summary>
        public decimal PriceMin { get; set; }

        public bool PriceVaries { get; set; }

        /// <summary>
        /// Returns the variant object of the currently-selected variant if there is a valid ?variant= query parameter in the URL. 
        /// If there is no selected variant, the first available variant is returned. In order for a variant to be available,
        ///  its variant.inventory_quantity must be greater than zero or variant.inventory_policy must be set to continue. 
        /// A variant with no inventory_management is considered available.
        /// </summary>
        public Variant SelectedOrFirstAvailableVariant
        {
            get
            {
                return this.SelectedVariant ?? this.FirstAvailableVariant;
            }
        }

        /// <summary>
        /// Returns the variant object of the currently-selected variant if there is a valid ?variant= parameter in the URL. Returns nil if there is not.
        /// </summary>
        public Variant SelectedVariant { get; set; }

        /// <summary>
        /// Returns an array of all of the product's tags. The tags are returned in alphabetical order.
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Returns the name of the custom product template assigned to the product, without the product. prefix nor the .liquid suffix. 
        /// Returns nil if a custom template is not assigned to the product.
        /// </summary>
        public string TemplateSuffix { get; set; }

        public string Title { get; set; }


        public string Type { get; set; }

        public string Url { get; set; }

        public ICollection<Variant> Variants { get; set; }
        public ICollection<ProductProperty> Properties { get; set; }

        public string Vendor { get; set; }

  
    }
}
