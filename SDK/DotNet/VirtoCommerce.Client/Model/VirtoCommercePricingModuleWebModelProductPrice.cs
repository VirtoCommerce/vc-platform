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
    public partial class VirtoCommercePricingModuleWebModelProductPrice :  IEquatable<VirtoCommercePricingModuleWebModelProductPrice>
    {
        /// <summary>
        /// Gets or Sets ProductId
        /// </summary>
        [DataMember(Name="productId", EmitDefaultValue=false)]
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or Sets ProductName
        /// </summary>
        [DataMember(Name="productName", EmitDefaultValue=false)]
        public string ProductName { get; set; }

        /// <summary>
        /// List prices for the products. It includes tiered prices also. (Depending on the quantity, for example)
        /// </summary>
        /// <value>List prices for the products. It includes tiered prices also. (Depending on the quantity, for example)</value>
        [DataMember(Name="prices", EmitDefaultValue=false)]
        public List<VirtoCommercePricingModuleWebModelPrice> Prices { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommercePricingModuleWebModelProductPrice {\n");
            sb.Append("  ProductId: ").Append(ProductId).Append("\n");
            sb.Append("  ProductName: ").Append(ProductName).Append("\n");
            sb.Append("  Prices: ").Append(Prices).Append("\n");
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
            return this.Equals(obj as VirtoCommercePricingModuleWebModelProductPrice);
        }

        /// <summary>
        /// Returns true if VirtoCommercePricingModuleWebModelProductPrice instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommercePricingModuleWebModelProductPrice to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommercePricingModuleWebModelProductPrice other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ProductId == other.ProductId ||
                    this.ProductId != null &&
                    this.ProductId.Equals(other.ProductId)
                ) && 
                (
                    this.ProductName == other.ProductName ||
                    this.ProductName != null &&
                    this.ProductName.Equals(other.ProductName)
                ) && 
                (
                    this.Prices == other.Prices ||
                    this.Prices != null &&
                    this.Prices.SequenceEqual(other.Prices)
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

                if (this.ProductId != null)
                    hash = hash * 59 + this.ProductId.GetHashCode();

                if (this.ProductName != null)
                    hash = hash * 59 + this.ProductName.GetHashCode();

                if (this.Prices != null)
                    hash = hash * 59 + this.Prices.GetHashCode();

                return hash;
            }
        }

    }
}
