using System;
using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class Property : AuditableEntity, IInheritable, ICloneable
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

        #region IInheritable Members
        public bool IsInherited { get; set; }
        #endregion

            #region ICloneable members
        public object Clone()
        {
            var retVal = new Property();
            retVal.Id = Id;
            retVal.CreatedBy = CreatedBy;
            retVal.CreatedDate = CreatedDate;
            retVal.ModifiedBy = ModifiedBy;
            retVal.ModifiedDate = ModifiedDate;

            retVal.CatalogId = CatalogId;
            retVal.Catalog = Catalog;
            retVal.CategoryId = CategoryId;
            retVal.Category = Category;
            retVal.Name = Name;
            retVal.Required = Required;
            retVal.Dictionary = Dictionary;
            retVal.Multivalue = Multivalue;
            retVal.Multilanguage = Multilanguage;
            retVal.ValueType = ValueType;

            retVal.Type = Type;
            retVal.ValueType = ValueType;
            retVal.Attributes = Attributes;
            retVal.DictionaryValues = DictionaryValues;
            retVal.DisplayNames = DisplayNames;

            retVal.IsInherited = IsInherited;

            return retVal;
        }
        #endregion
    }
}