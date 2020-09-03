using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue : ValueObject
    {
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public string Locale { get; set; }
        public object Value { get; set; }

        public string ValueId { get; set; }
        public DynamicPropertyValueType ValueType { get; set; }

        public string PropertyId { get; set; }
        public string PropertyName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Value != null ? Value.ToString().Truncate(50) : "n/a");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PropertyName;
            yield return ObjectId;
            yield return ObjectType;
            yield return ValueType;
            yield return ValueId;
            yield return Value;
            yield return Locale;
        }

        #region ICloneable members
        public override object Clone()
        {
            var result = base.Clone() as DynamicPropertyObjectValue;
            if (Value is ICloneable cloneableValue)
            {
                result.Value = cloneableValue.Clone();
            }
            return result;
        }
        #endregion
    }
}
