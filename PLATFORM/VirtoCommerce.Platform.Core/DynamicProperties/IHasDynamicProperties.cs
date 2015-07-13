using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IHasDynamicProperties
    {
        string Id { get; }
        ICollection<DynamicPropertyObjectValue> DynamicPropertyValues { get; set; }
    }
}
