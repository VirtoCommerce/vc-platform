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
    public class VirtoCommerceDomainCatalogModelSearchCriteria : IEquatable<VirtoCommerceDomainCatalogModelSearchCriteria>
    {
        
        /// <summary>
        /// Gets or Sets ResponseGroup
        /// </summary>
        [DataMember(Name="responseGroup", EmitDefaultValue=false)]
        public string ResponseGroup { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Keyword
        /// </summary>
        [DataMember(Name="keyword", EmitDefaultValue=false)]
        public string Keyword { get; set; }
  
        
        /// <summary>
        /// Gets or Sets SearchInChildren
        /// </summary>
        [DataMember(Name="searchInChildren", EmitDefaultValue=false)]
        public bool? SearchInChildren { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CategoriesIds
        /// </summary>
        [DataMember(Name="categoriesIds", EmitDefaultValue=false)]
        public List<string> CategoriesIds { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }
  
        
        /// <summary>
        /// Gets or Sets CatalogsIds
        /// </summary>
        [DataMember(Name="catalogsIds", EmitDefaultValue=false)]
        public List<string> CatalogsIds { get; set; }
  
        
        /// <summary>
        /// Gets or Sets LanguageCode
        /// </summary>
        [DataMember(Name="languageCode", EmitDefaultValue=false)]
        public string LanguageCode { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Sort
        /// </summary>
        [DataMember(Name="sort", EmitDefaultValue=false)]
        public string Sort { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Facets
        /// </summary>
        [DataMember(Name="facets", EmitDefaultValue=false)]
        public List<string> Facets { get; set; }
  
        
        /// <summary>
        /// Gets or Sets HideDirectLinkedCategories
        /// </summary>
        [DataMember(Name="hideDirectLinkedCategories", EmitDefaultValue=false)]
        public bool? HideDirectLinkedCategories { get; set; }
  
        
        /// <summary>
        /// Gets or Sets PropertyValues
        /// </summary>
        [DataMember(Name="propertyValues", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Start
        /// </summary>
        [DataMember(Name="start", EmitDefaultValue=false)]
        public int? Start { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Count
        /// </summary>
        [DataMember(Name="count", EmitDefaultValue=false)]
        public int? Count { get; set; }
  
        
        /// <summary>
        /// Gets or Sets IndexDate
        /// </summary>
        [DataMember(Name="indexDate", EmitDefaultValue=false)]
        public DateTime? IndexDate { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainCatalogModelSearchCriteria {\n");
            sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
            sb.Append("  Keyword: ").Append(Keyword).Append("\n");
            sb.Append("  SearchInChildren: ").Append(SearchInChildren).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  CategoriesIds: ").Append(CategoriesIds).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  CatalogsIds: ").Append(CatalogsIds).Append("\n");
            sb.Append("  LanguageCode: ").Append(LanguageCode).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Sort: ").Append(Sort).Append("\n");
            sb.Append("  Facets: ").Append(Facets).Append("\n");
            sb.Append("  HideDirectLinkedCategories: ").Append(HideDirectLinkedCategories).Append("\n");
            sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
            sb.Append("  Start: ").Append(Start).Append("\n");
            sb.Append("  Count: ").Append(Count).Append("\n");
            sb.Append("  IndexDate: ").Append(IndexDate).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelSearchCriteria);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelSearchCriteria instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceDomainCatalogModelSearchCriteria to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelSearchCriteria other)
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
                    this.SearchInChildren == other.SearchInChildren ||
                    this.SearchInChildren != null &&
                    this.SearchInChildren.Equals(other.SearchInChildren)
                ) && 
                (
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.CategoriesIds == other.CategoriesIds ||
                    this.CategoriesIds != null &&
                    this.CategoriesIds.SequenceEqual(other.CategoriesIds)
                ) && 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.CatalogsIds == other.CatalogsIds ||
                    this.CatalogsIds != null &&
                    this.CatalogsIds.SequenceEqual(other.CatalogsIds)
                ) && 
                (
                    this.LanguageCode == other.LanguageCode ||
                    this.LanguageCode != null &&
                    this.LanguageCode.Equals(other.LanguageCode)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Sort == other.Sort ||
                    this.Sort != null &&
                    this.Sort.Equals(other.Sort)
                ) && 
                (
                    this.Facets == other.Facets ||
                    this.Facets != null &&
                    this.Facets.SequenceEqual(other.Facets)
                ) && 
                (
                    this.HideDirectLinkedCategories == other.HideDirectLinkedCategories ||
                    this.HideDirectLinkedCategories != null &&
                    this.HideDirectLinkedCategories.Equals(other.HideDirectLinkedCategories)
                ) && 
                (
                    this.PropertyValues == other.PropertyValues ||
                    this.PropertyValues != null &&
                    this.PropertyValues.SequenceEqual(other.PropertyValues)
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
                ) && 
                (
                    this.IndexDate == other.IndexDate ||
                    this.IndexDate != null &&
                    this.IndexDate.Equals(other.IndexDate)
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
                
                if (this.SearchInChildren != null)
                    hash = hash * 57 + this.SearchInChildren.GetHashCode();
                
                if (this.CategoryId != null)
                    hash = hash * 57 + this.CategoryId.GetHashCode();
                
                if (this.CategoriesIds != null)
                    hash = hash * 57 + this.CategoriesIds.GetHashCode();
                
                if (this.CatalogId != null)
                    hash = hash * 57 + this.CatalogId.GetHashCode();
                
                if (this.CatalogsIds != null)
                    hash = hash * 57 + this.CatalogsIds.GetHashCode();
                
                if (this.LanguageCode != null)
                    hash = hash * 57 + this.LanguageCode.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 57 + this.Currency.GetHashCode();
                
                if (this.Code != null)
                    hash = hash * 57 + this.Code.GetHashCode();
                
                if (this.Sort != null)
                    hash = hash * 57 + this.Sort.GetHashCode();
                
                if (this.Facets != null)
                    hash = hash * 57 + this.Facets.GetHashCode();
                
                if (this.HideDirectLinkedCategories != null)
                    hash = hash * 57 + this.HideDirectLinkedCategories.GetHashCode();
                
                if (this.PropertyValues != null)
                    hash = hash * 57 + this.PropertyValues.GetHashCode();
                
                if (this.Start != null)
                    hash = hash * 57 + this.Start.GetHashCode();
                
                if (this.Count != null)
                    hash = hash * 57 + this.Count.GetHashCode();
                
                if (this.IndexDate != null)
                    hash = hash * 57 + this.IndexDate.GetHashCode();
                
                return hash;
            }
        }

    }


}
