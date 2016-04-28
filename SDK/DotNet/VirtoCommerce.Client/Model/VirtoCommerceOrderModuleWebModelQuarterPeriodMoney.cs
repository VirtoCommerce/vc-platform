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
    public partial class VirtoCommerceOrderModuleWebModelQuarterPeriodMoney :  IEquatable<VirtoCommerceOrderModuleWebModelQuarterPeriodMoney>
    {
        /// <summary>
        /// Gets or Sets Year
        /// </summary>
        [DataMember(Name="year", EmitDefaultValue=false)]
        public int? Year { get; set; }

        /// <summary>
        /// Gets or Sets Quarter
        /// </summary>
        [DataMember(Name="quarter", EmitDefaultValue=false)]
        public int? Quarter { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets Amount
        /// </summary>
        [DataMember(Name="amount", EmitDefaultValue=false)]
        public double? Amount { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelQuarterPeriodMoney {\n");
            sb.Append("  Year: ").Append(Year).Append("\n");
            sb.Append("  Quarter: ").Append(Quarter).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelQuarterPeriodMoney);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelQuarterPeriodMoney instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelQuarterPeriodMoney to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelQuarterPeriodMoney other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Year == other.Year ||
                    this.Year != null &&
                    this.Year.Equals(other.Year)
                ) && 
                (
                    this.Quarter == other.Quarter ||
                    this.Quarter != null &&
                    this.Quarter.Equals(other.Quarter)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Amount == other.Amount ||
                    this.Amount != null &&
                    this.Amount.Equals(other.Amount)
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

                if (this.Year != null)
                    hash = hash * 59 + this.Year.GetHashCode();

                if (this.Quarter != null)
                    hash = hash * 59 + this.Quarter.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Amount != null)
                    hash = hash * 59 + this.Amount.GetHashCode();

                return hash;
            }
        }

    }
}
