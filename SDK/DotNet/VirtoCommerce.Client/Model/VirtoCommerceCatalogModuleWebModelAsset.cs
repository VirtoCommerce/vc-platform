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
    /// Asset containing any content.
    /// </summary>
    [DataContract]
    public class VirtoCommerceCatalogModuleWebModelAsset : IEquatable<VirtoCommerceCatalogModuleWebModelAsset>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceCatalogModuleWebModelAsset" /> class.
        /// </summary>
        public VirtoCommerceCatalogModuleWebModelAsset()
        {
            
        }

        
        /// <summary>
        /// Gets or Sets Size
        /// </summary>
        [DataMember(Name="size", EmitDefaultValue=false)]
        public long? Size { get; set; }
  
        
        /// <summary>
        /// Gets or Sets MimeType
        /// </summary>
        [DataMember(Name="mimeType", EmitDefaultValue=false)]
        public string MimeType { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Gets or Sets RelativeUrl
        /// </summary>
        [DataMember(Name="relativeUrl", EmitDefaultValue=false)]
        public string RelativeUrl { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [DataMember(Name="url", EmitDefaultValue=false)]
        public string Url { get; set; }
  
        
        /// <summary>
        /// Gets or sets the asset type identifier.
        /// </summary>
        /// <value>Gets or sets the asset type identifier.</value>
        [DataMember(Name="typeId", EmitDefaultValue=false)]
        public string TypeId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the asset group name.
        /// </summary>
        /// <value>Gets or sets the asset group name.</value>
        [DataMember(Name="group", EmitDefaultValue=false)]
        public string Group { get; set; }
  
        
        /// <summary>
        /// Gets or sets the asset name.
        /// </summary>
        /// <value>Gets or sets the asset name.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the asset language.
        /// </summary>
        /// <value>Gets or sets the asset language.</value>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }
  
        
        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        /// <value>System flag used to mark that object was inherited from other</value>
        [DataMember(Name="isInherited", EmitDefaultValue=false)]
        public bool? IsInherited { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelAsset {\n");
            sb.Append("  Size: ").Append(Size).Append("\n");
            sb.Append("  MimeType: ").Append(MimeType).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  RelativeUrl: ").Append(RelativeUrl).Append("\n");
            sb.Append("  Url: ").Append(Url).Append("\n");
            sb.Append("  TypeId: ").Append(TypeId).Append("\n");
            sb.Append("  Group: ").Append(Group).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  IsInherited: ").Append(IsInherited).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelAsset);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelAsset instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceCatalogModuleWebModelAsset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelAsset other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Size == other.Size ||
                    this.Size != null &&
                    this.Size.Equals(other.Size)
                ) && 
                (
                    this.MimeType == other.MimeType ||
                    this.MimeType != null &&
                    this.MimeType.Equals(other.MimeType)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.RelativeUrl == other.RelativeUrl ||
                    this.RelativeUrl != null &&
                    this.RelativeUrl.Equals(other.RelativeUrl)
                ) && 
                (
                    this.Url == other.Url ||
                    this.Url != null &&
                    this.Url.Equals(other.Url)
                ) && 
                (
                    this.TypeId == other.TypeId ||
                    this.TypeId != null &&
                    this.TypeId.Equals(other.TypeId)
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
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.IsInherited == other.IsInherited ||
                    this.IsInherited != null &&
                    this.IsInherited.Equals(other.IsInherited)
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
                
                if (this.Size != null)
                    hash = hash * 57 + this.Size.GetHashCode();
                
                if (this.MimeType != null)
                    hash = hash * 57 + this.MimeType.GetHashCode();
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.RelativeUrl != null)
                    hash = hash * 57 + this.RelativeUrl.GetHashCode();
                
                if (this.Url != null)
                    hash = hash * 57 + this.Url.GetHashCode();
                
                if (this.TypeId != null)
                    hash = hash * 57 + this.TypeId.GetHashCode();
                
                if (this.Group != null)
                    hash = hash * 57 + this.Group.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.LanguageCode != null)
                    hash = hash * 57 + this.LanguageCode.GetHashCode();
                
                if (this.IsInherited != null)
                    hash = hash * 57 + this.IsInherited.GetHashCode();
                
                return hash;
            }
        }

    }


}
