using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicObjectProperty : DynamicProperty
    {
        public string ObjectId { get; set; }
        public ICollection<DynamicPropertyObjectValue> Values { get; set; }

        public virtual void SetMetaData(DynamicProperty metadata)
        {
            if (!IsTransient() && Id != metadata.Id)
            {
                throw new OperationCanceledException("unable to set meta data, it is already set and is different from passed");
            }

            Id = metadata.Id;
            ValueType = metadata.ValueType;
            IsArray = metadata.IsArray;
            IsDictionary = metadata.IsDictionary;
            IsMultilingual = metadata.IsMultilingual;
            IsRequired = metadata.IsRequired;
        }

        public override object Clone()
        {
            var result = base.Clone() as DynamicObjectProperty;
            if (Values != null)
            {
                result.Values = Values.Select(x => x.Clone() as DynamicPropertyObjectValue).ToArray();
            }
            return result;
        }

        public override string ToString()
        {
            var retVal = base.ToString();
            if (Values != null)
            {
                retVal += string.Format("[{0}]", Values.Count);
            }
            return retVal;
        }
    }
}
