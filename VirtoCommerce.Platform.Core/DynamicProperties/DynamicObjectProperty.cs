using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicObjectProperty : DynamicProperty
    {
        public string ObjectId { get; set; }
        public ICollection<DynamicPropertyObjectValue> Values { get; set; }

        public override DynamicProperty Clone()
        {
            var result = base.Clone() as DynamicObjectProperty;
            if (Values != null)
            {
                result.Values = Values.Select(x => x.Clone() as DynamicPropertyObjectValue).ToList();
            }
            return result;
        }

        public override string ToString()
        {
            var retVal = base.ToString();
            if (Values != null)
            {
                retVal += string.Format("[{0}]", Values.Count());
            }
            return retVal;
        }
    }
}
