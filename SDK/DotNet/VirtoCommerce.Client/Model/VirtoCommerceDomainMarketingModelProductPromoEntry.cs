using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceDomainMarketingModelProductPromoEntry :  IEquatable<VirtoCommerceDomainMarketingModelProductPromoEntry>
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        [DataMember(Name="quantity", EmitDefaultValue=false)]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or Sets Price
        /// </summary>
        [DataMember(Name="price", EmitDefaultValue=false)]
        public double? Price { get; set; }

        /// <summary>
        /// Gets or Sets Discount
        /// </summary>
        [DataMember(Name="discount", EmitDefaultValue=false)]
        public double? Discount { get; set; }

        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets Owner
        /// </summary>
        [DataMember(Name="owner", EmitDefaultValue=false)]
        public Object Owner { get; set; }

        /// <summary>
        /// Gets or Sets Outline
        /// </summary>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }

        /// <summary>
        /// Gets or Sets Variations
        /// </summary>
        [DataMember(Name="variations", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainMarketingModelProductPromoEntry> Variations { get; set; }

        /// <summary>
        /// Gets or Sets Attributes
        /// </summary>
        [DataMember(Name="attributes", EmitDefaultValue=false)]
        public Dictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainMarketingModelProductPromoEntry {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Quantity: ").Append(Quantity).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
            sb.Append("  Discount: ").Append(Discount).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  Variations: ").Append(Variations).Append("\n");
            sb.Append("  Attributes: ").Append(Attributes).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceDomainMarketingModelProductPromoEntry);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainMarketingModelProductPromoEntry instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainMarketingModelProductPromoEntry to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainMarketingModelProductPromoEntry other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Quantity == other.Quantity ||
                    this.Quantity != null &&
                    this.Quantity.Equals(other.Quantity)
                ) && 
                (
                    this.Price == other.Price ||
                    this.Price != null &&
                    this.Price.Equals(other.Price)
                ) && 
                (
                    this.Discount == other.Discount ||
                    this.Discount != null &&
                    this.Discount.Equals(other.Discount)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.Owner == other.Owner ||
                    this.Owner != null &&
                    this.Owner.Equals(other.Owner)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.Variations == other.Variations ||
                    this.Variations != null &&
                    this.Variations.SequenceEqual(other.Variations)
                ) && 
                (
                    this.Attributes == other.Attributes ||
                    this.Attributes != null &&
                    this.Attributes.SequenceEqual(other.Attributes)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.Quantity != null)
                    hash = hash * 59 + this.Quantity.GetHashCode();

                if (this.Price != null)
                    hash = hash * 59 + this.Price.GetHashCode();

                if (this.Discount != null)
                    hash = hash * 59 + this.Discount.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.Owner != null)
                    hash = hash * 59 + this.Owner.GetHashCode();

                if (this.Outline != null)
                    hash = hash * 59 + this.Outline.GetHashCode();

                if (this.Variations != null)
                    hash = hash * 59 + this.Variations.GetHashCode();

                if (this.Attributes != null)
                    hash = hash * 59 + this.Attributes.GetHashCode();

                return hash;
            }
        }

    }
}
