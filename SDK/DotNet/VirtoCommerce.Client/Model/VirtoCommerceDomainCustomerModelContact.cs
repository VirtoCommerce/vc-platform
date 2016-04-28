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
    public partial class VirtoCommerceDomainCustomerModelContact :  IEquatable<VirtoCommerceDomainCustomerModelContact>
    {
        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [DataMember(Name="firstName", EmitDefaultValue=false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets MiddleName
        /// </summary>
        [DataMember(Name="middleName", EmitDefaultValue=false)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [DataMember(Name="lastName", EmitDefaultValue=false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets FullName
        /// </summary>
        [DataMember(Name="fullName", EmitDefaultValue=false)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or Sets TimeZone
        /// </summary>
        [DataMember(Name="timeZone", EmitDefaultValue=false)]
        public string TimeZone { get; set; }

        /// <summary>
        /// Gets or Sets DefaultLanguage
        /// </summary>
        [DataMember(Name="defaultLanguage", EmitDefaultValue=false)]
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Gets or Sets BirthDate
        /// </summary>
        [DataMember(Name="birthDate", EmitDefaultValue=false)]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or Sets TaxpayerId
        /// </summary>
        [DataMember(Name="taxpayerId", EmitDefaultValue=false)]
        public string TaxpayerId { get; set; }

        /// <summary>
        /// Gets or Sets PreferredDelivery
        /// </summary>
        [DataMember(Name="preferredDelivery", EmitDefaultValue=false)]
        public string PreferredDelivery { get; set; }

        /// <summary>
        /// Gets or Sets PreferredCommunication
        /// </summary>
        [DataMember(Name="preferredCommunication", EmitDefaultValue=false)]
        public string PreferredCommunication { get; set; }

        /// <summary>
        /// Gets or Sets Salutation
        /// </summary>
        [DataMember(Name="salutation", EmitDefaultValue=false)]
        public string Salutation { get; set; }

        /// <summary>
        /// Gets or Sets Organizations
        /// </summary>
        [DataMember(Name="organizations", EmitDefaultValue=false)]
        public List<string> Organizations { get; set; }

        /// <summary>
        /// Gets or Sets SecurityAccounts
        /// </summary>
        [DataMember(Name="securityAccounts", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreSecurityApplicationUserExtended> SecurityAccounts { get; set; }

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
            sb.Append("class VirtoCommerceDomainCustomerModelContact {\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  MiddleName: ").Append(MiddleName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("  FullName: ").Append(FullName).Append("\n");
            sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
            sb.Append("  DefaultLanguage: ").Append(DefaultLanguage).Append("\n");
            sb.Append("  BirthDate: ").Append(BirthDate).Append("\n");
            sb.Append("  TaxpayerId: ").Append(TaxpayerId).Append("\n");
            sb.Append("  PreferredDelivery: ").Append(PreferredDelivery).Append("\n");
            sb.Append("  PreferredCommunication: ").Append(PreferredCommunication).Append("\n");
            sb.Append("  Salutation: ").Append(Salutation).Append("\n");
            sb.Append("  Organizations: ").Append(Organizations).Append("\n");
            sb.Append("  SecurityAccounts: ").Append(SecurityAccounts).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCustomerModelContact);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCustomerModelContact instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCustomerModelContact to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCustomerModelContact other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.FirstName == other.FirstName ||
                    this.FirstName != null &&
                    this.FirstName.Equals(other.FirstName)
                ) && 
                (
                    this.MiddleName == other.MiddleName ||
                    this.MiddleName != null &&
                    this.MiddleName.Equals(other.MiddleName)
                ) && 
                (
                    this.LastName == other.LastName ||
                    this.LastName != null &&
                    this.LastName.Equals(other.LastName)
                ) && 
                (
                    this.FullName == other.FullName ||
                    this.FullName != null &&
                    this.FullName.Equals(other.FullName)
                ) && 
                (
                    this.TimeZone == other.TimeZone ||
                    this.TimeZone != null &&
                    this.TimeZone.Equals(other.TimeZone)
                ) && 
                (
                    this.DefaultLanguage == other.DefaultLanguage ||
                    this.DefaultLanguage != null &&
                    this.DefaultLanguage.Equals(other.DefaultLanguage)
                ) && 
                (
                    this.BirthDate == other.BirthDate ||
                    this.BirthDate != null &&
                    this.BirthDate.Equals(other.BirthDate)
                ) && 
                (
                    this.TaxpayerId == other.TaxpayerId ||
                    this.TaxpayerId != null &&
                    this.TaxpayerId.Equals(other.TaxpayerId)
                ) && 
                (
                    this.PreferredDelivery == other.PreferredDelivery ||
                    this.PreferredDelivery != null &&
                    this.PreferredDelivery.Equals(other.PreferredDelivery)
                ) && 
                (
                    this.PreferredCommunication == other.PreferredCommunication ||
                    this.PreferredCommunication != null &&
                    this.PreferredCommunication.Equals(other.PreferredCommunication)
                ) && 
                (
                    this.Salutation == other.Salutation ||
                    this.Salutation != null &&
                    this.Salutation.Equals(other.Salutation)
                ) && 
                (
                    this.Organizations == other.Organizations ||
                    this.Organizations != null &&
                    this.Organizations.SequenceEqual(other.Organizations)
                ) && 
                (
                    this.SecurityAccounts == other.SecurityAccounts ||
                    this.SecurityAccounts != null &&
                    this.SecurityAccounts.SequenceEqual(other.SecurityAccounts)
                ) && 
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

                if (this.FirstName != null)
                    hash = hash * 59 + this.FirstName.GetHashCode();

                if (this.MiddleName != null)
                    hash = hash * 59 + this.MiddleName.GetHashCode();

                if (this.LastName != null)
                    hash = hash * 59 + this.LastName.GetHashCode();

                if (this.FullName != null)
                    hash = hash * 59 + this.FullName.GetHashCode();

                if (this.TimeZone != null)
                    hash = hash * 59 + this.TimeZone.GetHashCode();

                if (this.DefaultLanguage != null)
                    hash = hash * 59 + this.DefaultLanguage.GetHashCode();

                if (this.BirthDate != null)
                    hash = hash * 59 + this.BirthDate.GetHashCode();

                if (this.TaxpayerId != null)
                    hash = hash * 59 + this.TaxpayerId.GetHashCode();

                if (this.PreferredDelivery != null)
                    hash = hash * 59 + this.PreferredDelivery.GetHashCode();

                if (this.PreferredCommunication != null)
                    hash = hash * 59 + this.PreferredCommunication.GetHashCode();

                if (this.Salutation != null)
                    hash = hash * 59 + this.Salutation.GetHashCode();

                if (this.Organizations != null)
                    hash = hash * 59 + this.Organizations.GetHashCode();

                if (this.SecurityAccounts != null)
                    hash = hash * 59 + this.SecurityAccounts.GetHashCode();

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
