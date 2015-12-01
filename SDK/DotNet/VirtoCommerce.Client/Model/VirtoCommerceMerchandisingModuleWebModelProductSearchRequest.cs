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
    public class VirtoCommerceMerchandisingModuleWebModelProductSearchRequest : IEquatable<VirtoCommerceMerchandisingModuleWebModelProductSearchRequest>
    {
        
        /// <summary>
        /// Store ID
        /// </summary>
        /// <value>Store ID</value>
        [DataMember(Name="store", EmitDefaultValue=false)]
        public string Store { get; set; }
  
        
        /// <summary>
        /// Array of pricelist IDs
        /// </summary>
        /// <value>Array of pricelist IDs</value>
        [DataMember(Name="pricelists", EmitDefaultValue=false)]
        public List<string> Pricelists { get; set; }
  
        
        /// <summary>
        /// Response detalization scale (default value is ItemMedium)
        /// </summary>
        /// <value>Response detalization scale (default value is ItemMedium)</value>
        [DataMember(Name="responseGroup", EmitDefaultValue=false)]
        public string ResponseGroup { get; set; }
  
        
        /// <summary>
        /// Product category outline
        /// </summary>
        /// <value>Product category outline</value>
        [DataMember(Name="outline", EmitDefaultValue=false)]
        public string Outline { get; set; }
  
        
        /// <summary>
        /// Culture name (default value is \"en-us\")
        /// </summary>
        /// <value>Culture name (default value is \"en-us\")</value>
        [DataMember(Name="language", EmitDefaultValue=false)]
        public string Language { get; set; }
  
        
        /// <summary>
        /// Currency (default value is \"USD\")
        /// </summary>
        /// <value>Currency (default value is \"USD\")</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }
  
        
        /// <summary>
        /// Gets or sets the search phrase
        /// </summary>
        /// <value>Gets or sets the search phrase</value>
        [DataMember(Name="searchPhrase", EmitDefaultValue=false)]
        public string SearchPhrase { get; set; }
  
        
        /// <summary>
        /// Gets or sets the sort
        /// </summary>
        /// <value>Gets or sets the sort</value>
        [DataMember(Name="sort", EmitDefaultValue=false)]
        public string Sort { get; set; }
  
        
        /// <summary>
        /// Gets or sets the sort order ascending or descending
        /// </summary>
        /// <value>Gets or sets the sort order ascending or descending</value>
        [DataMember(Name="sortOrder", EmitDefaultValue=false)]
        public string SortOrder { get; set; }
  
        
        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        /// <value>Gets or sets the start date</value>
        [DataMember(Name="startDateFrom", EmitDefaultValue=false)]
        public DateTime? StartDateFrom { get; set; }
  
        
        /// <summary>
        /// Gets or sets the number of items to skip
        /// </summary>
        /// <value>Gets or sets the number of items to skip</value>
        [DataMember(Name="skip", EmitDefaultValue=false)]
        public int? Skip { get; set; }
  
        
        /// <summary>
        /// Gets or sets the number of items to return
        /// </summary>
        /// <value>Gets or sets the number of items to return</value>
        [DataMember(Name="take", EmitDefaultValue=false)]
        public int? Take { get; set; }
  
        
        /// <summary>
        /// Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3
        /// </summary>
        /// <value>Gets or sets search terms collection\r\n            Item format: name:value1,value2,value3</value>
        [DataMember(Name="terms", EmitDefaultValue=false)]
        public List<string> Terms { get; set; }
  
        
        /// <summary>
        /// Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3
        /// </summary>
        /// <value>Gets or sets the facets collection\r\n            Item format: name:value1,value2,value3</value>
        [DataMember(Name="facets", EmitDefaultValue=false)]
        public List<string> Facets { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelProductSearchRequest {\n");
            sb.Append("  Store: ").Append(Store).Append("\n");
            sb.Append("  Pricelists: ").Append(Pricelists).Append("\n");
            sb.Append("  ResponseGroup: ").Append(ResponseGroup).Append("\n");
            sb.Append("  Outline: ").Append(Outline).Append("\n");
            sb.Append("  Language: ").Append(Language).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  SearchPhrase: ").Append(SearchPhrase).Append("\n");
            sb.Append("  Sort: ").Append(Sort).Append("\n");
            sb.Append("  SortOrder: ").Append(SortOrder).Append("\n");
            sb.Append("  StartDateFrom: ").Append(StartDateFrom).Append("\n");
            sb.Append("  Skip: ").Append(Skip).Append("\n");
            sb.Append("  Take: ").Append(Take).Append("\n");
            sb.Append("  Terms: ").Append(Terms).Append("\n");
            sb.Append("  Facets: ").Append(Facets).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelProductSearchRequest);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelProductSearchRequest instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelProductSearchRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelProductSearchRequest other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Store == other.Store ||
                    this.Store != null &&
                    this.Store.Equals(other.Store)
                ) && 
                (
                    this.Pricelists == other.Pricelists ||
                    this.Pricelists != null &&
                    this.Pricelists.SequenceEqual(other.Pricelists)
                ) && 
                (
                    this.ResponseGroup == other.ResponseGroup ||
                    this.ResponseGroup != null &&
                    this.ResponseGroup.Equals(other.ResponseGroup)
                ) && 
                (
                    this.Outline == other.Outline ||
                    this.Outline != null &&
                    this.Outline.Equals(other.Outline)
                ) && 
                (
                    this.Language == other.Language ||
                    this.Language != null &&
                    this.Language.Equals(other.Language)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.SearchPhrase == other.SearchPhrase ||
                    this.SearchPhrase != null &&
                    this.SearchPhrase.Equals(other.SearchPhrase)
                ) && 
                (
                    this.Sort == other.Sort ||
                    this.Sort != null &&
                    this.Sort.Equals(other.Sort)
                ) && 
                (
                    this.SortOrder == other.SortOrder ||
                    this.SortOrder != null &&
                    this.SortOrder.Equals(other.SortOrder)
                ) && 
                (
                    this.StartDateFrom == other.StartDateFrom ||
                    this.StartDateFrom != null &&
                    this.StartDateFrom.Equals(other.StartDateFrom)
                ) && 
                (
                    this.Skip == other.Skip ||
                    this.Skip != null &&
                    this.Skip.Equals(other.Skip)
                ) && 
                (
                    this.Take == other.Take ||
                    this.Take != null &&
                    this.Take.Equals(other.Take)
                ) && 
                (
                    this.Terms == other.Terms ||
                    this.Terms != null &&
                    this.Terms.SequenceEqual(other.Terms)
                ) && 
                (
                    this.Facets == other.Facets ||
                    this.Facets != null &&
                    this.Facets.SequenceEqual(other.Facets)
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
                
                if (this.Store != null)
                    hash = hash * 57 + this.Store.GetHashCode();
                
                if (this.Pricelists != null)
                    hash = hash * 57 + this.Pricelists.GetHashCode();
                
                if (this.ResponseGroup != null)
                    hash = hash * 57 + this.ResponseGroup.GetHashCode();
                
                if (this.Outline != null)
                    hash = hash * 57 + this.Outline.GetHashCode();
                
                if (this.Language != null)
                    hash = hash * 57 + this.Language.GetHashCode();
                
                if (this.Currency != null)
                    hash = hash * 57 + this.Currency.GetHashCode();
                
                if (this.SearchPhrase != null)
                    hash = hash * 57 + this.SearchPhrase.GetHashCode();
                
                if (this.Sort != null)
                    hash = hash * 57 + this.Sort.GetHashCode();
                
                if (this.SortOrder != null)
                    hash = hash * 57 + this.SortOrder.GetHashCode();
                
                if (this.StartDateFrom != null)
                    hash = hash * 57 + this.StartDateFrom.GetHashCode();
                
                if (this.Skip != null)
                    hash = hash * 57 + this.Skip.GetHashCode();
                
                if (this.Take != null)
                    hash = hash * 57 + this.Take.GetHashCode();
                
                if (this.Terms != null)
                    hash = hash * 57 + this.Terms.GetHashCode();
                
                if (this.Facets != null)
                    hash = hash * 57 + this.Facets.GetHashCode();
                
                return hash;
            }
        }

    }


}
