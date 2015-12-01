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
    public class VirtoCommerceMerchandisingModuleWebModelDynamicContentItem : IEquatable<VirtoCommerceMerchandisingModuleWebModelDynamicContentItem>
    {
        
        /// <summary>
        /// Gets or sets the value of dynamic content item type
        /// </summary>
        /// <value>Gets or sets the value of dynamic content item type</value>
        [DataMember(Name="contentType", EmitDefaultValue=false)]
        public string ContentType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of dynamic content item description
        /// </summary>
        /// <value>Gets or sets the value of dynamic content item description</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of dynamic content item id
        /// </summary>
        /// <value>Gets or sets the value of dynamic content item id</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or sets the flag of supporting multilanguage for dynamic content item
        /// </summary>
        /// <value>Gets or sets the flag of supporting multilanguage for dynamic content item</value>
        [DataMember(Name="isMultilingual", EmitDefaultValue=false)]
        public bool? IsMultilingual { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of dynamic content item name
        /// </summary>
        /// <value>Gets or sets the value of dynamic content item name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the dictionary of dynamic content item propertines
        /// </summary>
        /// <value>Gets or sets the dictionary of dynamic content item propertines</value>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public Dictionary<string, Object> Properties { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelDynamicContentItem {\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  IsMultilingual: ").Append(IsMultilingual).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelDynamicContentItem);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelDynamicContentItem instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelDynamicContentItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelDynamicContentItem other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ContentType == other.ContentType ||
                    this.ContentType != null &&
                    this.ContentType.Equals(other.ContentType)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.IsMultilingual == other.IsMultilingual ||
                    this.IsMultilingual != null &&
                    this.IsMultilingual.Equals(other.IsMultilingual)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
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
                
                if (this.ContentType != null)
                    hash = hash * 57 + this.ContentType.GetHashCode();
                
                if (this.Description != null)
                    hash = hash * 57 + this.Description.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.IsMultilingual != null)
                    hash = hash * 57 + this.IsMultilingual.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Properties != null)
                    hash = hash * 57 + this.Properties.GetHashCode();
                
                return hash;
            }
        }

    }


}
