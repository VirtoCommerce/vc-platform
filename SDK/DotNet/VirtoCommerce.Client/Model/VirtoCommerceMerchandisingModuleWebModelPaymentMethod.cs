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
    public class VirtoCommerceMerchandisingModuleWebModelPaymentMethod : IEquatable<VirtoCommerceMerchandisingModuleWebModelPaymentMethod>
    {
        
        /// <summary>
        /// Gets or sets the value of payment gateway code
        /// </summary>
        /// <value>Gets or sets the value of payment gateway code</value>
        [DataMember(Name="gatewayCode", EmitDefaultValue=false)]
        public string GatewayCode { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method name
        /// </summary>
        /// <value>Gets or sets the value of payment method name</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method logo absolute URL
        /// </summary>
        /// <value>Gets or sets the value of payment method logo absolute URL</value>
        [DataMember(Name="iconUrl", EmitDefaultValue=false)]
        public string IconUrl { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method description
        /// </summary>
        /// <value>Gets or sets the value of payment method description</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method type
        /// </summary>
        /// <value>Gets or sets the value of payment method type</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method group type
        /// </summary>
        /// <value>Gets or sets the value of payment method group type</value>
        [DataMember(Name="group", EmitDefaultValue=false)]
        public string Group { get; set; }
  
        
        /// <summary>
        /// Gets or sets the value of payment method priority
        /// </summary>
        /// <value>Gets or sets the value of payment method priority</value>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }
  
        
  
        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceMerchandisingModuleWebModelPaymentMethod {\n");
            sb.Append("  GatewayCode: ").Append(GatewayCode).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  IconUrl: ").Append(IconUrl).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Group: ").Append(Group).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            
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
            return this.Equals(obj as VirtoCommerceMerchandisingModuleWebModelPaymentMethod);
        }

        /// <summary>
        /// Returns true if VirtoCommerceMerchandisingModuleWebModelPaymentMethod instances are equal
        /// </summary>
        /// <param name="obj">Instance of VirtoCommerceMerchandisingModuleWebModelPaymentMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceMerchandisingModuleWebModelPaymentMethod other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.GatewayCode == other.GatewayCode ||
                    this.GatewayCode != null &&
                    this.GatewayCode.Equals(other.GatewayCode)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.IconUrl == other.IconUrl ||
                    this.IconUrl != null &&
                    this.IconUrl.Equals(other.IconUrl)
                ) && 
                (
                    this.Description == other.Description ||
                    this.Description != null &&
                    this.Description.Equals(other.Description)
                ) && 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Group == other.Group ||
                    this.Group != null &&
                    this.Group.Equals(other.Group)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
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
                
                if (this.GatewayCode != null)
                    hash = hash * 57 + this.GatewayCode.GetHashCode();
                
                if (this.Name != null)
                    hash = hash * 57 + this.Name.GetHashCode();
                
                if (this.IconUrl != null)
                    hash = hash * 57 + this.IconUrl.GetHashCode();
                
                if (this.Description != null)
                    hash = hash * 57 + this.Description.GetHashCode();
                
                if (this.Type != null)
                    hash = hash * 57 + this.Type.GetHashCode();
                
                if (this.Group != null)
                    hash = hash * 57 + this.Group.GetHashCode();
                
                if (this.Priority != null)
                    hash = hash * 57 + this.Priority.GetHashCode();
                
                return hash;
            }
        }

    }


}
