using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Property is metainformation record about what additional information merchandising item can be characterized. It's unheritable and can be defined in catalog, category, product or variation level.
    /// </summary>
    public class Property
    {
		public Property()
		{
       
		}
		/// <summary>
		/// Create property meta information from property value
		/// </summary>
		/// <param name="propValue"></param>
		/// <param name="catalogId"></param>
		/// <param name="categoryId"></param>
		/// <param name="propertyType"></param>
		public Property(PropertyValue propValue, string catalogId, string categoryId, coreModel.PropertyType propertyType)
		{
			Id = propValue.Id;
			CatalogId = catalogId;
			IsManageable = false;
			Name = propValue.PropertyName;
			Type = propertyType;
			ValueType = propValue.ValueType;
			Values = new List<PropertyValue>();
		}

        /// <summary>
        /// Gets or sets a value indicating whether user can change property value.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
		public bool IsReadOnly { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether user can change property metadata or remove this property. 
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is manageable; otherwise, <c>false</c>.
        /// </value>
		public bool IsManageable { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is new. A new property should be created on server site instead of trying to update it.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is new; otherwise, <c>false</c>.
        /// </value>
		public bool IsNew { get; set; }
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the catalog id that this product belongs to.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
        public string CatalogId { get; set; }
        /// <summary>
        /// Gets or sets the catalog that this product belongs to.
        /// </summary>
        /// <value>
        /// The catalog.
        /// </value>
        public Catalog Catalog { get; set; }
        /// <summary>
        /// Gets or sets the category id that this product belongs to.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public string CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the category that this product belongs to.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public Category Category { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Property"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Property"/> is dictionary.
        /// </summary>
        /// <value>
        ///   <c>true</c> if dictionary; otherwise, <c>false</c>.
        /// </value>
        public bool Dictionary { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Property"/> supports multiple values.
        /// </summary>
        /// <value>
        ///   <c>true</c> if multivalue; otherwise, <c>false</c>.
        /// </value>
        public bool Multivalue { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Property"/> is multilingual.
        /// </summary>
        /// <value>
        ///   <c>true</c> if multilanguage; otherwise, <c>false</c>.
        /// </value>
        public bool Multilanguage { get; set; }
        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
		[JsonConverter(typeof(StringEnumConverter))]
        public coreModel.PropertyValueType ValueType { get; set; }
        /// <summary>
        /// Gets or sets the type of object this property is applied to.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
		[JsonConverter(typeof(StringEnumConverter))]
        public coreModel.PropertyType Type { get; set; }
        /// <summary>
        /// Gets or sets the current property value. Collection is used as a general placeholder to store both single and multi-value values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
		public ICollection<PropertyValue> Values { get; set; }
        /// <summary>
        /// Gets or sets the dictionary values.
        /// </summary>
        /// <value>
        /// The dictionary values.
        /// </value>
		public ICollection<PropertyDictionaryValue> DictionaryValues { get; set; }
        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public ICollection<PropertyAttribute> Attributes { get; set; }
        /// <summary>
        /// Gets or sets the display names.
        /// </summary>
        /// <value>
        /// The display names.
        /// </value>
		public ICollection<PropertyDisplayName> DisplayNames { get; set; }

        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        public bool IsInherited { get; set; }
    }
}