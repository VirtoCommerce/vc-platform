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
    public partial class VirtoCommerceQuoteModuleWebModelShipmentMethod :  IEquatable<VirtoCommerceQuoteModuleWebModelShipmentMethod>
    {
        /// <summary>
        /// Gets or Sets ShipmentMethodCode
        /// </summary>
        [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or Sets OptionName
        /// </summary>
        [DataMember(Name="optionName", EmitDefaultValue=false)]
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets LogoUrl
        /// </summary>
        [DataMember(Name="logoUrl", EmitDefaultValue=false)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets Price
        /// </summary>
        [DataMember(Name="price", EmitDefaultValue=false)]
        public double? Price { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceQuoteModuleWebModelShipmentMethod {\n");
            sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
            sb.Append("  OptionName: ").Append(OptionName).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
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
            return this.Equals(obj as VirtoCommerceQuoteModuleWebModelShipmentMethod);
        }

        /// <summary>
        /// Returns true if VirtoCommerceQuoteModuleWebModelShipmentMethod instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceQuoteModuleWebModelShipmentMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceQuoteModuleWebModelShipmentMethod other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.ShipmentMethodCode == other.ShipmentMethodCode ||
                    this.ShipmentMethodCode != null &&
                    this.ShipmentMethodCode.Equals(other.ShipmentMethodCode)
                ) && 
                (
                    this.OptionName == other.OptionName ||
                    this.OptionName != null &&
                    this.OptionName.Equals(other.OptionName)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.LogoUrl == other.LogoUrl ||
                    this.LogoUrl != null &&
                    this.LogoUrl.Equals(other.LogoUrl)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Price == other.Price ||
                    this.Price != null &&
                    this.Price.Equals(other.Price)
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

                if (this.ShipmentMethodCode != null)
                    hash = hash * 59 + this.ShipmentMethodCode.GetHashCode();

                if (this.OptionName != null)
                    hash = hash * 59 + this.OptionName.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.LogoUrl != null)
                    hash = hash * 59 + this.LogoUrl.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Price != null)
                    hash = hash * 59 + this.Price.GetHashCode();

                return hash;
            }
        }

    }
}
