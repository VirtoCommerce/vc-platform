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
    public partial class VirtoCommerceDomainTaxModelTaxEvaluationContext :  IEquatable<VirtoCommerceDomainTaxModelTaxEvaluationContext>
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets Customer
        /// </summary>
        [DataMember(Name="customer", EmitDefaultValue=false)]
        public VirtoCommerceDomainCustomerModelContact Customer { get; set; }

        /// <summary>
        /// Gets or Sets Organization
        /// </summary>
        [DataMember(Name="organization", EmitDefaultValue=false)]
        public VirtoCommerceDomainCustomerModelOrganization Organization { get; set; }

        /// <summary>
        /// Gets or Sets Address
        /// </summary>
        [DataMember(Name="address", EmitDefaultValue=false)]
        public VirtoCommerceDomainCommerceModelAddress Address { get; set; }

        /// <summary>
        /// Gets or Sets Currency
        /// </summary>
        [DataMember(Name="currency", EmitDefaultValue=false)]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or Sets Lines
        /// </summary>
        [DataMember(Name="lines", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainTaxModelTaxLine> Lines { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceDomainTaxModelTaxEvaluationContext {\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Customer: ").Append(Customer).Append("\n");
            sb.Append("  Organization: ").Append(Organization).Append("\n");
            sb.Append("  Address: ").Append(Address).Append("\n");
            sb.Append("  Currency: ").Append(Currency).Append("\n");
            sb.Append("  Lines: ").Append(Lines).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainTaxModelTaxEvaluationContext);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainTaxModelTaxEvaluationContext instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainTaxModelTaxEvaluationContext to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainTaxModelTaxEvaluationContext other)
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
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Customer == other.Customer ||
                    this.Customer != null &&
                    this.Customer.Equals(other.Customer)
                ) && 
                (
                    this.Organization == other.Organization ||
                    this.Organization != null &&
                    this.Organization.Equals(other.Organization)
                ) && 
                (
                    this.Address == other.Address ||
                    this.Address != null &&
                    this.Address.Equals(other.Address)
                ) && 
                (
                    this.Currency == other.Currency ||
                    this.Currency != null &&
                    this.Currency.Equals(other.Currency)
                ) && 
                (
                    this.Lines == other.Lines ||
                    this.Lines != null &&
                    this.Lines.SequenceEqual(other.Lines)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
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

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Customer != null)
                    hash = hash * 59 + this.Customer.GetHashCode();

                if (this.Organization != null)
                    hash = hash * 59 + this.Organization.GetHashCode();

                if (this.Address != null)
                    hash = hash * 59 + this.Address.GetHashCode();

                if (this.Currency != null)
                    hash = hash * 59 + this.Currency.GetHashCode();

                if (this.Lines != null)
                    hash = hash * 59 + this.Lines.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
