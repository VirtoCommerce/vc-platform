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
    public class VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection : IEquatable<VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection>
    {
        
        /// <summary>
        /// Gets or sets the collection of reposponse items
        /// </summary>
        /// <value>Gets or sets the collection of reposponse items</value>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceMerchandisingModuleWebModelCategory> Items { get; set; }
  
        
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
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection {\n");
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelCategoryResponseCollection other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
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
                
                if (this.Items != null)
                    hash = hash * 57 + this.Items.GetHashCode();
                
                if (this.Total != null)
                    hash = hash * 57 + this.Total.GetHashCode();
                
                return hash;
            }
        }

    }


}
