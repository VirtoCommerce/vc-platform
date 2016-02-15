using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;



namespace VirtoCommerce.Client.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceContentWebModelsThemeAssetFolder :  IEquatable<VirtoCommerceContentWebModelsThemeAssetFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsThemeAssetFolder" /> class.
        /// </summary>
        public VirtoCommerceContentWebModelsThemeAssetFolder()
        {
            
        }

        
        /// <summary>
        /// Theme asset folder name, one of the predefined values - 'assets', 'templates', 'snippets', 'layout', 'config', 'locales'
        /// </summary>
        /// <value>Theme asset folder name, one of the predefined values - 'assets', 'templates', 'snippets', 'layout', 'config', 'locales'</value>
        [DataMember(Name="folderName", EmitDefaultValue=false)]
        public string FolderName { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Assets
        /// </summary>
        [DataMember(Name="assets", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsThemeAsset> Assets { get; set; }
  
        
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
            sb.Append("class VirtoCommerceContentWebModelsThemeAssetFolder {\n");
            sb.Append("  FolderName: ").Append(FolderName).Append("\n");
            sb.Append("  Assets: ").Append(Assets).Append("\n");
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
            return this.Equals(obj as VirtoCommerceContentWebModelsThemeAssetFolder);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsThemeAssetFolder instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsThemeAssetFolder to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsThemeAssetFolder other)
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
                    this.Assets == other.Assets ||
                    this.Assets != null &&
                    this.Assets.SequenceEqual(other.Assets)
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
                    hash = hash * 59 + this.FolderName.GetHashCode();
                
                if (this.Assets != null)
                    hash = hash * 59 + this.Assets.GetHashCode();
                
                if (this.SecurityScopes != null)
                    hash = hash * 59 + this.SecurityScopes.GetHashCode();
                
                return hash;
            }
        }

    }


}
