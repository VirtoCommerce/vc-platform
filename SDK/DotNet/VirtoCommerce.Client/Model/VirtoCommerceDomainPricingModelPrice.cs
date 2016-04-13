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
    public partial class VirtoCommerceDomainPricingModelPrice :  IEquatable<VirtoCommerceDomainPricingModelPrice>
    {
        /// <summary>
        /// Gets or Sets PricelistId
        /// </summary>
        [DataMember(Name="pricelistId", EmitDefaultValue=false)]
        public string PricelistId { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets Sale
        /// </summary>
        [DataMember(Name="sale", EmitDefaultValue=false)]
        public double? Sale { get; set; }

        /// <summary>
        /// Gets or Sets List
        /// </summary>
        [DataMember(Name="list", EmitDefaultValue=false)]
        public double? List { get; set; }

        /// <summary>
        /// Gets or Sets MinQuantity
        /// </summary>
        [DataMember(Name="minQuantity", EmitDefaultValue=false)]
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Gets or Sets EffectiveValue
        /// </summary>
        [DataMember(Name="effectiveValue", EmitDefaultValue=false)]
        public double? EffectiveValue { get; private set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainPricingModelPrice {\n");
            sb.Append("  PricelistId: ").Append(PricelistId).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  Sale: ").Append(Sale).Append("\n");
            sb.Append("  List: ").Append(List).Append("\n");
            sb.Append("  MinQuantity: ").Append(MinQuantity).Append("\n");
            sb.Append("  EffectiveValue: ").Append(EffectiveValue).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainPricingModelPrice);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainPricingModelPrice instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainPricingModelPrice to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainPricingModelPrice other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.PricelistId == other.PricelistId ||
                    this.PricelistId != null &&
                    this.PricelistId.Equals(other.PricelistId)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.Sale == other.Sale ||
                    this.Sale != null &&
                    this.Sale.Equals(other.Sale)
                ) && 
                (
                    this.List == other.List ||
                    this.List != null &&
                    this.List.Equals(other.List)
                ) && 
                (
                    this.MinQuantity == other.MinQuantity ||
                    this.MinQuantity != null &&
                    this.MinQuantity.Equals(other.MinQuantity)
                ) && 
                (
                    this.EffectiveValue == other.EffectiveValue ||
                    this.EffectiveValue != null &&
                    this.EffectiveValue.Equals(other.EffectiveValue)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
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

                if (this.PricelistId != null)
                    hash = hash * 59 + this.PricelistId.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.Sale != null)
                    hash = hash * 59 + this.Sale.GetHashCode();

                if (this.List != null)
                    hash = hash * 59 + this.List.GetHashCode();

                if (this.MinQuantity != null)
                    hash = hash * 59 + this.MinQuantity.GetHashCode();

                if (this.EffectiveValue != null)
                    hash = hash * 59 + this.EffectiveValue.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
