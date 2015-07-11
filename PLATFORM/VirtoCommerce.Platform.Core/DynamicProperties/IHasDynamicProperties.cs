using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IHasDynamicProperties
    {
        ICollection<DynamicPropertyObjectValue> DynamicPropertyValues { get; set; }
    }
}
