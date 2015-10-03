#region
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DotLiquid;
using System;
using VirtoCommerce.Web.Helpers;
using Newtonsoft.Json;
#endregion

namespace VirtoCommerce.Web.Models
{
    [DataContract]
    public class Product : Drop
    {
        public Product()
        {
            this.Variants = new List<Variant>();
        }

        [DataMember]
        public bool Available
        {
            get
            {
                return this.Variants.Any(v => v.Available);
            }
        }


        [DataMember]
        public Collections Collections { get; set; }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal CompareAtPrice
        {
            get
            {
                return SelectedOrFirstAvailableVariant.CompareAtPrice;
            }
        }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal CompareAtPriceMax
        {
            get
            {
                return Variants.Max(v => v.CompareAtPrice);
            }
        }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal CompareAtPriceMin
        {
            get
            {
                return (decimal)Variants.Min(v => v.CompareAtPrice);
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
        public string TaxType { get; set; }

        [DataMember]
        public Image FeaturedImage { get; set; }

        [DataMember]
        public Variant FirstAvailableVariant
        {
            get
            {
                return this.Variants.FirstOrDefault(v => v.Available);
            }
        }

        [DataMember]
        public string Handle { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public ItemCollection<Image> Images { get; set; }

        [DataMember]
        public IEnumerable<SeoKeyword> Keywords { get; set; }

        [DataMember]
        public MetaFieldNamespacesCollection Metafields { get; set; }

        [DataMember]
        public string[] Options { get; set; }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal Price
        {
            get
            {
                return SelectedOrFirstAvailableVariant.Price;
            }
        }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal PriceMax
        {
            get
            {
                return Variants.Max(v => v.Price);
            }
        }

        [DataMember]
        [JsonConverter(typeof(DecimalPriceConverter))]
        public decimal PriceMin
        {
            get
            {
                return Variants.Min(v => v.Price);
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
        public Variant SelectedOrFirstAvailableVariant
        {
            get
            {
                return this.SelectedVariant != null ? this.SelectedVariant : this.FirstAvailableVariant;
            }
        }

        [DataMember]
        public Variant SelectedVariant
        {
            get
            {
                var queryString = System.Web.HttpContext.Current.Request.QueryString;

                return queryString["variant"] != null ?
                    this.Variants.FirstOrDefault(v => v.Id == queryString["variant"]) :
                    this.Variants.FirstOrDefault(v => v.Id == Handle);
            }
        }

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
        public ICollection<Variant> Variants { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public bool IsQuotable { get; set; }

        #region Public Methods and Operators
        public override string ToString()
        {
            return this.Id;
        }

        public override object BeforeMethod(string method)
        {
            return this.SelectedOrFirstAvailableVariant[method];
        }
        #endregion
    }
}