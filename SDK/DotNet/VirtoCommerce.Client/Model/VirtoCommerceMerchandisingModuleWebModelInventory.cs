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
    public class VirtoCommerceMerchandisingModuleWebModelInventory : IEquatable<VirtoCommerceMerchandisingModuleWebModelInventory>
    {
        
        /// <summary>
        /// Gets or sets the value of fulfillment canter id
        /// </summary>
        /// <value>Gets or sets the value of fulfillment canter id</value>
        [DataMember(Name="fulfillmentCenterId", EmitDefaultValue=false)]
        public string FulfillmentCenterId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of inventory quantity in stock
        /// </summary>
        /// <value>Gets or sets the value of inventory quantity in stock</value>
        [DataMember(Name="inStockQuantity", EmitDefaultValue=false)]
        public long? InStockQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of reserved inventory quantity
        /// </summary>
        /// <value>Gets or sets the value of reserved inventory quantity</value>
        [DataMember(Name="reservedQuantity", EmitDefaultValue=false)]
        public long? ReservedQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of reorder inventory minimum quanitity
        /// </summary>
        /// <value>Gets or sets the value of reorder inventory minimum quanitity</value>
        [DataMember(Name="reorderMinQuantity", EmitDefaultValue=false)]
        public long? ReorderMinQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of preorder inventory quantity
        /// </summary>
        /// <value>Gets or sets the value of preorder inventory quantity</value>
        [DataMember(Name="preorderQuantity", EmitDefaultValue=false)]
        public long? PreorderQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of backorder inventory quantity
        /// </summary>
        /// <value>Gets or sets the value of backorder inventory quantity</value>
        [DataMember(Name="backorderQuantity", EmitDefaultValue=false)]
        public long? BackorderQuantity { get; set; }
  
        
        /// <summary>
        /// Gets or sets the flag of backorder is allowed
        /// </summary>
        /// <value>Gets or sets the flag of backorder is allowed</value>
        [DataMember(Name="allowBackorder", EmitDefaultValue=false)]
        public bool? AllowBackorder { get; set; }
  
        
        /// <summary>
        /// Gets or sets the flag of preorder is allowed
        /// </summary>
        /// <value>Gets or sets the flag of preorder is allowed</value>
        [DataMember(Name="allowPreorder", EmitDefaultValue=false)]
        public bool? AllowPreorder { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value for inventory quantity in transit
        /// </summary>
        /// <value>Gets or sets the value for inventory quantity in transit</value>
        [DataMember(Name="inTransit", EmitDefaultValue=false)]
        public long? InTransit { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of date/time limit for preorder availability
        /// </summary>
        /// <value>Gets or sets the value of date/time limit for preorder availability</value>
        [DataMember(Name="preorderAvailabilityDate", EmitDefaultValue=false)]
        public DateTime? PreorderAvailabilityDate { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of date/time limit for backorder availability
        /// </summary>
        /// <value>Gets or sets the value of date/time limit for backorder availability</value>
        [DataMember(Name="backorderAvailabilityDate", EmitDefaultValue=false)]
        public DateTime? BackorderAvailabilityDate { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of inventory status
        /// </summary>
        /// <value>Gets or sets the value of inventory status</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public int? Status { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelInventory {\n");
            sb.Append("  FulfillmentCenterId: ").Append(FulfillmentCenterId).Append("\n");
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelInventory);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelInventory instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelInventory to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelInventory other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.FulfillmentCenterId == other.FulfillmentCenterId ||
                    this.FulfillmentCenterId != null &&
                    this.FulfillmentCenterId.Equals(other.FulfillmentCenterId)
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
                
                if (this.FulfillmentCenterId != null)
                    hash = hash * 57 + this.FulfillmentCenterId.GetHashCode();
                
                if (this.InStockQuantity != null)
                    hash = hash * 57 + this.InStockQuantity.GetHashCode();
                
                if (this.ReservedQuantity != null)
                    hash = hash * 57 + this.ReservedQuantity.GetHashCode();
                
                if (this.ReorderMinQuantity != null)
                    hash = hash * 57 + this.ReorderMinQuantity.GetHashCode();
                
                if (this.PreorderQuantity != null)
                    hash = hash * 57 + this.PreorderQuantity.GetHashCode();
                
                if (this.BackorderQuantity != null)
                    hash = hash * 57 + this.BackorderQuantity.GetHashCode();
                
                if (this.AllowBackorder != null)
                    hash = hash * 57 + this.AllowBackorder.GetHashCode();
                
                if (this.AllowPreorder != null)
                    hash = hash * 57 + this.AllowPreorder.GetHashCode();
                
                if (this.InTransit != null)
                    hash = hash * 57 + this.InTransit.GetHashCode();
                
                if (this.PreorderAvailabilityDate != null)
                    hash = hash * 57 + this.PreorderAvailabilityDate.GetHashCode();
                
                if (this.BackorderAvailabilityDate != null)
                    hash = hash * 57 + this.BackorderAvailabilityDate.GetHashCode();
                
                if (this.Status != null)
                    hash = hash * 57 + this.Status.GetHashCode();
                
                return hash;
            }
        }

    }


}
