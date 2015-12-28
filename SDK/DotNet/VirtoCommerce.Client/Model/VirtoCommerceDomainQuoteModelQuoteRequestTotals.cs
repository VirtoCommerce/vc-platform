using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class VirtoCommerceDomainQuoteModelQuoteRequestTotals : IEquatable<VirtoCommerceDomainQuoteModelQuoteRequestTotals>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceDomainQuoteModelQuoteRequestTotals" /> class.
        /// </summary>
        public VirtoCommerceDomainQuoteModelQuoteRequestTotals()
        {
            
        }

        
        /// <summary>
        /// Gets or Sets OriginalSubTotalExlTax
        /// </summary>
        [DataMember(Name="originalSubTotalExlTax", EmitDefaultValue=false)]
        public double? OriginalSubTotalExlTax { get; set; }
  
        
        /// <summary>
        /// Gets or Sets SubTotalExlTax
        /// </summary>
        [DataMember(Name="subTotalExlTax", EmitDefaultValue=false)]
        public double? SubTotalExlTax { get; set; }
  
        
        /// <summary>
        /// Gets or Sets ShippingTotal
        /// </summary>
        [DataMember(Name="shippingTotal", EmitDefaultValue=false)]
        public double? ShippingTotal { get; set; }
  
        
        /// <summary>
        /// Gets or Sets DiscountTotal
        /// </summary>
        [DataMember(Name="discountTotal", EmitDefaultValue=false)]
        public double? DiscountTotal { get; set; }
  
        
        /// <summary>
        /// Gets or Sets TaxTotal
        /// </summary>
        [DataMember(Name="taxTotal", EmitDefaultValue=false)]
        public double? TaxTotal { get; set; }
  
        
        /// <summary>
        /// Gets or Sets AdjustmentQuoteExlTax
        /// </summary>
        [DataMember(Name="adjustmentQuoteExlTax", EmitDefaultValue=false)]
        public double? AdjustmentQuoteExlTax { get; set; }
  
        
        /// <summary>
        /// Gets or Sets GrandTotalExlTax
        /// </summary>
        [DataMember(Name="grandTotalExlTax", EmitDefaultValue=false)]
        public double? GrandTotalExlTax { get; set; }
  
        
        /// <summary>
        /// Gets or Sets GrandTotalInclTax
        /// </summary>
        [DataMember(Name="grandTotalInclTax", EmitDefaultValue=false)]
        public double? GrandTotalInclTax { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainQuoteModelQuoteRequestTotals {\n");
            sb.Append("  OriginalSubTotalExlTax: ").Append(OriginalSubTotalExlTax).Append("\n");
            sb.Append("  SubTotalExlTax: ").Append(SubTotalExlTax).Append("\n");
            sb.Append("  ShippingTotal: ").Append(ShippingTotal).Append("\n");
            sb.Append("  DiscountTotal: ").Append(DiscountTotal).Append("\n");
            sb.Append("  TaxTotal: ").Append(TaxTotal).Append("\n");
            sb.Append("  AdjustmentQuoteExlTax: ").Append(AdjustmentQuoteExlTax).Append("\n");
            sb.Append("  GrandTotalExlTax: ").Append(GrandTotalExlTax).Append("\n");
            sb.Append("  GrandTotalInclTax: ").Append(GrandTotalInclTax).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceDomainQuoteModelQuoteRequestTotals);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainQuoteModelQuoteRequestTotals instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceDomainQuoteModelQuoteRequestTotals to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainQuoteModelQuoteRequestTotals other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.OriginalSubTotalExlTax == other.OriginalSubTotalExlTax ||
                    this.OriginalSubTotalExlTax != null &&
                    this.OriginalSubTotalExlTax.Equals(other.OriginalSubTotalExlTax)
                ) && 
                (
                    this.SubTotalExlTax == other.SubTotalExlTax ||
                    this.SubTotalExlTax != null &&
                    this.SubTotalExlTax.Equals(other.SubTotalExlTax)
                ) && 
                (
                    this.ShippingTotal == other.ShippingTotal ||
                    this.ShippingTotal != null &&
                    this.ShippingTotal.Equals(other.ShippingTotal)
                ) && 
                (
                    this.DiscountTotal == other.DiscountTotal ||
                    this.DiscountTotal != null &&
                    this.DiscountTotal.Equals(other.DiscountTotal)
                ) && 
                (
                    this.TaxTotal == other.TaxTotal ||
                    this.TaxTotal != null &&
                    this.TaxTotal.Equals(other.TaxTotal)
                ) && 
                (
                    this.AdjustmentQuoteExlTax == other.AdjustmentQuoteExlTax ||
                    this.AdjustmentQuoteExlTax != null &&
                    this.AdjustmentQuoteExlTax.Equals(other.AdjustmentQuoteExlTax)
                ) && 
                (
                    this.GrandTotalExlTax == other.GrandTotalExlTax ||
                    this.GrandTotalExlTax != null &&
                    this.GrandTotalExlTax.Equals(other.GrandTotalExlTax)
                ) && 
                (
                    this.GrandTotalInclTax == other.GrandTotalInclTax ||
                    this.GrandTotalInclTax != null &&
                    this.GrandTotalInclTax.Equals(other.GrandTotalInclTax)
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
                
                if (this.OriginalSubTotalExlTax != null)
                    hash = hash * 57 + this.OriginalSubTotalExlTax.GetHashCode();
                
                if (this.SubTotalExlTax != null)
                    hash = hash * 57 + this.SubTotalExlTax.GetHashCode();
                
                if (this.ShippingTotal != null)
                    hash = hash * 57 + this.ShippingTotal.GetHashCode();
                
                if (this.DiscountTotal != null)
                    hash = hash * 57 + this.DiscountTotal.GetHashCode();
                
                if (this.TaxTotal != null)
                    hash = hash * 57 + this.TaxTotal.GetHashCode();
                
                if (this.AdjustmentQuoteExlTax != null)
                    hash = hash * 57 + this.AdjustmentQuoteExlTax.GetHashCode();
                
                if (this.GrandTotalExlTax != null)
                    hash = hash * 57 + this.GrandTotalExlTax.GetHashCode();
                
                if (this.GrandTotalInclTax != null)
                    hash = hash * 57 + this.GrandTotalInclTax.GetHashCode();
                
                return hash;
            }
        }

    }


}
