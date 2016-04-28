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
    public partial class VirtoCommerceOrderModuleWebModelShipmentPackage :  IEquatable<VirtoCommerceOrderModuleWebModelShipmentPackage>
    {
        /// <summary>
        /// Gets or Sets BarCode
        /// </summary>
        [DataMember(Name="barCode", EmitDefaultValue=false)]
        public string BarCode { get; set; }

        /// <summary>
        /// Gets or Sets PackageType
        /// </summary>
        [DataMember(Name="packageType", EmitDefaultValue=false)]
        public string PackageType { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name="items", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelShipmentItem> Items { get; set; }

        /// <summary>
        /// Gets or Sets WeightUnit
        /// </summary>
        [DataMember(Name="weightUnit", EmitDefaultValue=false)]
        public string WeightUnit { get; set; }

        /// <summary>
        /// Gets or Sets Weight
        /// </summary>
        [DataMember(Name="weight", EmitDefaultValue=false)]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets or Sets MeasureUnit
        /// </summary>
        [DataMember(Name="measureUnit", EmitDefaultValue=false)]
        public string MeasureUnit { get; set; }

        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [DataMember(Name="height", EmitDefaultValue=false)]
        public double? Height { get; set; }

        /// <summary>
        /// Gets or Sets Length
        /// </summary>
        [DataMember(Name="length", EmitDefaultValue=false)]
        public double? Length { get; set; }

        /// <summary>
        /// Gets or Sets Width
        /// </summary>
        [DataMember(Name="width", EmitDefaultValue=false)]
        public double? Width { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        [DataMember(Name="createdDate", EmitDefaultValue=false)]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        [DataMember(Name="modifiedDate", EmitDefaultValue=false)]
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        [DataMember(Name="createdBy", EmitDefaultValue=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        [DataMember(Name="modifiedBy", EmitDefaultValue=false)]
        public string ModifiedBy { get; set; }

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
            sb.Append("class VirtoCommerceOrderModuleWebModelShipmentPackage {\n");
            sb.Append("  BarCode: ").Append(BarCode).Append("\n");
            sb.Append("  PackageType: ").Append(PackageType).Append("\n");
            sb.Append("  Items: ").Append(Items).Append("\n");
            sb.Append("  WeightUnit: ").Append(WeightUnit).Append("\n");
            sb.Append("  Weight: ").Append(Weight).Append("\n");
            sb.Append("  MeasureUnit: ").Append(MeasureUnit).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Length: ").Append(Length).Append("\n");
            sb.Append("  Width: ").Append(Width).Append("\n");
            sb.Append("  CreatedDate: ").Append(CreatedDate).Append("\n");
            sb.Append("  ModifiedDate: ").Append(ModifiedDate).Append("\n");
            sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
            sb.Append("  ModifiedBy: ").Append(ModifiedBy).Append("\n");
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
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelShipmentPackage);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelShipmentPackage instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelShipmentPackage to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelShipmentPackage other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.BarCode == other.BarCode ||
                    this.BarCode != null &&
                    this.BarCode.Equals(other.BarCode)
                ) && 
                (
                    this.PackageType == other.PackageType ||
                    this.PackageType != null &&
                    this.PackageType.Equals(other.PackageType)
                ) && 
                (
                    this.Items == other.Items ||
                    this.Items != null &&
                    this.Items.SequenceEqual(other.Items)
                ) && 
                (
                    this.WeightUnit == other.WeightUnit ||
                    this.WeightUnit != null &&
                    this.WeightUnit.Equals(other.WeightUnit)
                ) && 
                (
                    this.Weight == other.Weight ||
                    this.Weight != null &&
                    this.Weight.Equals(other.Weight)
                ) && 
                (
                    this.MeasureUnit == other.MeasureUnit ||
                    this.MeasureUnit != null &&
                    this.MeasureUnit.Equals(other.MeasureUnit)
                ) && 
                (
                    this.Height == other.Height ||
                    this.Height != null &&
                    this.Height.Equals(other.Height)
                ) && 
                (
                    this.Length == other.Length ||
                    this.Length != null &&
                    this.Length.Equals(other.Length)
                ) && 
                (
                    this.Width == other.Width ||
                    this.Width != null &&
                    this.Width.Equals(other.Width)
                ) && 
                (
                    this.CreatedDate == other.CreatedDate ||
                    this.CreatedDate != null &&
                    this.CreatedDate.Equals(other.CreatedDate)
                ) && 
                (
                    this.ModifiedDate == other.ModifiedDate ||
                    this.ModifiedDate != null &&
                    this.ModifiedDate.Equals(other.ModifiedDate)
                ) && 
                (
                    this.CreatedBy == other.CreatedBy ||
                    this.CreatedBy != null &&
                    this.CreatedBy.Equals(other.CreatedBy)
                ) && 
                (
                    this.ModifiedBy == other.ModifiedBy ||
                    this.ModifiedBy != null &&
                    this.ModifiedBy.Equals(other.ModifiedBy)
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

                if (this.BarCode != null)
                    hash = hash * 59 + this.BarCode.GetHashCode();

                if (this.PackageType != null)
                    hash = hash * 59 + this.PackageType.GetHashCode();

                if (this.Items != null)
                    hash = hash * 59 + this.Items.GetHashCode();

                if (this.WeightUnit != null)
                    hash = hash * 59 + this.WeightUnit.GetHashCode();

                if (this.Weight != null)
                    hash = hash * 59 + this.Weight.GetHashCode();

                if (this.MeasureUnit != null)
                    hash = hash * 59 + this.MeasureUnit.GetHashCode();

                if (this.Height != null)
                    hash = hash * 59 + this.Height.GetHashCode();

                if (this.Length != null)
                    hash = hash * 59 + this.Length.GetHashCode();

                if (this.Width != null)
                    hash = hash * 59 + this.Width.GetHashCode();

                if (this.CreatedDate != null)
                    hash = hash * 59 + this.CreatedDate.GetHashCode();

                if (this.ModifiedDate != null)
                    hash = hash * 59 + this.ModifiedDate.GetHashCode();

                if (this.CreatedBy != null)
                    hash = hash * 59 + this.CreatedBy.GetHashCode();

                if (this.ModifiedBy != null)
                    hash = hash * 59 + this.ModifiedBy.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                return hash;
            }
        }

    }
}
