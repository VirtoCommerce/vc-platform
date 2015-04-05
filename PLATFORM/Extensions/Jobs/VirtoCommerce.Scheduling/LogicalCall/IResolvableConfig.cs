using System.Collections.Generic;

namespace VirtoCommerce.Scheduling.LogicalCall
{
    public interface IResolvableConfig
    {
        void Initialize(Dictionary<string, string> properties);
    }
}