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
    public partial class VirtoCommerceCartModuleWebModelShippingMethod :  IEquatable<VirtoCommerceCartModuleWebModelShippingMethod>
    {
        /// <summary>
        /// Gets or sets the value of shipping method code
        /// </summary>
        /// <value>Gets or sets the value of shipping method code</value>
        [DataMember(Name="shipmentMethodCode", EmitDefaultValue=false)]
        public string ShipmentMethodCode { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method name
        /// </summary>
        /// <value>Gets or sets the value of shipping method name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option name
        /// </summary>
        /// <value>Gets or sets the value of shipping method option name</value>
        [DataMember(Name="optionName", EmitDefaultValue=false)]
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method option description
        /// </summary>
        /// <value>Gets or sets the value of shipping method option description</value>
        [DataMember(Name="optionDescription", EmitDefaultValue=false)]
        public string OptionDescription { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method logo absolute URL
        /// </summary>
        /// <value>Gets or sets the value of shipping method logo absolute URL</value>
        [DataMember(Name="logoUrl", EmitDefaultValue=false)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method tax type
        /// </summary>
        /// <value>Gets or sets the value of shipping method tax type</value>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method currency
        /// </summary>
        /// <value>Gets or sets the value of shipping method currency</value>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method price
        /// </summary>
        /// <value>Gets or sets the value of shipping method price</value>
        [DataMember(Name="price", EmitDefaultValue=false)]
        public double? Price { get; set; }

        /// <summary>
        /// Gets or sets the collection of shipping method discounts
        /// </summary>
        /// <value>Gets or sets the collection of shipping method discounts</value>
        [DataMember(Name="discounts", EmitDefaultValue=false)]
        public List<VirtoCommerceCartModuleWebModelDiscount> Discounts { get; set; }

        /// <summary>
        /// Gets or Sets Settings
        /// </summary>
        [DataMember(Name="settings", EmitDefaultValue=false)]
        public List<VirtoCommercePlatformCoreSettingsSettingEntry> Settings { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCartModuleWebModelShippingMethod {\n");
            sb.Append("  ShipmentMethodCode: ").Append(ShipmentMethodCode).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  OptionName: ").Append(OptionName).Append("\n");
            sb.Append("  OptionDescription: ").Append(OptionDescription).Append("\n");
            sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
            sb.Append("  Discounts: ").Append(Discounts).Append("\n");
            sb.Append("  Settings: ").Append(Settings).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCartModuleWebModelShippingMethod);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCartModuleWebModelShippingMethod instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCartModuleWebModelShippingMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCartModuleWebModelShippingMethod other)
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
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.OptionName == other.OptionName ||
                    this.OptionName != null &&
                    this.OptionName.Equals(other.OptionName)
                ) && 
                (
                    this.OptionDescription == other.OptionDescription ||
                    this.OptionDescription != null &&
                    this.OptionDescription.Equals(other.OptionDescription)
                ) && 
                (
                    this.LogoUrl == other.LogoUrl ||
                    this.LogoUrl != null &&
                    this.LogoUrl.Equals(other.LogoUrl)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
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
                ) && 
                (
                    this.Discounts == other.Discounts ||
                    this.Discounts != null &&
                    this.Discounts.SequenceEqual(other.Discounts)
                ) && 
                (
                    this.Settings == other.Settings ||
                    this.Settings != null &&
                    this.Settings.SequenceEqual(other.Settings)
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

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.OptionName != null)
                    hash = hash * 59 + this.OptionName.GetHashCode();

                if (this.OptionDescription != null)
                    hash = hash * 59 + this.OptionDescription.GetHashCode();

                if (this.LogoUrl != null)
                    hash = hash * 59 + this.LogoUrl.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Price != null)
                    hash = hash * 59 + this.Price.GetHashCode();

                if (this.Discounts != null)
                    hash = hash * 59 + this.Discounts.GetHashCode();

                if (this.Settings != null)
                    hash = hash * 59 + this.Settings.GetHashCode();

                return hash;
            }
        }

    }
}
