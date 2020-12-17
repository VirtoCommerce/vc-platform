using System;

namespace VirtoCommerce.Platform.Core.ObjectValue
{
    public class ObjectValueToStringAttribute : Attribute
    {
        public ObjectValueToStringAttribute(params string[] names)
        {
            Names = names;
        }

        public string[] Names { get; set; }
    }
}
