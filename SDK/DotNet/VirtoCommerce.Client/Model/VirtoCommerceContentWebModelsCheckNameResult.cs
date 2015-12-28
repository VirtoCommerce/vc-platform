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
    public class VirtoCommerceContentWebModelsCheckNameResult : IEquatable<VirtoCommerceContentWebModelsCheckNameResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtoCommerceContentWebModelsCheckNameResult" /> class.
        /// </summary>
        public VirtoCommerceContentWebModelsCheckNameResult()
        {
            
        }

        
        /// <summary>
        /// Result of checking (if true - enable to save object, if false - unable to save object)
        /// </summary>
        /// <value>Result of checking (if true - enable to save object, if false - unable to save object)</value>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public bool? Result { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceContentWebModelsCheckNameResult {\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceContentWebModelsCheckNameResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceContentWebModelsCheckNameResult instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceContentWebModelsCheckNameResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceContentWebModelsCheckNameResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Result == other.Result ||
                    this.Result != null &&
                    this.Result.Equals(other.Result)
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
                
                if (this.Result != null)
                    hash = hash * 57 + this.Result.GetHashCode();
                
                return hash;
            }
        }

    }


}
