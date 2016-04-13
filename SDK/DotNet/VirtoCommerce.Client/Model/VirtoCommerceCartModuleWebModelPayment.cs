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
    public partial class VirtoCommerceCartModuleWebModelPayment :  IEquatable<VirtoCommerceCartModuleWebModelPayment>
    {
        /// <summary>
        /// Gets or sets the value of payment outer id
        /// </summary>
        /// <value>Gets or sets the value of payment outer id</value>
        [DataMember(Name="outerId", EmitDefaultValue=false)]
        public string OuterId { get; set; }

        /// <summary>
        /// Gets or sets the value of payment gateway code
        /// </summary>
        /// <value>Gets or sets the value of payment gateway code</value>
        [DataMember(Name="paymentGatewayCode", EmitDefaultValue=false)]
        public string PaymentGatewayCode { get; set; }

        /// <summary>
        /// Gets or sets the value of payment currency
        /// </summary>
        /// <value>Gets or sets the value of payment currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of payment amount
        /// </summary>
        /// <value>Gets or sets the value of payment amount</value>
        [DataMember(Name="amount", EmitDefaultValue=false)]
        public double? Amount { get; set; }

        /// <summary>
        /// Gets or sets the billing address
        /// </summary>
        /// <value>Gets or sets the billing address</value>
        [DataMember(Name="billingAddress", EmitDefaultValue=false)]
        public VirtoCommerceCartModuleWebModelAddress BillingAddress { get; set; }

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
            sb.Append("class VirtoCommerceCartModuleWebModelPayment {\n");
            sb.Append("  OuterId: ").Append(OuterId).Append("\n");
            sb.Append("  PaymentGatewayCode: ").Append(PaymentGatewayCode).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  BillingAddress: ").Append(BillingAddress).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelPayment);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelPayment instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelPayment to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelPayment other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.OuterId == other.OuterId ||
                    this.OuterId != null &&
                    this.OuterId.Equals(other.OuterId)
                ) && 
                (
                    this.PaymentGatewayCode == other.PaymentGatewayCode ||
                    this.PaymentGatewayCode != null &&
                    this.PaymentGatewayCode.Equals(other.PaymentGatewayCode)
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
                ) && 
                (
                    this.BillingAddress == other.BillingAddress ||
                    this.BillingAddress != null &&
                    this.BillingAddress.Equals(other.BillingAddress)
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

                if (this.OuterId != null)
                    hash = hash * 59 + this.OuterId.GetHashCode();

                if (this.PaymentGatewayCode != null)
                    hash = hash * 59 + this.PaymentGatewayCode.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Amount != null)
                    hash = hash * 59 + this.Amount.GetHashCode();

                if (this.BillingAddress != null)
                    hash = hash * 59 + this.BillingAddress.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
