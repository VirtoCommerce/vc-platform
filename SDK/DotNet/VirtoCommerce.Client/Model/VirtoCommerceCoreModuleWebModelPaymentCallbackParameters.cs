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
    public partial class VirtoCommerceCoreModuleWebModelPaymentCallbackParameters :  IEquatable<VirtoCommerceCoreModuleWebModelPaymentCallbackParameters>
    {
        /// <summary>
        /// Gets or Sets Parameters
        /// </summary>
        [DataMember(Name="parameters", EmitDefaultValue=false)]
        public List<VirtoCommerceCoreModuleWebModelKeyValuePair> Parameters { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCoreModuleWebModelPaymentCallbackParameters {\n");
            sb.Append("  Parameters: ").Append(Parameters).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCoreModuleWebModelPaymentCallbackParameters);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCoreModuleWebModelPaymentCallbackParameters instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCoreModuleWebModelPaymentCallbackParameters to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCoreModuleWebModelPaymentCallbackParameters other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Parameters == other.Parameters ||
                    this.Parameters != null &&
                    this.Parameters.SequenceEqual(other.Parameters)
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

                if (this.Parameters != null)
                    hash = hash * 59 + this.Parameters.GetHashCode();

                return hash;
            }
        }

    }
}
