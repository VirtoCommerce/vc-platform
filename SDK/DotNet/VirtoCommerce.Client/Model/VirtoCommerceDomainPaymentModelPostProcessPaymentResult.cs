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
    public partial class VirtoCommerceDomainPaymentModelPostProcessPaymentResult :  IEquatable<VirtoCommerceDomainPaymentModelPostProcessPaymentResult>
    {
        /// <summary>
        /// Gets or Sets NewPaymentStatus
        /// </summary>
        [DataMember(Name="newPaymentStatus", EmitDefaultValue=false)]
        public string NewPaymentStatus { get; set; }

        /// <summary>
        /// Gets or Sets IsSuccess
        /// </summary>
        [DataMember(Name="isSuccess", EmitDefaultValue=false)]
        public bool? IsSuccess { get; set; }

        /// <summary>
        /// Gets or Sets ErrorMessage
        /// </summary>
        [DataMember(Name="errorMessage", EmitDefaultValue=false)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or Sets ReturnUrl
        /// </summary>
        [DataMember(Name="returnUrl", EmitDefaultValue=false)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or Sets OrderId
        /// </summary>
        [DataMember(Name="orderId", EmitDefaultValue=false)]
        public string OrderId { get; set; }

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
            sb.Append("class VirtoCommerceDomainPaymentModelPostProcessPaymentResult {\n");
            sb.Append("  NewPaymentStatus: ").Append(NewPaymentStatus).Append("\n");
            sb.Append("  IsSuccess: ").Append(IsSuccess).Append("\n");
            sb.Append("  ErrorMessage: ").Append(ErrorMessage).Append("\n");
            sb.Append("  ReturnUrl: ").Append(ReturnUrl).Append("\n");
            sb.Append("  OrderId: ").Append(OrderId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainPaymentModelPostProcessPaymentResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainPaymentModelPostProcessPaymentResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainPaymentModelPostProcessPaymentResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainPaymentModelPostProcessPaymentResult other)
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
                    this.IsSuccess == other.IsSuccess ||
                    this.IsSuccess != null &&
                    this.IsSuccess.Equals(other.IsSuccess)
                ) && 
                (
                    this.ErrorMessage == other.ErrorMessage ||
                    this.ErrorMessage != null &&
                    this.ErrorMessage.Equals(other.ErrorMessage)
                ) && 
                (
                    this.ReturnUrl == other.ReturnUrl ||
                    this.ReturnUrl != null &&
                    this.ReturnUrl.Equals(other.ReturnUrl)
                ) && 
                (
                    this.OrderId == other.OrderId ||
                    this.OrderId != null &&
                    this.OrderId.Equals(other.OrderId)
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

                if (this.IsSuccess != null)
                    hash = hash * 59 + this.IsSuccess.GetHashCode();

                if (this.ErrorMessage != null)
                    hash = hash * 59 + this.ErrorMessage.GetHashCode();

                if (this.ReturnUrl != null)
                    hash = hash * 59 + this.ReturnUrl.GetHashCode();

                if (this.OrderId != null)
                    hash = hash * 59 + this.OrderId.GetHashCode();

                if (this.OuterId != null)
                    hash = hash * 59 + this.OuterId.GetHashCode();

                return hash;
            }
        }

    }
}
