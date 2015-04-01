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
        [DataMember]
        public bool Available { get; set; }

        [DataMember]
        public Collections Collections { get; set; }

        [DataMember]
        public decimal CompareAtPriceMax
        {
            get
            {
                return this.Variants != null ? this.Variants.Max(v => v.CompareAtPrice) : this.Price;
            }
        }

        [DataMember]
        public decimal CompareAtPriceMin
        {
            get
            {
                return this.Variants != null ? this.Variants.Min(v => v.CompareAtPrice) : this.Price;
            }
        }

        [DataMember]
        public bool CompareAtPriceVaries
        {
            get
            {
                return this.CompareAtPriceMax != this.CompareAtPriceMin;
            }
        }

        [DataMember]
        public string Content
        {
            get
            {
                return this.Description;
            }
        }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Excerpt { get; set; }

        [DataMember]
        public Image FeaturedImage { get; set; }

        [DataMember]
        public Variant FirstAvailableVariant { get; set; }

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
        public decimal Price { get; set; }

        [DataMember]
        public decimal PriceMax
        {
            get
            {
                return this.Variants != null ? this.Variants.Max(v => v.Price) : this.Price;
            }
        }

        [DataMember]
        public decimal PriceMin
        {
            get
            {
                return this.Variants != null ? this.Variants.Min(v => v.Price) : this.Price;
            }
        }

        [DataMember]
        public bool PriceVaries
        {
            get
            {
                return this.PriceMin != this.PriceMax;
            }
        }

        [DataMember]
        public Variant SelectedOrFirstAvailableVariant { get; set; }

        [DataMember]
        public Variant SelectedVariant { get; set; }

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