using System;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue : ICloneable
    {
        public string Locale { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", Value != null ? Value.ToString().Truncate(50) : "n/a");
        }
        public object Clone()
        {
            var retVal = new DynamicPropertyObjectValue
            {
                Locale = Locale,
                Value = Value
            };
            var cloneableValue = Value as ICloneable;
            if(cloneableValue != null )
            {
                retVal.Value = cloneableValue.Clone();
            }
            return retVal;
        }
    }
}
