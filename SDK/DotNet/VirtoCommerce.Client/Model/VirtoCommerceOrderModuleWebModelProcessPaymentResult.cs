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
    /// Represent process payment request result
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceOrderModuleWebModelProcessPaymentResult :  IEquatable<VirtoCommerceOrderModuleWebModelProcessPaymentResult>
    {
        /// <summary>
        /// Gets or Sets NewPaymentStatus
        /// </summary>
        [DataMember(Name="newPaymentStatus", EmitDefaultValue=false)]
        public string NewPaymentStatus { get; set; }

        /// <summary>
        /// Gets or Sets PaymentMethodType
        /// </summary>
        [DataMember(Name="paymentMethodType", EmitDefaultValue=false)]
        public string PaymentMethodType { get; set; }

        /// <summary>
        /// Redirect url used for OutSite payment processing
        /// </summary>
        /// <value>Redirect url used for OutSite payment processing</value>
        [DataMember(Name="redirectUrl", EmitDefaultValue=false)]
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Gets or Sets IsSuccess
        /// </summary>
        [DataMember(Name="isSuccess", EmitDefaultValue=false)]
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// Gets or Sets Error
        /// </summary>
        [DataMember(Name="error", EmitDefaultValue=false)]
        public string Error { get; set; }

        /// <summary>
        /// Generated Html form used for InSite payment processing
        /// </summary>
        /// <value>Generated Html form used for InSite payment processing</value>
        [DataMember(Name="htmlForm", EmitDefaultValue=false)]
        public string HtmlForm { get; set; }

        /// <summary>
        /// Gets or Sets OuterId
        /// </summary>
        [DataMember(Name="outerId", EmitDefaultValue=false)]
        public string OuterId { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelProcessPaymentResult {\n");
            sb.Append("  NewPaymentStatus: ").Append(NewPaymentStatus).Append("\n");
            sb.Append("  PaymentMethodType: ").Append(PaymentMethodType).Append("\n");
            sb.Append("  RedirectUrl: ").Append(RedirectUrl).Append("\n");
            sb.Append("  IsSuccess: ").Append(IsSuccess).Append("\n");
            sb.Append("  Error: ").Append(Error).Append("\n");
            sb.Append("  HtmlForm: ").Append(HtmlForm).Append("\n");
            sb.Append("  OuterId: ").Append(OuterId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelProcessPaymentResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelProcessPaymentResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelProcessPaymentResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelProcessPaymentResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.NewPaymentStatus == other.NewPaymentStatus ||
                    this.NewPaymentStatus != null &&
                    this.NewPaymentStatus.Equals(other.NewPaymentStatus)
                ) && 
                (
                    this.PaymentMethodType == other.PaymentMethodType ||
                    this.PaymentMethodType != null &&
                    this.PaymentMethodType.Equals(other.PaymentMethodType)
                ) && 
                (
                    this.RedirectUrl == other.RedirectUrl ||
                    this.RedirectUrl != null &&
                    this.RedirectUrl.Equals(other.RedirectUrl)
                ) && 
                (
                    this.IsSuccess == other.IsSuccess ||
                    this.IsSuccess != null &&
                    this.IsSuccess.Equals(other.IsSuccess)
                ) && 
                (
                    this.Error == other.Error ||
                    this.Error != null &&
                    this.Error.Equals(other.Error)
                ) && 
                (
                    this.HtmlForm == other.HtmlForm ||
                    this.HtmlForm != null &&
                    this.HtmlForm.Equals(other.HtmlForm)
                ) && 
                (
                    this.OuterId == other.OuterId ||
                    this.OuterId != null &&
                    this.OuterId.Equals(other.OuterId)
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

                if (this.NewPaymentStatus != null)
                    hash = hash * 59 + this.NewPaymentStatus.GetHashCode();

                if (this.PaymentMethodType != null)
                    hash = hash * 59 + this.PaymentMethodType.GetHashCode();

                if (this.RedirectUrl != null)
                    hash = hash * 59 + this.RedirectUrl.GetHashCode();

                if (this.IsSuccess != null)
                    hash = hash * 59 + this.IsSuccess.GetHashCode();

                if (this.Error != null)
                    hash = hash * 59 + this.Error.GetHashCode();

                if (this.HtmlForm != null)
                    hash = hash * 59 + this.HtmlForm.GetHashCode();

                if (this.OuterId != null)
                    hash = hash * 59 + this.OuterId.GetHashCode();

                return hash;
            }
        }

    }
}
