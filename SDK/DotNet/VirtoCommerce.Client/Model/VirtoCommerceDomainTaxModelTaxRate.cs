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
    public partial class VirtoCommerceDomainTaxModelTaxRate :  IEquatable<VirtoCommerceDomainTaxModelTaxRate>
    {
        /// <summary>
        /// Gets or Sets Rate
        /// </summary>
        [DataMember(Name="rate", EmitDefaultValue=false)]
        public double? Rate { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets Line
        /// </summary>
        [DataMember(Name="line", EmitDefaultValue=false)]
        public VirtoCommerceDomainTaxModelTaxLine Line { get; set; }

        /// <summary>
        /// Gets or Sets TaxProvider
        /// </summary>
        [DataMember(Name="taxProvider", EmitDefaultValue=false)]
        public VirtoCommerceDomainTaxModelTaxProvider TaxProvider { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainTaxModelTaxRate {\n");
            sb.Append("  Rate: ").Append(Rate).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Line: ").Append(Line).Append("\n");
            sb.Append("  TaxProvider: ").Append(TaxProvider).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainTaxModelTaxRate);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainTaxModelTaxRate instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainTaxModelTaxRate to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainTaxModelTaxRate other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Rate == other.Rate ||
                    this.Rate != null &&
                    this.Rate.Equals(other.Rate)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Line == other.Line ||
                    this.Line != null &&
                    this.Line.Equals(other.Line)
                ) && 
                (
                    this.TaxProvider == other.TaxProvider ||
                    this.TaxProvider != null &&
                    this.TaxProvider.Equals(other.TaxProvider)
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

                if (this.Rate != null)
                    hash = hash * 59 + this.Rate.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Line != null)
                    hash = hash * 59 + this.Line.GetHashCode();

                if (this.TaxProvider != null)
                    hash = hash * 59 + this.TaxProvider.GetHashCode();

                return hash;
            }
        }

    }
}
