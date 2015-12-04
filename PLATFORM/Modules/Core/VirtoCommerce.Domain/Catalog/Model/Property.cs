using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class Property : AuditableEntity
	{
		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }
		public string CategoryId { get; set; }
		public Category Category { get; set; }
		public string Name { get; set; }
		public bool Required { get; set; }
		public bool Dictionary { get; set; }
		public bool Multivalue { get; set; }
		public bool Multilanguage { get; set; }
		public PropertyValueType ValueType { get; set; }
		public PropertyType Type { get; set; }
		public ICollection<PropertyAttribute> Attributes { get; set; }
		public ICollection<PropertyDictionaryValue> DictionaryValues { get; set; }
		public ICollection<PropertyDisplayName> DisplayNames { get; set; }


        /// <summary>
        /// Because we not have a direct link between prop values and properties we should
        /// find property value meta information by comparing key properties like name and value type.
        /// </summary>
        /// <param name="propValue"></param>
        /// <returns></returns>
        public bool IsSuitableForValue(PropertyValue propValue)
        {
            return String.Equals(Name, propValue.PropertyName, StringComparison.InvariantCultureIgnoreCase) && ValueType == propValue.ValueType;
        }
    }
}