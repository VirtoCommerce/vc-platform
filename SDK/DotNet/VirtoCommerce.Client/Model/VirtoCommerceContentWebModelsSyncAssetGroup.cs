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
    public class VirtoCommerceContentWebModelsSyncAssetGroup : IEquatable<VirtoCommerceContentWebModelsSyncAssetGroup>
    {
        
        /// <summary>
        /// Asset element group type, one of the two predefined values - 'pages', 'theme'
        /// </summary>
        /// <value>Asset element group type, one of the two predefined values - 'pages', 'theme'</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }
  
        
        /// <summary>
        /// Gets or Sets Assets
        /// </summary>
        [DataMember(Name="assets", EmitDefaultValue=false)]
        public List<VirtoCommerceContentWebModelsSyncAsset> Assets { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsSyncAssetGroup {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Assets: ").Append(Assets).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsSyncAssetGroup);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsSyncAssetGroup instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceContentWebModelsSyncAssetGroup to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsSyncAssetGroup other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Assets == other.Assets ||
                    this.Assets != null &&
                    this.Assets.SequenceEqual(other.Assets)
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
                
                if (this.Type != null)
                    hash = hash * 57 + this.Type.GetHashCode();
                
                if (this.Assets != null)
                    hash = hash * 57 + this.Assets.GetHashCode();
                
                return hash;
            }
        }

    }


}
