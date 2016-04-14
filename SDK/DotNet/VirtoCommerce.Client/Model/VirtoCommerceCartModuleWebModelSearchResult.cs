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
    public partial class VirtoCommerceCartModuleWebModelSearchResult :  IEquatable<VirtoCommerceCartModuleWebModelSearchResult>
    {
        /// <summary>
        /// Gets or sets the value of search result total shopping cart count
        /// </summary>
        /// <value>Gets or sets the value of search result total shopping cart count</value>
        [DataMember(Name="totalCount", EmitDefaultValue=false)]
        public int? TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the collection of search result shopping carts
        /// </summary>
        /// <value>Gets or sets the collection of search result shopping carts</value>
        [DataMember(Name="shopingCarts", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelShoppingCart> ShopingCarts { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCartModuleWebModelSearchResult {\n");
            sb.Append("  TotalCount: ").Append(TotalCount).Append("\n");
            sb.Append("  ShopingCarts: ").Append(ShopingCarts).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelSearchResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelSearchResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelSearchResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelSearchResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.TotalCount == other.TotalCount ||
                    this.TotalCount != null &&
                    this.TotalCount.Equals(other.TotalCount)
                ) && 
                (
                    this.ShopingCarts == other.ShopingCarts ||
                    this.ShopingCarts != null &&
                    this.ShopingCarts.SequenceEqual(other.ShopingCarts)
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

                if (this.TotalCount != null)
                    hash = hash * 59 + this.TotalCount.GetHashCode();

                if (this.ShopingCarts != null)
                    hash = hash * 59 + this.ShopingCarts.GetHashCode();

                return hash;
            }
        }

    }
}
