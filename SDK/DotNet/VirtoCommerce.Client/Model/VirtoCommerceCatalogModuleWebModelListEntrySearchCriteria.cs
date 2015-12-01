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
    /// Search criteria for categories and/or items.
    /// </summary>
    [DataContract]
    public class VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria : IEquatable<VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria>
    {
        
        /// <summary>
        /// Gets or sets the response group to define which types of entries to search for.
        /// </summary>
        /// <value>Gets or sets the response group to define which types of entries to search for.</value>
        [DataMember(Name="responseGroup", EmitDefaultValue=false)]
        public string ResponseGroup { get; set; }
  
        
        /// <summary>
        /// Gets or sets the keyword to search for.
        /// </summary>
        /// <value>Gets or sets the keyword to search for.</value>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }
  
        
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>Gets or sets the category identifier.</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>Gets or sets the catalog identifier.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }
  
        
        /// <summary>
        /// Gets or sets the start index of total results from which entries should be returned.
        /// </summary>
        /// <value>Gets or sets the start index of total results from which entries should be returned.</value>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }
  
        
        /// <summary>
        /// Gets or sets the maximum count of results to return.
        /// </summary>
        /// <value>Gets or sets the maximum count of results to return.</value>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria {\n");
            sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Start: ").Append(Start).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelListEntrySearchCriteria other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ResponseGroup == other.ResponseGroup ||
                    this.ResponseGroup != null &&
                    this.ResponseGroup.Equals(other.ResponseGroup)
                ) && 
                (
                    this.Keyword == other.Keyword ||
                    this.Keyword != null &&
                    this.Keyword.Equals(other.Keyword)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.Start == other.Start ||
                    this.Start != null &&
                    this.Start.Equals(other.Start)
                ) && 
                (
                    this.Count == other.Count ||
                    this.Count != null &&
                    this.Count.Equals(other.Count)
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
                
                if (this.ResponseGroup != null)
                    hash = hash * 57 + this.ResponseGroup.GetHashCode();
                
                if (this.Keyword != null)
                    hash = hash * 57 + this.Keyword.GetHashCode();
                
                if (this.CategoryId != null)
                    hash = hash * 57 + this.CategoryId.GetHashCode();
                
                if (this.CatalogId != null)
                    hash = hash * 57 + this.CatalogId.GetHashCode();
                
                if (this.Start != null)
                    hash = hash * 57 + this.Start.GetHashCode();
                
                if (this.Count != null)
                    hash = hash * 57 + this.Count.GetHashCode();
                
                return hash;
            }
        }

    }


}
