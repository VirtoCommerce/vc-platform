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
    /// Represent fulfillment center information
    /// </summary>
    [DataContract]
    public class VirtoCommerceCoreModuleWebModelFulfillmentCenter : IEquatable<VirtoCommerceCoreModuleWebModelFulfillmentCenter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceCoreModuleWebModelFulfillmentCenter" /> class.
        /// </summary>
        public VirtoCommerceCoreModuleWebModelFulfillmentCenter()
        {
            
        }

        
        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }
  
        
        /// <summary>
        /// Gets or Sets MaxReleasesPerPickBatch
        /// </summary>
        [DataMember(Name="maxReleasesPerPickBatch", EmitDefaultValue=false)]
        public int? MaxReleasesPerPickBatch { get; set; }
  
        
        /// <summary>
        /// Gets or Sets PickDelay
        /// </summary>
        [DataMember(Name="pickDelay", EmitDefaultValue=false)]
        public int? PickDelay { get; set; }
  
        
        /// <summary>
        /// Gets or Sets DaytimePhoneNumber
        /// </summary>
        [DataMember(Name="daytimePhoneNumber", EmitDefaultValue=false)]
        public string DaytimePhoneNumber { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Line1
        /// </summary>
        [DataMember(Name="line1", EmitDefaultValue=false)]
        public string Line1 { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Line2
        /// </summary>
        [DataMember(Name="line2", EmitDefaultValue=false)]
        public string Line2 { get; set; }
  
        
        /// <summary>
        /// Gets or Sets City
        /// </summary>
        [DataMember(Name="city", EmitDefaultValue=false)]
        public string City { get; set; }
  
        
        /// <summary>
        /// Gets or Sets StateProvince
        /// </summary>
        [DataMember(Name="stateProvince", EmitDefaultValue=false)]
        public string StateProvince { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CountryCode
        /// </summary>
        [DataMember(Name="countryCode", EmitDefaultValue=false)]
        public string CountryCode { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CountryName
        /// </summary>
        [DataMember(Name="countryName", EmitDefaultValue=false)]
        public string CountryName { get; set; }
  
        
        /// <summary>
        /// Gets or Sets PostalCode
        /// </summary>
        [DataMember(Name="postalCode", EmitDefaultValue=false)]
        public string PostalCode { get; set; }
  
        
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
            sb.Append("class VirtoCommerceCoreModuleWebModelFulfillmentCenter {\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  MaxReleasesPerPickBatch: ").Append(MaxReleasesPerPickBatch).Append("\n");
            sb.Append("  PickDelay: ").Append(PickDelay).Append("\n");
            sb.Append("  DaytimePhoneNumber: ").Append(DaytimePhoneNumber).Append("\n");
            sb.Append("  Line1: ").Append(Line1).Append("\n");
            sb.Append("  Line2: ").Append(Line2).Append("\n");
            sb.Append("  City: ").Append(City).Append("\n");
            sb.Append("  StateProvince: ").Append(StateProvince).Append("\n");
            sb.Append("  CountryCode: ").Append(CountryCode).Append("\n");
            sb.Append("  CountryName: ").Append(CountryName).Append("\n");
            sb.Append("  PostalCode: ").Append(PostalCode).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCoreModuleWebModelFulfillmentCenter);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCoreModuleWebModelFulfillmentCenter instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceCoreModuleWebModelFulfillmentCenter to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCoreModuleWebModelFulfillmentCenter other)
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
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.MaxReleasesPerPickBatch == other.MaxReleasesPerPickBatch ||
                    this.MaxReleasesPerPickBatch != null &&
                    this.MaxReleasesPerPickBatch.Equals(other.MaxReleasesPerPickBatch)
                ) && 
                (
                    this.PickDelay == other.PickDelay ||
                    this.PickDelay != null &&
                    this.PickDelay.Equals(other.PickDelay)
                ) && 
                (
                    this.DaytimePhoneNumber == other.DaytimePhoneNumber ||
                    this.DaytimePhoneNumber != null &&
                    this.DaytimePhoneNumber.Equals(other.DaytimePhoneNumber)
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
                    this.City == other.City ||
                    this.City != null &&
                    this.City.Equals(other.City)
                ) && 
                (
                    this.StateProvince == other.StateProvince ||
                    this.StateProvince != null &&
                    this.StateProvince.Equals(other.StateProvince)
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
                    this.PostalCode == other.PostalCode ||
                    this.PostalCode != null &&
                    this.PostalCode.Equals(other.PostalCode)
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
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Description != null)
                    hash = hash * 57 + this.Description.GetHashCode();
                
                if (this.MaxReleasesPerPickBatch != null)
                    hash = hash * 57 + this.MaxReleasesPerPickBatch.GetHashCode();
                
                if (this.PickDelay != null)
                    hash = hash * 57 + this.PickDelay.GetHashCode();
                
                if (this.DaytimePhoneNumber != null)
                    hash = hash * 57 + this.DaytimePhoneNumber.GetHashCode();
                
                if (this.Line1 != null)
                    hash = hash * 57 + this.Line1.GetHashCode();
                
                if (this.Line2 != null)
                    hash = hash * 57 + this.Line2.GetHashCode();
                
                if (this.City != null)
                    hash = hash * 57 + this.City.GetHashCode();
                
                if (this.StateProvince != null)
                    hash = hash * 57 + this.StateProvince.GetHashCode();
                
                if (this.CountryCode != null)
                    hash = hash * 57 + this.CountryCode.GetHashCode();
                
                if (this.CountryName != null)
                    hash = hash * 57 + this.CountryName.GetHashCode();
                
                if (this.PostalCode != null)
                    hash = hash * 57 + this.PostalCode.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                return hash;
            }
        }

    }


}
