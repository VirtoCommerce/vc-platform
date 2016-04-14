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
    public partial class VirtoCommerceDomainPaymentModelBankCardInfo :  IEquatable<VirtoCommerceDomainPaymentModelBankCardInfo>
    {
        /// <summary>
        /// Gets or Sets BankCardNumber
        /// </summary>
        [DataMember(Name="bankCardNumber", EmitDefaultValue=false)]
        public string BankCardNumber { get; set; }

        /// <summary>
        /// Gets or Sets BankCardType
        /// </summary>
        [DataMember(Name="bankCardType", EmitDefaultValue=false)]
        public string BankCardType { get; set; }

        /// <summary>
        /// Gets or Sets BankCardMonth
        /// </summary>
        [DataMember(Name="bankCardMonth", EmitDefaultValue=false)]
        public int? BankCardMonth { get; set; }

        /// <summary>
        /// Gets or Sets BankCardYear
        /// </summary>
        [DataMember(Name="bankCardYear", EmitDefaultValue=false)]
        public int? BankCardYear { get; set; }

        /// <summary>
        /// Gets or Sets BankCardCVV2
        /// </summary>
        [DataMember(Name="bankCardCVV2", EmitDefaultValue=false)]
        public string BankCardCVV2 { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainPaymentModelBankCardInfo {\n");
            sb.Append("  BankCardNumber: ").Append(BankCardNumber).Append("\n");
            sb.Append("  BankCardType: ").Append(BankCardType).Append("\n");
            sb.Append("  BankCardMonth: ").Append(BankCardMonth).Append("\n");
            sb.Append("  BankCardYear: ").Append(BankCardYear).Append("\n");
            sb.Append("  BankCardCVV2: ").Append(BankCardCVV2).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainPaymentModelBankCardInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainPaymentModelBankCardInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainPaymentModelBankCardInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainPaymentModelBankCardInfo other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.BankCardNumber == other.BankCardNumber ||
                    this.BankCardNumber != null &&
                    this.BankCardNumber.Equals(other.BankCardNumber)
                ) && 
                (
                    this.BankCardType == other.BankCardType ||
                    this.BankCardType != null &&
                    this.BankCardType.Equals(other.BankCardType)
                ) && 
                (
                    this.BankCardMonth == other.BankCardMonth ||
                    this.BankCardMonth != null &&
                    this.BankCardMonth.Equals(other.BankCardMonth)
                ) && 
                (
                    this.BankCardYear == other.BankCardYear ||
                    this.BankCardYear != null &&
                    this.BankCardYear.Equals(other.BankCardYear)
                ) && 
                (
                    this.BankCardCVV2 == other.BankCardCVV2 ||
                    this.BankCardCVV2 != null &&
                    this.BankCardCVV2.Equals(other.BankCardCVV2)
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

                if (this.BankCardNumber != null)
                    hash = hash * 59 + this.BankCardNumber.GetHashCode();

                if (this.BankCardType != null)
                    hash = hash * 59 + this.BankCardType.GetHashCode();

                if (this.BankCardMonth != null)
                    hash = hash * 59 + this.BankCardMonth.GetHashCode();

                if (this.BankCardYear != null)
                    hash = hash * 59 + this.BankCardYear.GetHashCode();

                if (this.BankCardCVV2 != null)
                    hash = hash * 59 + this.BankCardCVV2.GetHashCode();

                return hash;
            }
        }

    }
}
