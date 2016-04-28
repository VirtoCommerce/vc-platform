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
    /// Property is metainformation record about what additional information merchandising item can be characterized. It&#39;s unheritable and can be defined in catalog, category, product or variation level.
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceCatalogModuleWebModelProperty :  IEquatable<VirtoCommerceCatalogModuleWebModelProperty>
    {
        /// <summary>
        /// Gets or sets a value indicating whether user can change property value.
        /// </summary>
        /// <value>Gets or sets a value indicating whether user can change property value.</value>
        [DataMember(Name="isReadOnly", EmitDefaultValue=false)]
        public bool? IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can change property metadata or remove this property.
        /// </summary>
        /// <value>Gets or sets a value indicating whether user can change property metadata or remove this property.</value>
        [DataMember(Name="isManageable", EmitDefaultValue=false)]
        public bool? IsManageable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is new. A new property should be created on server site instead of trying to update it.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this instance is new. A new property should be created on server site instead of trying to update it.</value>
        [DataMember(Name="isNew", EmitDefaultValue=false)]
        public bool? IsNew { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id", EmitDefaultValue=false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the catalog id that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog id that this product belongs to.</value>
        [DataMember(Name="catalogId", EmitDefaultValue=false)]
        public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the catalog that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the catalog that this product belongs to.</value>
        [DataMember(Name="catalog", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCatalog Catalog { get; set; }

        /// <summary>
        /// Gets or sets the category id that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the category id that this product belongs to.</value>
        [DataMember(Name="categoryId", EmitDefaultValue=false)]
        public string CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category that this product belongs to.
        /// </summary>
        /// <value>Gets or sets the category that this product belongs to.</value>
        [DataMember(Name="category", EmitDefaultValue=false)]
        public VirtoCommerceCatalogModuleWebModelCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>Gets or sets the name.</value>
        [DataMember(Name="name", EmitDefaultValue=false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is required.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is required.</value>
        [DataMember(Name="required", EmitDefaultValue=false)]
        public bool? Required { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is dictionary.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is dictionary.</value>
        [DataMember(Name="dictionary", EmitDefaultValue=false)]
        public bool? Dictionary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} supports multiple values.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} supports multiple values.</value>
        [DataMember(Name="multivalue", EmitDefaultValue=false)]
        public bool? Multivalue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is multilingual.
        /// </summary>
        /// <value>Gets or sets a value indicating whether this {VirtoCommerce.CatalogModule.Web.Model.Property} is multilingual.</value>
        [DataMember(Name="multilanguage", EmitDefaultValue=false)]
        public bool? Multilanguage { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>Gets or sets the type of the value.</value>
        [DataMember(Name="valueType", EmitDefaultValue=false)]
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or sets the type of object this property is applied to.
        /// </summary>
        /// <value>Gets or sets the type of object this property is applied to.</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the current property value. Collection is used as a general placeholder to store both single and multi-value values.
        /// </summary>
        /// <value>Gets or sets the current property value. Collection is used as a general placeholder to store both single and multi-value values.</value>
        [DataMember(Name="values", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelPropertyValue> Values { get; set; }

        /// <summary>
        /// Gets or sets the dictionary values.
        /// </summary>
        /// <value>Gets or sets the dictionary values.</value>
        [DataMember(Name="dictionaryValues", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelPropertyDictionaryValue> DictionaryValues { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>Gets or sets the attributes.</value>
        [DataMember(Name="attributes", EmitDefaultValue=false)]
        public List<VirtoCommerceCatalogModuleWebModelPropertyAttribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the display names.
        /// </summary>
        /// <value>Gets or sets the display names.</value>
        [DataMember(Name="displayNames", EmitDefaultValue=false)]
        public List<VirtoCommerceDomainCatalogModelPropertyDisplayName> DisplayNames { get; set; }

        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        /// <value>System flag used to mark that object was inherited from other</value>
        [DataMember(Name="isInherited", EmitDefaultValue=false)]
        public bool? IsInherited { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceCatalogModuleWebModelProperty {\n");
            sb.Append("  IsReadOnly: ").Append(IsReadOnly).Append("\n");
            sb.Append("  IsManageable: ").Append(IsManageable).Append("\n");
            sb.Append("  IsNew: ").Append(IsNew).Append("\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  CatalogId: ").Append(CatalogId).Append("\n");
            sb.Append("  Catalog: ").Append(Catalog).Append("\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Required: ").Append(Required).Append("\n");
            sb.Append("  Dictionary: ").Append(Dictionary).Append("\n");
            sb.Append("  Multivalue: ").Append(Multivalue).Append("\n");
            sb.Append("  Multilanguage: ").Append(Multilanguage).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Values: ").Append(Values).Append("\n");
            sb.Append("  DictionaryValues: ").Append(DictionaryValues).Append("\n");
            sb.Append("  Attributes: ").Append(Attributes).Append("\n");
            sb.Append("  DisplayNames: ").Append(DisplayNames).Append("\n");
            sb.Append("  IsInherited: ").Append(IsInherited).Append("\n");
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
            return this.Equals(obj as VirtoCommerceCatalogModuleWebModelProperty);
        }

        /// <summary>
        /// Returns true if VirtoCommerceCatalogModuleWebModelProperty instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceCatalogModuleWebModelProperty to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceCatalogModuleWebModelProperty other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.IsReadOnly == other.IsReadOnly ||
                    this.IsReadOnly != null &&
                    this.IsReadOnly.Equals(other.IsReadOnly)
                ) && 
                (
                    this.IsManageable == other.IsManageable ||
                    this.IsManageable != null &&
                    this.IsManageable.Equals(other.IsManageable)
                ) && 
                (
                    this.IsNew == other.IsNew ||
                    this.IsNew != null &&
                    this.IsNew.Equals(other.IsNew)
                ) && 
                (
                    this.Id == other.Id ||
                    this.Id != null &&
                    this.Id.Equals(other.Id)
                ) && 
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
                    this.CategoryId == other.CategoryId ||
                    this.CategoryId != null &&
                    this.CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    this.Category == other.Category ||
                    this.Category != null &&
                    this.Category.Equals(other.Category)
                ) && 
                (
                    this.Name == other.Name ||
                    this.Name != null &&
                    this.Name.Equals(other.Name)
                ) && 
                (
                    this.Required == other.Required ||
                    this.Required != null &&
                    this.Required.Equals(other.Required)
                ) && 
                (
                    this.Dictionary == other.Dictionary ||
                    this.Dictionary != null &&
                    this.Dictionary.Equals(other.Dictionary)
                ) && 
                (
                    this.Multivalue == other.Multivalue ||
                    this.Multivalue != null &&
                    this.Multivalue.Equals(other.Multivalue)
                ) && 
                (
                    this.Multilanguage == other.Multilanguage ||
                    this.Multilanguage != null &&
                    this.Multilanguage.Equals(other.Multilanguage)
                ) && 
                (
                    this.ValueType == other.ValueType ||
                    this.ValueType != null &&
                    this.ValueType.Equals(other.ValueType)
                ) && 
                (
                    this.Type == other.Type ||
                    this.Type != null &&
                    this.Type.Equals(other.Type)
                ) && 
                (
                    this.Values == other.Values ||
                    this.Values != null &&
                    this.Values.SequenceEqual(other.Values)
                ) && 
                (
                    this.DictionaryValues == other.DictionaryValues ||
                    this.DictionaryValues != null &&
                    this.DictionaryValues.SequenceEqual(other.DictionaryValues)
                ) && 
                (
                    this.Attributes == other.Attributes ||
                    this.Attributes != null &&
                    this.Attributes.SequenceEqual(other.Attributes)
                ) && 
                (
                    this.DisplayNames == other.DisplayNames ||
                    this.DisplayNames != null &&
                    this.DisplayNames.SequenceEqual(other.DisplayNames)
                ) && 
                (
                    this.IsInherited == other.IsInherited ||
                    this.IsInherited != null &&
                    this.IsInherited.Equals(other.IsInherited)
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

                if (this.IsReadOnly != null)
                    hash = hash * 59 + this.IsReadOnly.GetHashCode();

                if (this.IsManageable != null)
                    hash = hash * 59 + this.IsManageable.GetHashCode();

                if (this.IsNew != null)
                    hash = hash * 59 + this.IsNew.GetHashCode();

                if (this.Id != null)
                    hash = hash * 59 + this.Id.GetHashCode();

                if (this.CatalogId != null)
                    hash = hash * 59 + this.CatalogId.GetHashCode();

                if (this.Catalog != null)
                    hash = hash * 59 + this.Catalog.GetHashCode();

                if (this.CategoryId != null)
                    hash = hash * 59 + this.CategoryId.GetHashCode();

                if (this.Category != null)
                    hash = hash * 59 + this.Category.GetHashCode();

                if (this.Name != null)
                    hash = hash * 59 + this.Name.GetHashCode();

                if (this.Required != null)
                    hash = hash * 59 + this.Required.GetHashCode();

                if (this.Dictionary != null)
                    hash = hash * 59 + this.Dictionary.GetHashCode();

                if (this.Multivalue != null)
                    hash = hash * 59 + this.Multivalue.GetHashCode();

                if (this.Multilanguage != null)
                    hash = hash * 59 + this.Multilanguage.GetHashCode();

                if (this.ValueType != null)
                    hash = hash * 59 + this.ValueType.GetHashCode();

                if (this.Type != null)
                    hash = hash * 59 + this.Type.GetHashCode();

                if (this.Values != null)
                    hash = hash * 59 + this.Values.GetHashCode();

                if (this.DictionaryValues != null)
                    hash = hash * 59 + this.DictionaryValues.GetHashCode();

                if (this.Attributes != null)
                    hash = hash * 59 + this.Attributes.GetHashCode();

                if (this.DisplayNames != null)
                    hash = hash * 59 + this.DisplayNames.GetHashCode();

                if (this.IsInherited != null)
                    hash = hash * 59 + this.IsInherited.GetHashCode();

                return hash;
            }
        }

    }
}
