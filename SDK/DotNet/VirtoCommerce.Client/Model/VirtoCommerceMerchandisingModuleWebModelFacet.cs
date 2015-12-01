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
    public class VirtoCommerceMerchandisingModuleWebModelFacet : IEquatable<VirtoCommerceMerchandisingModuleWebModelFacet>
    {
        
        /// <summary>
        /// Gets or sets the value of facet type
        /// </summary>
        /// <value>Gets or sets the value of facet type</value>
        [DataMember(Name="facetType", EmitDefaultValue=false)]
        public string FacetType { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of facet field
        /// </summary>
        /// <value>Gets or sets the value of facet field</value>
        [DataMember(Name="field", EmitDefaultValue=false)]
        public string Field { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of facet label
        /// </summary>
        /// <value>Gets or sets the value of facet label</value>
        [DataMember(Name="label", EmitDefaultValue=false)]
        public string Label { get; set; }
  
        
        /// <summary>
        /// Gets or sets the collection of facet values
        /// </summary>
        /// <value>Gets or sets the collection of facet values</value>
        [DataMember(Name="values", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelFacetValue> Values { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelFacet {\n");
            sb.Append("  FacetType: ").Append(FacetType).Append("\n");
            sb.Append("  Field: ").Append(Field).Append("\n");
            sb.Append("  Label: ").Append(Label).Append("\n");
            sb.Append("  Values: ").Append(Values).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelFacet);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelFacet instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelFacet to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelFacet other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.FacetType == other.FacetType ||
                    this.FacetType != null &&
                    this.FacetType.Equals(other.FacetType)
                ) && 
                (
                    this.Field == other.Field ||
                    this.Field != null &&
                    this.Field.Equals(other.Field)
                ) && 
                (
                    this.Label == other.Label ||
                    this.Label != null &&
                    this.Label.Equals(other.Label)
                ) && 
                (
                    this.Values == other.Values ||
                    this.Values != null &&
                    this.Values.SequenceEqual(other.Values)
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
                
                if (this.FacetType != null)
                    hash = hash * 57 + this.FacetType.GetHashCode();
                
                if (this.Field != null)
                    hash = hash * 57 + this.Field.GetHashCode();
                
                if (this.Label != null)
                    hash = hash * 57 + this.Label.GetHashCode();
                
                if (this.Values != null)
                    hash = hash * 57 + this.Values.GetHashCode();
                
                return hash;
            }
        }

    }


}
