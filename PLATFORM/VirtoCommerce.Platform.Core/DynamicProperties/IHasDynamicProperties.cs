using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IHasDynamicProperties
    {
        string Id { get; }
		string ObjectType { get; set; }
		ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }
}
