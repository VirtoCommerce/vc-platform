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
    public partial class VirtoCommerceContentWebModelsGetPagesResult :  IEquatable<VirtoCommerceContentWebModelsGetPagesResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsGetPagesResult" /> class.
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsGetPagesResult" />class.
        /// </summary>
        /// <param name="Folders">Collection of pages folders (by default - &#39;pages&#39;, &#39;includes&#39;), that contains page elements.</param>
        /// <param name="Pages">Collection of page elements (used in pages rendering (page html, images, etc.)).</param>

        public VirtoCommerceContentWebModelsGetPagesResult(List<VirtoCommerceContentWebModelsPageFolder> Folders = null, List<VirtoCommerceContentWebModelsPage> Pages = null)
        {
            this.Folders = Folders;
            this.Pages = Pages;
            
        }

        /// <summary>
        /// Collection of pages folders (by default - &#39;pages&#39;, &#39;includes&#39;), that contains page elements
        /// </summary>
        /// <value>Collection of pages folders (by default - &#39;pages&#39;, &#39;includes&#39;), that contains page elements</value>
        [DataMember(Name="folders", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsPageFolder> Folders { get; set; }

        /// <summary>
        /// Collection of page elements (used in pages rendering (page html, images, etc.))
        /// </summary>
        /// <value>Collection of page elements (used in pages rendering (page html, images, etc.))</value>
        [DataMember(Name="pages", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsPage> Pages { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsGetPagesResult {\n");
            sb.Append("  Folders: ").Append(Folders).Append("\n");
            sb.Append("  Pages: ").Append(Pages).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsGetPagesResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsGetPagesResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceContentWebModelsGetPagesResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsGetPagesResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Folders == other.Folders ||
                    this.Folders != null &&
                    this.Folders.SequenceEqual(other.Folders)
                ) && 
                (
                    this.Pages == other.Pages ||
                    this.Pages != null &&
                    this.Pages.SequenceEqual(other.Pages)
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
                
                if (this.Folders != null)
                    hash = hash * 59 + this.Folders.GetHashCode();
                
                if (this.Pages != null)
                    hash = hash * 59 + this.Pages.GetHashCode();
                
                return hash;
            }
        }

    }


}
