using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IHasDynamicProperties
    {
        ICollection<DynamicProperty> DynamicProperties { get; set; }
    }
}
