using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue : ValueObject, ICloneable
    {
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }

        public string Locale { get; set; }
        public object Value { get; set; }


        public string ValueId { get; set; }
        public DynamicPropertyValueType ValueType { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Value != null ? Value.ToString().Truncate(50) : "n/a");
        }
        #region ICloneable members
        public object Clone()
        {
            var result = MemberwiseClone() as DynamicPropertyObjectValue;
            if (Value is ICloneable cloneableValue)
            {
                result.Value = cloneableValue.Clone();
            }
            return result;
        }
        #endregion
    }
}
