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
    public class VirtoCommerceMerchandisingModuleWebModelProductSearchResult : IEquatable<VirtoCommerceMerchandisingModuleWebModelProductSearchResult>
    {
        
        /// <summary>
        /// Gets or sets the collection of facets
        /// </summary>
        /// <value>Gets or sets the collection of facets</value>
        [DataMember(Name="facets", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelFacet> Facets { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of reposponse items
        /// </summary>
        /// <value>Gets or sets the collection of reposponse items</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelProduct> Items { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of response items total count
        /// </summary>
        /// <value>Gets or sets the value of response items total count</value>
        [DataMember(Name="total", EmitDefaultValue=false)]
        public int? Total { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelProductSearchResult {\n");
            sb.Append("  Facets: ").Append(Facets).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  Total: ").Append(Total).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelProductSearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelProductSearchResult instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelProductSearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelProductSearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Facets == other.Facets ||
                    this.Facets != null &&
                    this.Facets.SequenceEqual(other.Facets)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.Total == other.Total ||
                    this.Total != null &&
                    this.Total.Equals(other.Total)
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
                
                if (this.Facets != null)
                    hash = hash * 57 + this.Facets.GetHashCode();
                
                if (this.Items != null)
                    hash = hash * 57 + this.Items.GetHashCode();
                
                if (this.Total != null)
                    hash = hash * 57 + this.Total.GetHashCode();
                
                return hash;
            }
        }

    }


}
