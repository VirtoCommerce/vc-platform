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
    public partial class VirtoCommerceContentWebModelsThemeAsset :  IEquatable<VirtoCommerceContentWebModelsThemeAsset>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsThemeAsset" /> class.
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsThemeAsset" />class.
        /// </summary>
        /// <param name="Id">Id, contains full path relative to theme root folder.</param>
        /// <param name="Name">Name.</param>
        /// <param name="Content">Theme asset text content (text files - html, css, js &amp;amp; etc), based on content type.</param>
        /// <param name="ByteContent">Theme asset byte content (non-text files - images, fonts, zips &amp;amp; etc), based on content type.</param>
        /// <param name="AssetUrl">AssetUrl.</param>
        /// <param name="ContentType">ContentType.</param>
        /// <param name="Updated">Theme asset last update date.</param>
        /// <param name="SecurityScopes">SecurityScopes.</param>

        public VirtoCommerceContentWebModelsThemeAsset(string Id = null, string Name = null, string Content = null, byte[] ByteContent = null, string AssetUrl = null, string ContentType = null, DateTime? Updated = null, List<string> SecurityScopes = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Content = Content;
            this.ByteContent = ByteContent;
            this.AssetUrl = AssetUrl;
            this.ContentType = ContentType;
            this.Updated = Updated;
            this.SecurityScopes = SecurityScopes;
            
        }

        /// <summary>
        /// Id, contains full path relative to theme root folder
        /// </summary>
        /// <value>Id, contains full path relative to theme root folder</value>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Theme asset text content (text files - html, css, js &amp;amp; etc), based on content type
        /// </summary>
        /// <value>Theme asset text content (text files - html, css, js &amp;amp; etc), based on content type</value>
        [DataMember(Name="content", EmitDefaultValue=false)]
        public string Content { get; set; }

        /// <summary>
        /// Theme asset byte content (non-text files - images, fonts, zips &amp;amp; etc), based on content type
        /// </summary>
        /// <value>Theme asset byte content (non-text files - images, fonts, zips &amp;amp; etc), based on content type</value>
        [DataMember(Name="byteContent", EmitDefaultValue=false)]
        public byte[] ByteContent { get; set; }

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
        /// Theme asset last update date
        /// </summary>
        /// <value>Theme asset last update date</value>
        [DataMember(Name="updated", EmitDefaultValue=false)]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Gets or Sets SecurityScopes
        /// </summary>
        [DataMember(Name="securityScopes", EmitDefaultValue=false)]
        public List<string> SecurityScopes { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsThemeAsset {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
            sb.Append("  AssetUrl: ").Append(AssetUrl).Append("\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Updated: ").Append(Updated).Append("\n");
            sb.Append("  SecurityScopes: ").Append(SecurityScopes).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsThemeAsset);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsThemeAsset instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsThemeAsset to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsThemeAsset other)
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
                ) && 
                (
                    this.SecurityScopes == other.SecurityScopes ||
                    this.SecurityScopes != null &&
                    this.SecurityScopes.SequenceEqual(other.SecurityScopes)
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
                    hash = hash * 59 + this.Id.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();
                
                if (this.Content != null)
                    hash = hash * 59 + this.Content.GetHashCode();
                
                if (this.ByteContent != null)
                    hash = hash * 59 + this.ByteContent.GetHashCode();
                
                if (this.AssetUrl != null)
                    hash = hash * 59 + this.AssetUrl.GetHashCode();
                
                if (this.ContentType != null)
                    hash = hash * 59 + this.ContentType.GetHashCode();
                
                if (this.Updated != null)
                    hash = hash * 59 + this.Updated.GetHashCode();
                
                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();
                
                return hash;
            }
        }

    }


}
