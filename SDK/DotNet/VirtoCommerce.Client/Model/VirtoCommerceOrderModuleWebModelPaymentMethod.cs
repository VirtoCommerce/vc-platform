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
    public partial class VirtoCommerceOrderModuleWebModelPaymentMethod :  IEquatable<VirtoCommerceOrderModuleWebModelPaymentMethod>
    {
        /// <summary>
        /// Gets or sets the value of payment gateway code
        /// </summary>
        /// <value>Gets or sets the value of payment gateway code</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method name
        /// </summary>
        /// <value>Gets or sets the value of payment method name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method logo absolute URL
        /// </summary>
        /// <value>Gets or sets the value of payment method logo absolute URL</value>
        [DataMember(Name="iconUrl", EmitDefaultValue=false)]
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method description
        /// </summary>
        /// <value>Gets or sets the value of payment method description</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method type
        /// </summary>
        /// <value>Gets or sets the value of payment method type</value>
        [DataMember(Name="paymentMethodType", EmitDefaultValue=false)]
        public string PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method group type
        /// </summary>
        /// <value>Gets or sets the value of payment method group type</value>
        [DataMember(Name="paymentMethodGroupType", EmitDefaultValue=false)]
        public string PaymentMethodGroupType { get; set; }

        /// <summary>
        /// Gets or sets the value of payment method priority
        /// </summary>
        /// <value>Gets or sets the value of payment method priority</value>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Is payment method available for partial payments
        /// </summary>
        /// <value>Is payment method available for partial payments</value>
        [DataMember(Name="isAvailableForPartial", EmitDefaultValue=false)]
        public bool? IsAvailableForPartial { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelPaymentMethod {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IconUrl: ").Append(IconUrl).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  PaymentMethodType: ").Append(PaymentMethodType).Append("\n");
            sb.Append("  PaymentMethodGroupType: ").Append(PaymentMethodGroupType).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  IsAvailableForPartial: ").Append(IsAvailableForPartial).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelPaymentMethod);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelPaymentMethod instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelPaymentMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelPaymentMethod other)
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
                    this.IconUrl == other.IconUrl ||
                    this.IconUrl != null &&
                    this.IconUrl.Equals(other.IconUrl)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.PaymentMethodType == other.PaymentMethodType ||
                    this.PaymentMethodType != null &&
                    this.PaymentMethodType.Equals(other.PaymentMethodType)
                ) && 
                (
                    this.PaymentMethodGroupType == other.PaymentMethodGroupType ||
                    this.PaymentMethodGroupType != null &&
                    this.PaymentMethodGroupType.Equals(other.PaymentMethodGroupType)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.IsAvailableForPartial == other.IsAvailableForPartial ||
                    this.IsAvailableForPartial != null &&
                    this.IsAvailableForPartial.Equals(other.IsAvailableForPartial)
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

                if (this.IconUrl != null)
                    hash = hash * 59 + this.IconUrl.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.PaymentMethodType != null)
                    hash = hash * 59 + this.PaymentMethodType.GetHashCode();

                if (this.PaymentMethodGroupType != null)
                    hash = hash * 59 + this.PaymentMethodGroupType.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.IsAvailableForPartial != null)
                    hash = hash * 59 + this.IsAvailableForPartial.GetHashCode();

                return hash;
            }
        }

    }
}
