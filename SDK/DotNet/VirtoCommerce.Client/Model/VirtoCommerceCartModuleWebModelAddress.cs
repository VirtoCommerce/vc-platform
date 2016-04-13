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
    public partial class VirtoCommerceCartModuleWebModelAddress :  IEquatable<VirtoCommerceCartModuleWebModelAddress>
    {
        /// <summary>
        /// Gets or sets the value of address type
        /// </summary>
        /// <value>Gets or sets the value of address type</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value of organization name
        /// </summary>
        /// <value>Gets or sets the value of organization name</value>
        [DataMember(Name="organization", EmitDefaultValue=false)]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the value of country code
        /// </summary>
        /// <value>Gets or sets the value of country code</value>
        [DataMember(Name="countryCode", EmitDefaultValue=false)]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the value of country name
        /// </summary>
        /// <value>Gets or sets the value of country name</value>
        [DataMember(Name="countryName", EmitDefaultValue=false)]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the value of city name
        /// </summary>
        /// <value>Gets or sets the value of city name</value>
        [DataMember(Name="city", EmitDefaultValue=false)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the value of postal code
        /// </summary>
        /// <value>Gets or sets the value of postal code</value>
        [DataMember(Name="postalCode", EmitDefaultValue=false)]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the value of zip code
        /// </summary>
        /// <value>Gets or sets the value of zip code</value>
        [DataMember(Name="zip", EmitDefaultValue=false)]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the value of address line 1
        /// </summary>
        /// <value>Gets or sets the value of address line 1</value>
        [DataMember(Name="line1", EmitDefaultValue=false)]
        public string Line1 { get; set; }

        /// <summary>
        /// Gets or sets the value of address line 2
        /// </summary>
        /// <value>Gets or sets the value of address line 2</value>
        [DataMember(Name="line2", EmitDefaultValue=false)]
        public string Line2 { get; set; }

        /// <summary>
        /// Gets or sets the value of region code
        /// </summary>
        /// <value>Gets or sets the value of region code</value>
        [DataMember(Name="regionId", EmitDefaultValue=false)]
        public string RegionId { get; set; }

        /// <summary>
        /// Gets or sets the value of region name
        /// </summary>
        /// <value>Gets or sets the value of region name</value>
        [DataMember(Name="regionName", EmitDefaultValue=false)]
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the value of first name
        /// </summary>
        /// <value>Gets or sets the value of first name</value>
        [DataMember(Name="firstName", EmitDefaultValue=false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the value of middle name
        /// </summary>
        /// <value>Gets or sets the value of middle name</value>
        [DataMember(Name="middleName", EmitDefaultValue=false)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the value of last name
        /// </summary>
        /// <value>Gets or sets the value of last name</value>
        [DataMember(Name="lastName", EmitDefaultValue=false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the value of phone number
        /// </summary>
        /// <value>Gets or sets the value of phone number</value>
        [DataMember(Name="phone", EmitDefaultValue=false)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the value of E-mail address
        /// </summary>
        /// <value>Gets or sets the value of E-mail address</value>
        [DataMember(Name="email", EmitDefaultValue=false)]
        public string Email { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCartModuleWebModelAddress {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Organization: ").Append(Organization).Append("\n");
            sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
            sb.Append("  CountryName: ").Append(CountryName).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
            sb.Append("  Zip: ").Append(Zip).Append("\n");
            sb.Append("  Line1: ").Append(Line1).Append("\n");
            sb.Append("  Line2: ").Append(Line2).Append("\n");
            sb.Append("  RegionId: ").Append(RegionId).Append("\n");
            sb.Append("  RegionName: ").Append(RegionName).Append("\n");
            sb.Append("  FirstName: ").Append(FirstName).Append("\n");
            sb.Append("  MiddleName: ").Append(MiddleName).Append("\n");
            sb.Append("  LastName: ").Append(LastName).Append("\n");
            sb.Append("  Phone: ").Append(Phone).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelAddress);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelAddress instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelAddress to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelAddress other)
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
                    this.Organization == other.Organization ||
                    this.Organization != null &&
                    this.Organization.Equals(other.Organization)
                ) && 
                (
                    this.CountryCode == other.CountryCode ||
                    this.CountryCode != null &&
                    this.CountryCode.Equals(other.CountryCode)
                ) && 
                (
                    this.CountryName == other.CountryName ||
                    this.CountryName != null &&
                    this.CountryName.Equals(other.CountryName)
                ) && 
                (
                    this.City == other.City ||
                    this.City != null &&
                    this.City.Equals(other.City)
                ) && 
                (
                    this.PostalCode == other.PostalCode ||
                    this.PostalCode != null &&
                    this.PostalCode.Equals(other.PostalCode)
                ) && 
                (
                    this.Zip == other.Zip ||
                    this.Zip != null &&
                    this.Zip.Equals(other.Zip)
                ) && 
                (
                    this.Line1 == other.Line1 ||
                    this.Line1 != null &&
                    this.Line1.Equals(other.Line1)
                ) && 
                (
                    this.Line2 == other.Line2 ||
                    this.Line2 != null &&
                    this.Line2.Equals(other.Line2)
                ) && 
                (
                    this.RegionId == other.RegionId ||
                    this.RegionId != null &&
                    this.RegionId.Equals(other.RegionId)
                ) && 
                (
                    this.RegionName == other.RegionName ||
                    this.RegionName != null &&
                    this.RegionName.Equals(other.RegionName)
                ) && 
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
                    this.Phone == other.Phone ||
                    this.Phone != null &&
                    this.Phone.Equals(other.Phone)
                ) && 
                (
                    this.Email == other.Email ||
                    this.Email != null &&
                    this.Email.Equals(other.Email)
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

                if (this.Organization != null)
                    hash = hash * 59 + this.Organization.GetHashCode();

                if (this.CountryCode != null)
                    hash = hash * 59 + this.CountryCode.GetHashCode();

                if (this.CountryName != null)
                    hash = hash * 59 + this.CountryName.GetHashCode();

                if (this.City != null)
                    hash = hash * 59 + this.City.GetHashCode();

                if (this.PostalCode != null)
                    hash = hash * 59 + this.PostalCode.GetHashCode();

                if (this.Zip != null)
                    hash = hash * 59 + this.Zip.GetHashCode();

                if (this.Line1 != null)
                    hash = hash * 59 + this.Line1.GetHashCode();

                if (this.Line2 != null)
                    hash = hash * 59 + this.Line2.GetHashCode();

                if (this.RegionId != null)
                    hash = hash * 59 + this.RegionId.GetHashCode();

                if (this.RegionName != null)
                    hash = hash * 59 + this.RegionName.GetHashCode();

                if (this.FirstName != null)
                    hash = hash * 59 + this.FirstName.GetHashCode();

                if (this.MiddleName != null)
                    hash = hash * 59 + this.MiddleName.GetHashCode();

                if (this.LastName != null)
                    hash = hash * 59 + this.LastName.GetHashCode();

                if (this.Phone != null)
                    hash = hash * 59 + this.Phone.GetHashCode();

                if (this.Email != null)
                    hash = hash * 59 + this.Email.GetHashCode();

                return hash;
            }
        }

    }
}
