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
    /// Represent marketing promotion, define applicable rules and rewards amount in marketing system
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceMarketingModuleWebModelPromotion :  IEquatable<VirtoCommerceMarketingModuleWebModelPromotion>
    {
        /// <summary>
        /// It contains the name of realizing this type promotion.\r\n            DynamicPromotion is build in implementation allow to construct promotion with dynamic conditions and rewards.\r\n            For complex custom scenarios user may define personal &#39;hard-coded&#39; promotion types
        /// </summary>
        /// <value>It contains the name of realizing this type promotion.\r\n            DynamicPromotion is build in implementation allow to construct promotion with dynamic conditions and rewards.\r\n            For complex custom scenarios user may define personal &#39;hard-coded&#39; promotion types</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Store id that is covered by this promotion
        /// </summary>
        /// <value>Store id that is covered by this promotion</value>
        [DataMember(Name="store", EmitDefaultValue=false)]
        public string Store { get; set; }

        /// <summary>
        /// Catalog id that is covered by this promotion
        /// </summary>
        /// <value>Catalog id that is covered by this promotion</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Maximum promotion usage count
        /// </summary>
        /// <value>Maximum promotion usage count</value>
        [DataMember(Name="maxUsageCount", EmitDefaultValue=false)]
        public int? MaxUsageCount { get; set; }

        /// <summary>
        /// Gets or Sets MaxPersonalUsageCount
        /// </summary>
        [DataMember(Name="maxPersonalUsageCount", EmitDefaultValue=false)]
        public int? MaxPersonalUsageCount { get; set; }

        /// <summary>
        /// List of coupons codes which may be used for activate that promotion
        /// </summary>
        /// <value>List of coupons codes which may be used for activate that promotion</value>
        [DataMember(Name="coupons", EmitDefaultValue=false)]
        public List<string> Coupons { get; set; }

        /// <summary>
        /// Used for choosing in combination
        /// </summary>
        /// <value>Used for choosing in combination</value>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name="endDate", EmitDefaultValue=false)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Dynamic conditions tree determine the applicability of this promotion and reward definition
        /// </summary>
        /// <value>Dynamic conditions tree determine the applicability of this promotion and reward definition</value>
        [DataMember(Name="dynamicExpression", EmitDefaultValue=false)]
        public VirtoCommerceDomainMarketingModelPromoDynamicExpressionTree DynamicExpression { get; set; }

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
            sb.Append("class VirtoCommerceMarketingModuleWebModelPromotion {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Store: ").Append(Store).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  MaxUsageCount: ").Append(MaxUsageCount).Append("\n");
            sb.Append("  MaxPersonalUsageCount: ").Append(MaxPersonalUsageCount).Append("\n");
            sb.Append("  Coupons: ").Append(Coupons).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  DynamicExpression: ").Append(DynamicExpression).Append("\n");
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
            return this.Equals(obj as VirtoCommerceMarketingModuleWebModelPromotion);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMarketingModuleWebModelPromotion instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceMarketingModuleWebModelPromotion to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMarketingModuleWebModelPromotion other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Store == other.Store ||
                    this.Store != null &&
                    this.Store.Equals(other.Store)
                ) && 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.MaxUsageCount == other.MaxUsageCount ||
                    this.MaxUsageCount != null &&
                    this.MaxUsageCount.Equals(other.MaxUsageCount)
                ) && 
                (
                    this.MaxPersonalUsageCount == other.MaxPersonalUsageCount ||
                    this.MaxPersonalUsageCount != null &&
                    this.MaxPersonalUsageCount.Equals(other.MaxPersonalUsageCount)
                ) && 
                (
                    this.Coupons == other.Coupons ||
                    this.Coupons != null &&
                    this.Coupons.SequenceEqual(other.Coupons)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) && 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
                ) && 
                (
                    this.DynamicExpression == other.DynamicExpression ||
                    this.DynamicExpression != null &&
                    this.DynamicExpression.Equals(other.DynamicExpression)
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

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Store != null)
                    hash = hash * 59 + this.Store.GetHashCode();

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.MaxUsageCount != null)
                    hash = hash * 59 + this.MaxUsageCount.GetHashCode();

                if (this.MaxPersonalUsageCount != null)
                    hash = hash * 59 + this.MaxPersonalUsageCount.GetHashCode();

                if (this.Coupons != null)
                    hash = hash * 59 + this.Coupons.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.StartDate != null)
                    hash = hash * 59 + this.StartDate.GetHashCode();

                if (this.EndDate != null)
                    hash = hash * 59 + this.EndDate.GetHashCode();

                if (this.DynamicExpression != null)
                    hash = hash * 59 + this.DynamicExpression.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
