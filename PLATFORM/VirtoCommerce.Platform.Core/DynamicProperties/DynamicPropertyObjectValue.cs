using System;
using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue : ICloneable
    {
        public string Locale { get; set; }
        public object Value { get; set; }

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
