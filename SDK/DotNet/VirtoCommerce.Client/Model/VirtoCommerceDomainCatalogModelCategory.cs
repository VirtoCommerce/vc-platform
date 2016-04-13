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
    public partial class VirtoCommerceDomainCatalogModelCategory :  IEquatable<VirtoCommerceDomainCatalogModelCategory>
    {
        /// <summary>
        /// Gets or Sets CatalogId
        /// </summary>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or Sets Catalog
        /// </summary>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceDomainCatalogModelCatalog Catalog { get; set; }

        /// <summary>
        /// Gets or Sets ParentId
        /// </summary>
        [DataMember(Name="parentId", EmitDefaultValue=false)]
        public string ParentId { get; set; }

        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// Gets or Sets TaxType
        /// </summary>
        [DataMember(Name="taxType", EmitDefaultValue=false)]
        public string TaxType { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Path
        /// </summary>
        [DataMember(Name="path", EmitDefaultValue=false)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or Sets IsVirtual
        /// </summary>
        [DataMember(Name="isVirtual", EmitDefaultValue=false)]
        public bool? IsVirtual { get; set; }

        /// <summary>
        /// Gets or Sets Level
        /// </summary>
        [DataMember(Name="level", EmitDefaultValue=false)]
        public int? Level { get; set; }

        /// <summary>
        /// Gets or Sets Parents
        /// </summary>
        [DataMember(Name="parents", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCategory> Parents { get; set; }

        /// <summary>
        /// Gets or Sets Priority
        /// </summary>
        [DataMember(Name="priority", EmitDefaultValue=false)]
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or Sets IsActive
        /// </summary>
        [DataMember(Name="isActive", EmitDefaultValue=false)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or Sets Children
        /// </summary>
        [DataMember(Name="children", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCategory> Children { get; set; }

        /// <summary>
        /// Gets or Sets Properties
        /// </summary>
        [DataMember(Name="properties", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelProperty> Properties { get; set; }

        /// <summary>
        /// Gets or Sets PropertyValues
        /// </summary>
        [DataMember(Name="propertyValues", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyValue> PropertyValues { get; set; }

        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name="links", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelCategoryLink> Links { get; set; }

        /// <summary>
        /// Gets or Sets SeoObjectType
        /// </summary>
        [DataMember(Name="seoObjectType", EmitDefaultValue=false)]
        public string SeoObjectType { get; private set; }

        /// <summary>
        /// Gets or Sets SeoInfos
        /// </summary>
        [DataMember(Name="seoInfos", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCommerceModelSeoInfo> SeoInfos { get; set; }

        /// <summary>
        /// Gets or Sets Images
        /// </summary>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelImage> Images { get; set; }

        /// <summary>
        /// Gets or Sets Outlines
        /// </summary>
        [DataMember(Name="outlines", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelOutline> Outlines { get; set; }

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
            sb.Append("class VirtoCommerceDomainCatalogModelCategory {\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  TaxType: ").Append(TaxType).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Path: ").Append(Path).Append("\n");
            sb.Append("  IsVirtual: ").Append(IsVirtual).Append("\n");
            sb.Append("  Level: ").Append(Level).Append("\n");
            sb.Append("  Parents: ").Append(Parents).Append("\n");
            sb.Append("  Priority: ").Append(Priority).Append("\n");
            sb.Append("  IsActive: ").Append(IsActive).Append("\n");
            sb.Append("  Children: ").Append(Children).Append("\n");
            sb.Append("  Properties: ").Append(Properties).Append("\n");
            sb.Append("  PropertyValues: ").Append(PropertyValues).Append("\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
            sb.Append("  SeoObjectType: ").Append(SeoObjectType).Append("\n");
            sb.Append("  SeoInfos: ").Append(SeoInfos).Append("\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  Outlines: ").Append(Outlines).Append("\n");
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
            return this.Equals(obj as VirtoCommerceDomainCatalogModelCategory);
        }

        /// <summary>
        /// Returns true if VirtoCommerceDomainCatalogModelCategory instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceDomainCatalogModelCategory to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceDomainCatalogModelCategory other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.CatalogId == other.CatalogId ||
                    this.CatalogId != null &&
                    this.CatalogId.Equals(other.CatalogId)
                ) && 
                (
                    this.Catalog == other.Catalog ||
                    this.Catalog != null &&
                    this.Catalog.Equals(other.Catalog)
                ) && 
                (
                    this.ParentId == other.ParentId ||
                    this.ParentId != null &&
                    this.ParentId.Equals(other.ParentId)
                ) && 
                (
                    this.Code == other.Code ||
                    this.Code != null &&
                    this.Code.Equals(other.Code)
                ) && 
                (
                    this.TaxType == other.TaxType ||
                    this.TaxType != null &&
                    this.TaxType.Equals(other.TaxType)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Path == other.Path ||
                    this.Path != null &&
                    this.Path.Equals(other.Path)
                ) && 
                (
                    this.IsVirtual == other.IsVirtual ||
                    this.IsVirtual != null &&
                    this.IsVirtual.Equals(other.IsVirtual)
                ) && 
                (
                    this.Level == other.Level ||
                    this.Level != null &&
                    this.Level.Equals(other.Level)
                ) && 
                (
                    this.Parents == other.Parents ||
                    this.Parents != null &&
                    this.Parents.SequenceEqual(other.Parents)
                ) && 
                (
                    this.Priority == other.Priority ||
                    this.Priority != null &&
                    this.Priority.Equals(other.Priority)
                ) && 
                (
                    this.IsActive == other.IsActive ||
                    this.IsActive != null &&
                    this.IsActive.Equals(other.IsActive)
                ) && 
                (
                    this.Children == other.Children ||
                    this.Children != null &&
                    this.Children.SequenceEqual(other.Children)
                ) && 
                (
                    this.Properties == other.Properties ||
                    this.Properties != null &&
                    this.Properties.SequenceEqual(other.Properties)
                ) && 
                (
                    this.PropertyValues == other.PropertyValues ||
                    this.PropertyValues != null &&
                    this.PropertyValues.SequenceEqual(other.PropertyValues)
                ) && 
                (
                    this.Links == other.Links ||
                    this.Links != null &&
                    this.Links.SequenceEqual(other.Links)
                ) && 
                (
                    this.SeoObjectType == other.SeoObjectType ||
                    this.SeoObjectType != null &&
                    this.SeoObjectType.Equals(other.SeoObjectType)
                ) && 
                (
                    this.SeoInfos == other.SeoInfos ||
                    this.SeoInfos != null &&
                    this.SeoInfos.SequenceEqual(other.SeoInfos)
                ) && 
                (
                    this.Images == other.Images ||
                    this.Images != null &&
                    this.Images.SequenceEqual(other.Images)
                ) && 
                (
                    this.Outlines == other.Outlines ||
                    this.Outlines != null &&
                    this.Outlines.SequenceEqual(other.Outlines)
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

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.ParentId != null)
                    hash = hash * 59 + this.ParentId.GetHashCode();

                if (this.Code != null)
                    hash = hash * 59 + this.Code.GetHashCode();

                if (this.TaxType != null)
                    hash = hash * 59 + this.TaxType.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Path != null)
                    hash = hash * 59 + this.Path.GetHashCode();

                if (this.IsVirtual != null)
                    hash = hash * 59 + this.IsVirtual.GetHashCode();

                if (this.Level != null)
                    hash = hash * 59 + this.Level.GetHashCode();

                if (this.Parents != null)
                    hash = hash * 59 + this.Parents.GetHashCode();

                if (this.Priority != null)
                    hash = hash * 59 + this.Priority.GetHashCode();

                if (this.IsActive != null)
                    hash = hash * 59 + this.IsActive.GetHashCode();

                if (this.Children != null)
                    hash = hash * 59 + this.Children.GetHashCode();

                if (this.Properties != null)
                    hash = hash * 59 + this.Properties.GetHashCode();

                if (this.PropertyValues != null)
                    hash = hash * 59 + this.PropertyValues.GetHashCode();

                if (this.Links != null)
                    hash = hash * 59 + this.Links.GetHashCode();

                if (this.SeoObjectType != null)
                    hash = hash * 59 + this.SeoObjectType.GetHashCode();

                if (this.SeoInfos != null)
                    hash = hash * 59 + this.SeoInfos.GetHashCode();

                if (this.Images != null)
                    hash = hash * 59 + this.Images.GetHashCode();

                if (this.Outlines != null)
                    hash = hash * 59 + this.Outlines.GetHashCode();

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
