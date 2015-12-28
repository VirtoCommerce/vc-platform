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
    public class VirtoCommerceContentWebModelsPageFolder : IEquatable<VirtoCommerceContentWebModelsPageFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsPageFolder" /> class.
        /// </summary>
        public VirtoCommerceContentWebModelsPageFolder()
        {
            
        }

        
        /// <summary>
        /// Page folder name, by-default 'pages' and 'includes'
        /// </summary>
        /// <value>Page folder name, by-default 'pages' and 'includes'</value>
        [DataMember(Name="folderName", EmitDefaultValue=false)]
        public string FolderName { get; set; }
  
        
        /// <summary>
        /// Collection of page elements in this folder
        /// </summary>
        /// <value>Collection of page elements in this folder</value>
        [DataMember(Name="pages", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsPage> Pages { get; set; }
  
        
        /// <summary>
        /// Collection of folders
        /// </summary>
        /// <value>Collection of folders</value>
        [DataMember(Name="folders", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsPageFolder> Folders { get; set; }
  
        
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
            sb.Append("class VirtoCommerceContentWebModelsPageFolder {\n");
            sb.Append("  FolderName: ").Append(FolderName).Append("\n");
            sb.Append("  Pages: ").Append(Pages).Append("\n");
            sb.Append("  Folders: ").Append(Folders).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsPageFolder);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsPageFolder instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceContentWebModelsPageFolder to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsPageFolder other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.FolderName == other.FolderName ||
                    this.FolderName != null &&
                    this.FolderName.Equals(other.FolderName)
                ) && 
                (
                    this.Pages == other.Pages ||
                    this.Pages != null &&
                    this.Pages.SequenceEqual(other.Pages)
                ) && 
                (
                    this.Folders == other.Folders ||
                    this.Folders != null &&
                    this.Folders.SequenceEqual(other.Folders)
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
                
                if (this.FolderName != null)
                    hash = hash * 57 + this.FolderName.GetHashCode();
                
                if (this.Pages != null)
                    hash = hash * 57 + this.Pages.GetHashCode();
                
                if (this.Folders != null)
                    hash = hash * 57 + this.Folders.GetHashCode();
                
                if (this.SecurityScopes != null)
                    hash = hash * 57 + this.SecurityScopes.GetHashCode();
                
                return hash;
            }
        }

    }


}
