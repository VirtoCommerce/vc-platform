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
    /// Represent base class for all order module documents (operations)\r\n            contains shared set of properties
    /// </summary>
    [DataContract]
    public class VirtoCommerceOrderModuleWebModelOperation : IEquatable<VirtoCommerceOrderModuleWebModelOperation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceOrderModuleWebModelOperation" /> class.
        /// </summary>
        public VirtoCommerceOrderModuleWebModelOperation()
        {
            
        }

        
        /// <summary>
        /// Operation type string representation (CustomerOrder, Shipment etc)
        /// </summary>
        /// <value>Operation type string representation (CustomerOrder, Shipment etc)</value>
        [DataMember(Name="operationType", EmitDefaultValue=false)]
        public string OperationType { get; set; }
  
        
        /// <summary>
        /// Unique user friendly document number (generate automatically based on special algorithm realization)
        /// </summary>
        /// <value>Unique user friendly document number (generate automatically based on special algorithm realization)</value>
        [DataMember(Name="number", EmitDefaultValue=false)]
        public string Number { get; set; }
  
        
        /// <summary>
        /// Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)
        /// </summary>
        /// <value>Flag can be used to refer to a specific order status in a variety of user scenarios with combination of Status\r\n            (Order completion, Shipment send etc)</value>
        [DataMember(Name="isApproved", EmitDefaultValue=false)]
        public bool? IsApproved { get; set; }
  
        
        /// <summary>
        /// Current operation status may have any values defined by concrete business process
        /// </summary>
        /// <value>Current operation status may have any values defined by concrete business process</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Comment
        /// </summary>
        [DataMember(Name="comment", EmitDefaultValue=false)]
        public string Comment { get; set; }
  
        
        /// <summary>
        /// Currency code
        /// </summary>
        /// <value>Currency code</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }
  
        
        /// <summary>
        /// Gets or Sets TaxIncluded
        /// </summary>
        [DataMember(Name="taxIncluded", EmitDefaultValue=false)]
        public bool? TaxIncluded { get; set; }
  
        
        /// <summary>
        /// Money amount without tax
        /// </summary>
        /// <value>Money amount without tax</value>
        [DataMember(Name="sum", EmitDefaultValue=false)]
        public double? Sum { get; set; }
  
        
        /// <summary>
        /// Tax total
        /// </summary>
        /// <value>Tax total</value>
        [DataMember(Name="tax", EmitDefaultValue=false)]
        public double? Tax { get; set; }
  
        
        /// <summary>
        /// Gets or Sets IsCancelled
        /// </summary>
        [DataMember(Name="isCancelled", EmitDefaultValue=false)]
        public bool? IsCancelled { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CancelledDate
        /// </summary>
        [DataMember(Name="cancelledDate", EmitDefaultValue=false)]
        public DateTime? CancelledDate { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CancelReason
        /// </summary>
        [DataMember(Name="cancelReason", EmitDefaultValue=false)]
        public string CancelReason { get; set; }
  
        
        /// <summary>
        /// Used for construct hierarchy of operation and represent parent operation id
        /// </summary>
        /// <value>Used for construct hierarchy of operation and represent parent operation id</value>
        [DataMember(Name="parentOperationId", EmitDefaultValue=false)]
        public string ParentOperationId { get; set; }
  
        
        /// <summary>
        /// Gets or Sets ChildrenOperations
        /// </summary>
        [DataMember(Name="childrenOperations", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelOperation> ChildrenOperations { get; set; }
  
        
        /// <summary>
        /// Used for dynamic properties management, contains object type string
        /// </summary>
        /// <value>Used for dynamic properties management, contains object type string</value>
        [DataMember(Name="objectType", EmitDefaultValue=false)]
        public string ObjectType { get; set; }
  
        
        /// <summary>
        /// Dynamic properties collections
        /// </summary>
        /// <value>Dynamic properties collections</value>
        [DataMember(Name="dynamicProperties", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreDynamicPropertiesDynamicObjectProperty> DynamicProperties { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }
  
        
        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }
  
        
        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }
  
        
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
            sb.Append("class VirtoCommerceOrderModuleWebModelOperation {\n");
            sb.Append("  OperationType: ").Append(OperationType).Append("\n");
            sb.Append("  Number: ").Append(Number).Append("\n");
            sb.Append("  IsApproved: ").Append(IsApproved).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  TaxIncluded: ").Append(TaxIncluded).Append("\n");
            sb.Append("  Sum: ").Append(Sum).Append("\n");
            sb.Append("  Tax: ").Append(Tax).Append("\n");
            sb.Append("  IsCancelled: ").Append(IsCancelled).Append("\n");
            sb.Append("  CancelledDate: ").Append(CancelledDate).Append("\n");
            sb.Append("  CancelReason: ").Append(CancelReason).Append("\n");
            sb.Append("  ParentOperationId: ").Append(ParentOperationId).Append("\n");
            sb.Append("  ChildrenOperations: ").Append(ChildrenOperations).Append("\n");
            sb.Append("  ObjectType: ").Append(ObjectType).Append("\n");
            sb.Append("  DynamicProperties: ").Append(DynamicProperties).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelOperation);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelOperation instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceOrderModuleWebModelOperation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelOperation other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.OperationType == other.OperationType ||
                    this.OperationType != null &&
                    this.OperationType.Equals(other.OperationType)
                ) && 
                (
                    this.Number == other.Number ||
                    this.Number != null &&
                    this.Number.Equals(other.Number)
                ) && 
                (
                    this.IsApproved == other.IsApproved ||
                    this.IsApproved != null &&
                    this.IsApproved.Equals(other.IsApproved)
                ) && 
                (
                    this.Status == other.Status ||
                    this.Status != null &&
                    this.Status.Equals(other.Status)
                ) && 
                (
                    this.Comment == other.Comment ||
                    this.Comment != null &&
                    this.Comment.Equals(other.Comment)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.TaxIncluded == other.TaxIncluded ||
                    this.TaxIncluded != null &&
                    this.TaxIncluded.Equals(other.TaxIncluded)
                ) && 
                (
                    this.Sum == other.Sum ||
                    this.Sum != null &&
                    this.Sum.Equals(other.Sum)
                ) && 
                (
                    this.Tax == other.Tax ||
                    this.Tax != null &&
                    this.Tax.Equals(other.Tax)
                ) && 
                (
                    this.IsCancelled == other.IsCancelled ||
                    this.IsCancelled != null &&
                    this.IsCancelled.Equals(other.IsCancelled)
                ) && 
                (
                    this.CancelledDate == other.CancelledDate ||
                    this.CancelledDate != null &&
                    this.CancelledDate.Equals(other.CancelledDate)
                ) && 
                (
                    this.CancelReason == other.CancelReason ||
                    this.CancelReason != null &&
                    this.CancelReason.Equals(other.CancelReason)
                ) && 
                (
                    this.ParentOperationId == other.ParentOperationId ||
                    this.ParentOperationId != null &&
                    this.ParentOperationId.Equals(other.ParentOperationId)
                ) && 
                (
                    this.ChildrenOperations == other.ChildrenOperations ||
                    this.ChildrenOperations != null &&
                    this.ChildrenOperations.SequenceEqual(other.ChildrenOperations)
                ) && 
                (
                    this.ObjectType == other.ObjectType ||
                    this.ObjectType != null &&
                    this.ObjectType.Equals(other.ObjectType)
                ) && 
                (
                    this.DynamicProperties == other.DynamicProperties ||
                    this.DynamicProperties != null &&
                    this.DynamicProperties.SequenceEqual(other.DynamicProperties)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
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
                
                if (this.OperationType != null)
                    hash = hash * 57 + this.OperationType.GetHashCode();
                
                if (this.Number != null)
                    hash = hash * 57 + this.Number.GetHashCode();
                
                if (this.IsApproved != null)
                    hash = hash * 57 + this.IsApproved.GetHashCode();
                
                if (this.Status != null)
                    hash = hash * 57 + this.Status.GetHashCode();
                
                if (this.Comment != null)
                    hash = hash * 57 + this.Comment.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 57 + this.Currency.GetHashCode();
                
                if (this.TaxIncluded != null)
                    hash = hash * 57 + this.TaxIncluded.GetHashCode();
                
                if (this.Sum != null)
                    hash = hash * 57 + this.Sum.GetHashCode();
                
                if (this.Tax != null)
                    hash = hash * 57 + this.Tax.GetHashCode();
                
                if (this.IsCancelled != null)
                    hash = hash * 57 + this.IsCancelled.GetHashCode();
                
                if (this.CancelledDate != null)
                    hash = hash * 57 + this.CancelledDate.GetHashCode();
                
                if (this.CancelReason != null)
                    hash = hash * 57 + this.CancelReason.GetHashCode();
                
                if (this.ParentOperationId != null)
                    hash = hash * 57 + this.ParentOperationId.GetHashCode();
                
                if (this.ChildrenOperations != null)
                    hash = hash * 57 + this.ChildrenOperations.GetHashCode();
                
                if (this.ObjectType != null)
                    hash = hash * 57 + this.ObjectType.GetHashCode();
                
                if (this.DynamicProperties != null)
                    hash = hash * 57 + this.DynamicProperties.GetHashCode();
                
                if (this.CreatedDate != null)
                    hash = hash * 57 + this.CreatedDate.GetHashCode();
                
                if (this.ModifiedDate != null)
                    hash = hash * 57 + this.ModifiedDate.GetHashCode();
                
                if (this.CreatedBy != null)
                    hash = hash * 57 + this.CreatedBy.GetHashCode();
                
                if (this.ModifiedBy != null)
                    hash = hash * 57 + this.ModifiedBy.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                return hash;
            }
        }

    }


}
