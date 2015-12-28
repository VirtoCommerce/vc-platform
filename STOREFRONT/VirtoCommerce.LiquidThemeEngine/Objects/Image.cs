using DotLiquid;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// Represents image objects
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/product#product-image
    /// </remarks>
    [DataContract]
    public class Image : Drop
    {
        /// <summary>
        /// Returns the alt tag of the image
        /// </summary>
        [DataMember]
        public string Alt { get; set; }

        /// <summary>
        /// Returns true if the image has been associated with a variant. Returns false otherwise.
        /// </summary>
        [DataMember]
        public bool? AttachedToVariant { get; set; }

        /// <summary>
        /// Returns the value of product id.
        /// </summary>
        [DataMember]
        public string ProductId { get; set; }

        /// <summary>
        /// Returns the position of the image, starting at 1.
        /// </summary>
        [DataMember]
        public int Position { get; set; }

        /// <summary>
        /// Returns the relative path of the product image. This is the same as outputting {{ image }}.
        /// </summary>
        [DataMember]
        public string Src { get; set; }

        /// <summary>
        /// Returns the value of image name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Returns the variant object(s) that the image is associated with.
        /// </summary>
        [DataMember]
        public Variant[] Variants { get; set; }

        public override string ToString()
        {
            return Src;
        }
    }
}