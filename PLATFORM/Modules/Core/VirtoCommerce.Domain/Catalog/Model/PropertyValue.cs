using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Domain.Catalog.Model
{
	public class PropertyValue : AuditableEntity, ILanguageSupport, IInheritable, ICloneable
    {
		public string PropertyId { get; set; }
		public string PropertyName { get; set; }
		public Property Property { get; set; }
		public string Alias { get; set; }
		public string ValueId { get; set; }
		public object Value { get; set; }
		public PropertyValueType ValueType { get; set; }
		public string LanguageCode { get; set; }

		public override string ToString()
		{
			return (PropertyName ?? "unknown") + ":" + (Value ?? "undefined");
		}

        /// <summary>
        /// Returns for current value all dictionary values in all defined languages 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PropertyValue> TryGetAllLocalizedDictValues()
        {
            var retVal = new List<PropertyValue>();
           
            if (Property != null && Property.Dictionary && Property.Multilanguage && Property.DictionaryValues != null)
            {
                foreach (var dictValue in Property.DictionaryValues.Where(x => x.Alias == Alias))
                {
                    var langDictPropValue = this.Clone() as PropertyValue;
                    langDictPropValue.Id = null;
                    langDictPropValue.LanguageCode = dictValue.LanguageCode;
                    langDictPropValue.Value = dictValue.Value;
                    langDictPropValue.ValueId = dictValue.Id;
                    retVal.Add(langDictPropValue);
                }
            }
            return retVal;
        }

        #region IInheritable Members
        public bool IsInherited { get; set; }
        #endregion

        #region ICloneable members
        public object Clone()
        {
            var retVal = new PropertyValue();
            retVal.Id = Id;
            retVal.CreatedBy = CreatedBy;
            retVal.CreatedDate = CreatedDate;
            retVal.ModifiedBy = ModifiedBy;
            retVal.ModifiedDate = ModifiedDate;

            retVal.PropertyId = PropertyId;
            retVal.Property = Property != null ? Property.Clone() as Property : null;
            retVal.PropertyName = PropertyName;
            retVal.Alias = Alias;
            retVal.ValueId = ValueId;
            retVal.Value = Value;
            retVal.ValueType = ValueType;
            retVal.IsInherited = IsInherited;
           
            return retVal;
        }
        #endregion
    }
}