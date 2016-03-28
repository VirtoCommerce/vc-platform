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
    public partial class VirtoCommerceContentWebModelsPage :  IEquatable<VirtoCommerceContentWebModelsPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsPage" /> class.
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsPage" />class.
        /// </summary>
        /// <param name="Id">Id.</param>
        /// <param name="Name">Page element name, contains the path relative to the root pages folder.</param>
        /// <param name="Content">Page element text content (text page element), based on content type.</param>
        /// <param name="ByteContent">Page element byte content (non-text page element), bases on content type.</param>
        /// <param name="ContentType">ContentType.</param>
        /// <param name="Language">Locale.</param>
        /// <param name="ModifiedDate">ModifiedDate.</param>
        /// <param name="FileUrl">FileUrl.</param>
        /// <param name="SecurityScopes">SecurityScopes.</param>

        public VirtoCommerceContentWebModelsPage(string Id = null, string Name = null, string Content = null, byte[] ByteContent = null, string ContentType = null, string Language = null, DateTime? ModifiedDate = null, string FileUrl = null, List<string> SecurityScopes = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Content = Content;
            this.ByteContent = ByteContent;
            this.ContentType = ContentType;
            this.Language = Language;
            this.ModifiedDate = ModifiedDate;
            this.FileUrl = FileUrl;
            this.SecurityScopes = SecurityScopes;
            
        }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Page element name, contains the path relative to the root pages folder
        /// </summary>
        /// <value>Page element name, contains the path relative to the root pages folder</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Page element text content (text page element), based on content type
        /// </summary>
        /// <value>Page element text content (text page element), based on content type</value>
        [DataMember(Name="content", EmitDefaultValue=false)]
        public string Content { get; set; }

        /// <summary>
        /// Page element byte content (non-text page element), bases on content type
        /// </summary>
        /// <value>Page element byte content (non-text page element), bases on content type</value>
        [DataMember(Name="byteContent", EmitDefaultValue=false)]
        public byte[] ByteContent { get; set; }

        /// <summary>
        /// Gets or Sets ContentType
        /// </summary>
        [DataMember(Name="contentType", EmitDefaultValue=false)]
        public string ContentType { get; set; }

        /// <summary>
        /// Locale
        /// </summary>
        /// <value>Locale</value>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets FileUrl
        /// </summary>
        [DataMember(Name="fileUrl", EmitDefaultValue=false)]
        public string FileUrl { get; set; }

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
            sb.Append("class VirtoCommerceContentWebModelsPage {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Content: ").Append(Content).Append("\n");
            sb.Append("  ByteContent: ").Append(ByteContent).Append("\n");
            sb.Append("  ContentType: ").Append(ContentType).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  FileUrl: ").Append(FileUrl).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsPage);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsPage instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsPage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsPage other)
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
                    this.ContentType == other.ContentType ||
                    this.ContentType != null &&
                    this.ContentType.Equals(other.ContentType)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.FileUrl == other.FileUrl ||
                    this.FileUrl != null &&
                    this.FileUrl.Equals(other.FileUrl)
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
                
                if (this.ContentType != null)
                    hash = hash * 59 + this.ContentType.GetHashCode();
                
                if (this.Language != null)
                    hash = hash * 59 + this.Language.GetHashCode();
                
                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();
                
                if (this.FileUrl != null)
                    hash = hash * 59 + this.FileUrl.GetHashCode();
                
                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();
                
                return hash;
            }
        }

    }


}
