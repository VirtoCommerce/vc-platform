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
    public partial class VirtoCommerceDomainCustomerModelMember :  IEquatable<VirtoCommerceDomainCustomerModelMember>
    {
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets MemberType
        /// </summary>
        [DataMember(Name="memberType", EmitDefaultValue=false)]
        public string MemberType { get; set; }

        /// <summary>
        /// Gets or Sets Addresses
        /// </summary>
        [DataMember(Name="addresses", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelAddress> Addresses { get; set; }

        /// <summary>
        /// Gets or Sets Phones
        /// </summary>
        [DataMember(Name="phones", EmitDefaultValue=false)]
        public List<string> Phones { get; set; }

        /// <summary>
        /// Gets or Sets Emails
        /// </summary>
        [DataMember(Name="emails", EmitDefaultValue=false)]
        public List<string> Emails { get; set; }

        /// <summary>
        /// Gets or Sets Notes
        /// </summary>
        [DataMember(Name="notes", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCustomerModelNote> Notes { get; set; }

        /// <summary>
        /// Gets or Sets ObjectType
        /// </summary>
        [DataMember(Name="objectType", EmitDefaultValue=false)]
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or Sets DynamicProperties
        /// </summary>
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
            sb.Append("class VirtoCommerceDomainCustomerModelMember {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  MemberType: ").Append(MemberType).Append("\n");
            sb.Append("  Addresses: ").Append(Addresses).Append("\n");
            sb.Append("  Phones: ").Append(Phones).Append("\n");
            sb.Append("  Emails: ").Append(Emails).Append("\n");
            sb.Append("  Notes: ").Append(Notes).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCustomerModelMember);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCustomerModelMember instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCustomerModelMember to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCustomerModelMember other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.MemberType == other.MemberType ||
                    this.MemberType != null &&
                    this.MemberType.Equals(other.MemberType)
                ) && 
                (
                    this.Addresses == other.Addresses ||
                    this.Addresses != null &&
                    this.Addresses.SequenceEqual(other.Addresses)
                ) && 
                (
                    this.Phones == other.Phones ||
                    this.Phones != null &&
                    this.Phones.SequenceEqual(other.Phones)
                ) && 
                (
                    this.Emails == other.Emails ||
                    this.Emails != null &&
                    this.Emails.SequenceEqual(other.Emails)
                ) && 
                (
                    this.Notes == other.Notes ||
                    this.Notes != null &&
                    this.Notes.SequenceEqual(other.Notes)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.MemberType != null)
                    hash = hash * 59 + this.MemberType.GetHashCode();

                if (this.Addresses != null)
                    hash = hash * 59 + this.Addresses.GetHashCode();

                if (this.Phones != null)
                    hash = hash * 59 + this.Phones.GetHashCode();

                if (this.Emails != null)
                    hash = hash * 59 + this.Emails.GetHashCode();

                if (this.Notes != null)
                    hash = hash * 59 + this.Notes.GetHashCode();

                if (this.ObjectType != null)
                    hash = hash * 59 + this.ObjectType.GetHashCode();

                if (this.DynamicProperties != null)
                    hash = hash * 59 + this.DynamicProperties.GetHashCode();

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
