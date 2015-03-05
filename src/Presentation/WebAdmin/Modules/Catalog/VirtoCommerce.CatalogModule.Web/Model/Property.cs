using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class Property
    {
      
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
        public PropertyValueType ValueType { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
        public PropertyType Type { get; set; }
		public List<PropertyValue> Values { get; set; }
		public List<PropertyDictionaryValue> DictionaryValues { get; set; }
        public List<PropertyAttribute> Attributes { get; set; }
    }
}