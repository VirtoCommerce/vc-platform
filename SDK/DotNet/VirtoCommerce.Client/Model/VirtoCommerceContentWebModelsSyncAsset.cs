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
    public class VirtoCommerceContentWebModelsSyncAsset : IEquatable<VirtoCommerceContentWebModelsSyncAsset>
    {
        
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }
  
        
        /// <summary>
        /// Asset element name, contains full path of theme asset or page element relative to the root
        /// </summary>
        /// <value>Asset element name, contains full path of theme asset or page element relative to the root</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Asset element text content (text files - html, js, css, txt, liquid &amp; etc), based on content type
        /// </summary>
        /// <value>Asset element text content (text files - html, js, css, txt, liquid &amp; etc), based on content type</value>
        [DataMember(Name="content", EmitDefaultValue=false)]
        public string Content { get; set; }
  
        
        /// <summary>
        /// Asset element byte content (non-text files - images, fonts, zips, pdfs &amp; etc), based on content type
        /// </summary>
        /// <value>Asset element byte content (non-text files - images, fonts, zips, pdfs &amp; etc), based on content type</value>
        [DataMember(Name="byteContent", EmitDefaultValue=false)]
        public string ByteContent { get; set; }
  
        
        /// <summary>
        /// Gets or Sets AssetUrl
        /// </summary>
        [DataMember(Name="assetUrl", EmitDefaultValue=false)]
        public string AssetUrl { get; set; }
  
        
        /// <summary>
        /// Gets or Sets ContentType
        /// </summary>
        [DataMember(Name="contentType", EmitDefaultValue=false)]
        public string ContentType { get; set; }
  
        
        /// <summary>
        /// Last updated date of the asset element
        /// </summary>
        /// <value>Last updated date of the asset element</value>
        [DataMember(Name="updated", EmitDefaultValue=false)]
        public DateTime? Updated { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsSyncAsset {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
            sb.Append("  AssetUrl: ").Append(AssetUrl).Append("\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Updated: ").Append(Updated).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsSyncAsset);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsSyncAsset instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceContentWebModelsSyncAsset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsSyncAsset other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Content == other.Content ||
                    this.Content != null &&
                    this.Content.Equals(other.Content)
                ) && 
                (
                    this.ByteContent == other.ByteContent ||
                    this.ByteContent != null &&
                    this.ByteContent.Equals(other.ByteContent)
                ) && 
                (
                    this.AssetUrl == other.AssetUrl ||
                    this.AssetUrl != null &&
                    this.AssetUrl.Equals(other.AssetUrl)
                ) && 
                (
                    this.ContentType == other.ContentType ||
                    this.ContentType != null &&
                    this.ContentType.Equals(other.ContentType)
                ) && 
                (
                    this.Updated == other.Updated ||
                    this.Updated != null &&
                    this.Updated.Equals(other.Updated)
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
                
                if (this.Id != null)
                    hash = hash * 57 + this.Id.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.Content != null)
                    hash = hash * 57 + this.Content.GetHashCode();
                
                if (this.ByteContent != null)
                    hash = hash * 57 + this.ByteContent.GetHashCode();
                
                if (this.AssetUrl != null)
                    hash = hash * 57 + this.AssetUrl.GetHashCode();
                
                if (this.ContentType != null)
                    hash = hash * 57 + this.ContentType.GetHashCode();
                
                if (this.Updated != null)
                    hash = hash * 57 + this.Updated.GetHashCode();
                
                return hash;
            }
        }

    }


}
