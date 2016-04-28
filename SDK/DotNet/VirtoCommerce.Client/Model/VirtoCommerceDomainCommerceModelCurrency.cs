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
    public partial class VirtoCommerceDomainCommerceModelCurrency :  IEquatable<VirtoCommerceDomainCommerceModelCurrency>
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets IsPrimary
        /// </summary>
        [DataMember(Name="isPrimary", EmitDefaultValue=false)]
        public bool? IsPrimary { get; set; }

        /// <summary>
        /// Gets or Sets ExchangeRate
        /// </summary>
        [DataMember(Name="exchangeRate", EmitDefaultValue=false)]
        public double? ExchangeRate { get; set; }

        /// <summary>
        /// Gets or Sets Symbol
        /// </summary>
        [DataMember(Name="symbol", EmitDefaultValue=false)]
        public string Symbol { get; set; }

        /// <summary>
        /// Gets or Sets CustomFormatting
        /// </summary>
        [DataMember(Name="customFormatting", EmitDefaultValue=false)]
        public string CustomFormatting { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCommerceModelCurrency {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IsPrimary: ").Append(IsPrimary).Append("\n");
            sb.Append("  ExchangeRate: ").Append(ExchangeRate).Append("\n");
            sb.Append("  Symbol: ").Append(Symbol).Append("\n");
            sb.Append("  CustomFormatting: ").Append(CustomFormatting).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCommerceModelCurrency);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCommerceModelCurrency instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCommerceModelCurrency to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCommerceModelCurrency other)
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.IsPrimary == other.IsPrimary ||
                    this.IsPrimary != null &&
                    this.IsPrimary.Equals(other.IsPrimary)
                ) && 
                (
                    this.ExchangeRate == other.ExchangeRate ||
                    this.ExchangeRate != null &&
                    this.ExchangeRate.Equals(other.ExchangeRate)
                ) && 
                (
                    this.Symbol == other.Symbol ||
                    this.Symbol != null &&
                    this.Symbol.Equals(other.Symbol)
                ) && 
                (
                    this.CustomFormatting == other.CustomFormatting ||
                    this.CustomFormatting != null &&
                    this.CustomFormatting.Equals(other.CustomFormatting)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.IsPrimary != null)
                    hash = hash * 59 + this.IsPrimary.GetHashCode();

                if (this.ExchangeRate != null)
                    hash = hash * 59 + this.ExchangeRate.GetHashCode();

                if (this.Symbol != null)
                    hash = hash * 59 + this.Symbol.GetHashCode();

                if (this.CustomFormatting != null)
                    hash = hash * 59 + this.CustomFormatting.GetHashCode();

                return hash;
            }
        }

    }
}
