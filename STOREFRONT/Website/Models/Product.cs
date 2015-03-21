#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;

#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Product : Drop
    {
        #region Public Properties
        [DataMember]
        public bool Available { get; set; }

        public Collections Collections { get; set; }

        [DataMember]
        public decimal CompareAtPriceMax
        {
            get
            {
                if (Variants != null && Variants.Any())
                {
                    return Variants.Max(v => v.CompareAtPrice);
                }

                return decimal.Zero;
            }
        }

        [DataMember]
        public decimal CompareAtPriceMin
        {
            get
            {
                if (Variants != null && Variants.Any())
                {
                    return Variants.Min(v=>v.CompareAtPrice);
                }

                return decimal.Zero;
            }
        }

        [DataMember]
        public bool CompareAtPriceVaries
        {
            get
            {
                return CompareAtPriceMax != CompareAtPriceMin;
            }
        }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Excerpt { get; set; }

        [DataMember]
        public Image FeaturedImage
        {
            get
            {
                if (Images != null && Images.Any())
                {
                    var primaryImage = Images.SingleOrDefault(i => i.Name.Equals("primaryimage"));
                    if (primaryImage != null) return primaryImage;

                    return Images.ElementAt(0);
                }

                return null;
            }
        }

        [DataMember]
        public Variant FirstAvailableVariant
        {
            get
            {
                return this.Variants != null && this.Variants.Any() ? this.Variants[0] : null;
            }
        }

        [DataMember]
        public string Handle { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public IEnumerable<Image> Images { get; set; }

        [DataMember]
        public IEnumerable<SeoKeyword> Keywords { get; set; }

        [DataMember]
        public MetaFieldNamespacesCollection Metafields { get; set; }

        [DataMember]
        public ItemCollection<string> Options { get; set; }

        [DataMember]
        public decimal Price
        {
            get
            {
                if (SelectedOrFirstAvailableVariant != null)
                {
                    return SelectedOrFirstAvailableVariant.Price;
                }

                return decimal.Zero;
            }
        }

        [DataMember]
        public decimal PriceMax
        {
            get
            {
                if (Variants != null && Variants.Any())
                {
                    return Variants.Max(v => v.Price);
                }

                return decimal.Zero;
            }
        }

        [DataMember]
        public decimal PriceMin
        {
            get
            {
                if (Variants != null && Variants.Any())
                {
                    return Variants.Max(v => v.Price);
                }

                return decimal.Zero;
            }
        }

        [DataMember]
        public bool PriceVaries
        {
            get
            {
                return PriceMin != PriceMax;
            }
        }

        public Variant SelectedOrFirstAvailableVariant
        {
            get
            {
                return this.Variants != null && this.Variants.Any() ? this.Variants[0] : null;
            }
        }

        [DataMember]
        public string SelectedVariant { get; set; }

        [DataMember]
        public ItemCollection<string> Tags { get; set; }

        [DataMember]
        public string TemplateSuffix { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public List<Variant> Variants { get; set; }

        [DataMember]
        public string Vendor { get; set; }
        #endregion

        #region Public Methods and Operators
        public override string ToString()
        {
            return this.Id;
        }

        public override object BeforeMethod(string method)
        {
            if (SelectedOrFirstAvailableVariant != null)
            {
                return SelectedOrFirstAvailableVariant[method];
            }

            return null;
        }
        #endregion
    }
}