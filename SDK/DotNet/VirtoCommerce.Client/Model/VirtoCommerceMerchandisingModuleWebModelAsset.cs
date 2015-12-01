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
    public class VirtoCommerceMerchandisingModuleWebModelAsset : IEquatable<VirtoCommerceMerchandisingModuleWebModelAsset>
    {
        
        /// <summary>
        /// Gets or sets the value of asset absolute URL
        /// </summary>
        /// <value>Gets or sets the value of asset absolute URL</value>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of asset group name
        /// </summary>
        /// <value>Gets or sets the value of asset group name</value>
        [DataMember(Name="group", EmitDefaultValue=false)]
        public string Group { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of asset name
        /// </summary>
        /// <value>Gets or sets the value of asset name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of asset file size in bytes
        /// </summary>
        /// <value>Gets or sets the value of asset file size in bytes</value>
        [DataMember(Name="size", EmitDefaultValue=false)]
        public long? Size { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of asset MIME type
        /// </summary>
        /// <value>Gets or sets the value of asset MIME type</value>
        [DataMember(Name="mimeType", EmitDefaultValue=false)]
        public string MimeType { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelAsset {\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  Group: ").Append(Group).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  MimeType: ").Append(MimeType).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelAsset);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelAsset instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelAsset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelAsset other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.Group == other.Group ||
                    this.Group != null &&
                    this.Group.Equals(other.Group)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Size == other.Size ||
                    this.Size != null &&
                    this.Size.Equals(other.Size)
                ) && 
                (
                    this.MimeType == other.MimeType ||
                    this.MimeType != null &&
                    this.MimeType.Equals(other.MimeType)
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
                
                if (this.Url != null)
                    hash = hash * 57 + this.Url.GetHashCode();
                
                if (this.Group != null)
                    hash = hash * 57 + this.Group.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Size != null)
                    hash = hash * 57 + this.Size.GetHashCode();
                
                if (this.MimeType != null)
                    hash = hash * 57 + this.MimeType.GetHashCode();
                
                return hash;
            }
        }

    }


}
