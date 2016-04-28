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
    public partial class VirtoCommerceInventoryModuleWebModelInventoryInfo :  IEquatable<VirtoCommerceInventoryModuleWebModelInventoryInfo>
    {
        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets FulfillmentCenter
        /// </summary>
        [DataMember(Name="fulfillmentCenter", EmitDefaultValue=false)]
        public VirtoCommerceInventoryModuleWebModelFulfillmentCenter FulfillmentCenter { get; set; }

        /// <summary>
        /// Gets or Sets FulfillmentCenterId
        /// </summary>
        [DataMember(Name="fulfillmentCenterId", EmitDefaultValue=false)]
        public string FulfillmentCenterId { get; set; }

        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets InStockQuantity
        /// </summary>
        [DataMember(Name="inStockQuantity", EmitDefaultValue=false)]
        public long? InStockQuantity { get; set; }

        /// <summary>
        /// Gets or Sets ReservedQuantity
        /// </summary>
        [DataMember(Name="reservedQuantity", EmitDefaultValue=false)]
        public long? ReservedQuantity { get; set; }

        /// <summary>
        /// Gets or Sets ReorderMinQuantity
        /// </summary>
        [DataMember(Name="reorderMinQuantity", EmitDefaultValue=false)]
        public long? ReorderMinQuantity { get; set; }

        /// <summary>
        /// Gets or Sets PreorderQuantity
        /// </summary>
        [DataMember(Name="preorderQuantity", EmitDefaultValue=false)]
        public long? PreorderQuantity { get; set; }

        /// <summary>
        /// Gets or Sets BackorderQuantity
        /// </summary>
        [DataMember(Name="backorderQuantity", EmitDefaultValue=false)]
        public long? BackorderQuantity { get; set; }

        /// <summary>
        /// Gets or Sets AllowBackorder
        /// </summary>
        [DataMember(Name="allowBackorder", EmitDefaultValue=false)]
        public bool? AllowBackorder { get; set; }

        /// <summary>
        /// Gets or Sets AllowPreorder
        /// </summary>
        [DataMember(Name="allowPreorder", EmitDefaultValue=false)]
        public bool? AllowPreorder { get; set; }

        /// <summary>
        /// Gets or Sets InTransit
        /// </summary>
        [DataMember(Name="inTransit", EmitDefaultValue=false)]
        public long? InTransit { get; set; }

        /// <summary>
        /// Gets or Sets PreorderAvailabilityDate
        /// </summary>
        [DataMember(Name="preorderAvailabilityDate", EmitDefaultValue=false)]
        public DateTime? PreorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or Sets BackorderAvailabilityDate
        /// </summary>
        [DataMember(Name="backorderAvailabilityDate", EmitDefaultValue=false)]
        public DateTime? BackorderAvailabilityDate { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceInventoryModuleWebModelInventoryInfo {\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
            sb.Append("  FulfillmentCenter: ").Append(FulfillmentCenter).Append("\n");
            sb.Append("  FulfillmentCenterId: ").Append(FulfillmentCenterId).Append("\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  InStockQuantity: ").Append(InStockQuantity).Append("\n");
            sb.Append("  ReservedQuantity: ").Append(ReservedQuantity).Append("\n");
            sb.Append("  ReorderMinQuantity: ").Append(ReorderMinQuantity).Append("\n");
            sb.Append("  PreorderQuantity: ").Append(PreorderQuantity).Append("\n");
            sb.Append("  BackorderQuantity: ").Append(BackorderQuantity).Append("\n");
            sb.Append("  AllowBackorder: ").Append(AllowBackorder).Append("\n");
            sb.Append("  AllowPreorder: ").Append(AllowPreorder).Append("\n");
            sb.Append("  InTransit: ").Append(InTransit).Append("\n");
            sb.Append("  PreorderAvailabilityDate: ").Append(PreorderAvailabilityDate).Append("\n");
            sb.Append("  BackorderAvailabilityDate: ").Append(BackorderAvailabilityDate).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
            return this.Equals(obj as VirtoCommerceInventoryModuleWebModelInventoryInfo);
        }

        /// <summary>
        /// Returns true if VirtoCommerceInventoryModuleWebModelInventoryInfo instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceInventoryModuleWebModelInventoryInfo to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceInventoryModuleWebModelInventoryInfo other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
                ) && 
                (
                    this.FulfillmentCenter == other.FulfillmentCenter ||
                    this.FulfillmentCenter != null &&
                    this.FulfillmentCenter.Equals(other.FulfillmentCenter)
                ) && 
                (
                    this.FulfillmentCenterId == other.FulfillmentCenterId ||
                    this.FulfillmentCenterId != null &&
                    this.FulfillmentCenterId.Equals(other.FulfillmentCenterId)
                ) && 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.InStockQuantity == other.InStockQuantity ||
                    this.InStockQuantity != null &&
                    this.InStockQuantity.Equals(other.InStockQuantity)
                ) && 
                (
                    this.ReservedQuantity == other.ReservedQuantity ||
                    this.ReservedQuantity != null &&
                    this.ReservedQuantity.Equals(other.ReservedQuantity)
                ) && 
                (
                    this.ReorderMinQuantity == other.ReorderMinQuantity ||
                    this.ReorderMinQuantity != null &&
                    this.ReorderMinQuantity.Equals(other.ReorderMinQuantity)
                ) && 
                (
                    this.PreorderQuantity == other.PreorderQuantity ||
                    this.PreorderQuantity != null &&
                    this.PreorderQuantity.Equals(other.PreorderQuantity)
                ) && 
                (
                    this.BackorderQuantity == other.BackorderQuantity ||
                    this.BackorderQuantity != null &&
                    this.BackorderQuantity.Equals(other.BackorderQuantity)
                ) && 
                (
                    this.AllowBackorder == other.AllowBackorder ||
                    this.AllowBackorder != null &&
                    this.AllowBackorder.Equals(other.AllowBackorder)
                ) && 
                (
                    this.AllowPreorder == other.AllowPreorder ||
                    this.AllowPreorder != null &&
                    this.AllowPreorder.Equals(other.AllowPreorder)
                ) && 
                (
                    this.InTransit == other.InTransit ||
                    this.InTransit != null &&
                    this.InTransit.Equals(other.InTransit)
                ) && 
                (
                    this.PreorderAvailabilityDate == other.PreorderAvailabilityDate ||
                    this.PreorderAvailabilityDate != null &&
                    this.PreorderAvailabilityDate.Equals(other.PreorderAvailabilityDate)
                ) && 
                (
                    this.BackorderAvailabilityDate == other.BackorderAvailabilityDate ||
                    this.BackorderAvailabilityDate != null &&
                    this.BackorderAvailabilityDate.Equals(other.BackorderAvailabilityDate)
                ) && 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
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

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.FulfillmentCenter != null)
                    hash = hash * 59 + this.FulfillmentCenter.GetHashCode();

                if (this.FulfillmentCenterId != null)
                    hash = hash * 59 + this.FulfillmentCenterId.GetHashCode();

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.InStockQuantity != null)
                    hash = hash * 59 + this.InStockQuantity.GetHashCode();

                if (this.ReservedQuantity != null)
                    hash = hash * 59 + this.ReservedQuantity.GetHashCode();

                if (this.ReorderMinQuantity != null)
                    hash = hash * 59 + this.ReorderMinQuantity.GetHashCode();

                if (this.PreorderQuantity != null)
                    hash = hash * 59 + this.PreorderQuantity.GetHashCode();

                if (this.BackorderQuantity != null)
                    hash = hash * 59 + this.BackorderQuantity.GetHashCode();

                if (this.AllowBackorder != null)
                    hash = hash * 59 + this.AllowBackorder.GetHashCode();

                if (this.AllowPreorder != null)
                    hash = hash * 59 + this.AllowPreorder.GetHashCode();

                if (this.InTransit != null)
                    hash = hash * 59 + this.InTransit.GetHashCode();

                if (this.PreorderAvailabilityDate != null)
                    hash = hash * 59 + this.PreorderAvailabilityDate.GetHashCode();

                if (this.BackorderAvailabilityDate != null)
                    hash = hash * 59 + this.BackorderAvailabilityDate.GetHashCode();

                if (this.Status != null)
                    hash = hash * 59 + this.Status.GetHashCode();

                return hash;
            }
        }

    }
}
