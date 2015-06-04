using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Web.Model
{
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
		/// Can user change value
		/// </summary>
		public bool IsReadOnly { get; set; }
		/// <summary>
		/// Can user change property metdata and remove property
		/// </summary>
		public bool IsManageable { get; set; }
		public bool IsNew { get; set; }
        public string Id { get; set; }
        public string CatalogId { get; set; }
        public Catalog Catalog { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public bool Required { get; set; }
        public bool Dictionary { get; set; }
        public bool Multivalue { get; set; }
        public bool Multilanguage { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
        public coreModel.PropertyValueType ValueType { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
        public coreModel.PropertyType Type { get; set; }
		public List<PropertyValue> Values { get; set; }
		public List<PropertyDictionaryValue> DictionaryValues { get; set; }
        public List<PropertyAttribute> Attributes { get; set; }

		/// <summary>
		/// Because we not have a direct link beetwen prop values and properties we should
		/// find property value meta information throught comparing key properties like a property name and value type
		/// </summary>
		/// <param name="propValue"></param>
		/// <returns></returns>
		public bool IsSuitableForValue(PropertyValue propValue)
		{
			return String.Equals(Name, propValue.PropertyName, StringComparison.InvariantCultureIgnoreCase) && ValueType == propValue.ValueType;
		}
    }
}