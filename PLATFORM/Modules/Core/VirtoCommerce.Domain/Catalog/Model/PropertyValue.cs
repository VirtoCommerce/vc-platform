using System;
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