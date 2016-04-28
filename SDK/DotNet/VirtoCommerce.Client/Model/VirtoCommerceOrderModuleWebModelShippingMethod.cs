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
    public partial class VirtoCommerceOrderModuleWebModelShippingMethod :  IEquatable<VirtoCommerceOrderModuleWebModelShippingMethod>
    {
        /// <summary>
        /// Code used for link shipment with external carrier service implementation (FedEx, USPS etc)
        /// </summary>
        /// <value>Code used for link shipment with external carrier service implementation (FedEx, USPS etc)</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Method name (system name)
        /// </summary>
        /// <value>Method name (system name)</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of shipping method name
        /// </summary>
        /// <value>Gets or sets the value of shipping method name</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Describe some shipment options (Vip, Air, Moment etc)
        /// </summary>
        /// <value>Describe some shipment options (Vip, Air, Moment etc)</value>
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
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelShippingMethod {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  OptionName: ").Append(OptionName).Append("\n");
            sb.Append("  OptionDescription: ").Append(OptionDescription).Append("\n");
            sb.Append("  LogoUrl: ").Append(LogoUrl).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelShippingMethod);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelShippingMethod instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelShippingMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelShippingMethod other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
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

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Description != null)
                    hash = hash * 59 + this.Description.GetHashCode();

                if (this.OptionName != null)
                    hash = hash * 59 + this.OptionName.GetHashCode();

                if (this.OptionDescription != null)
                    hash = hash * 59 + this.OptionDescription.GetHashCode();

                if (this.LogoUrl != null)
                    hash = hash * 59 + this.LogoUrl.GetHashCode();

                return hash;
            }
        }

    }
}
