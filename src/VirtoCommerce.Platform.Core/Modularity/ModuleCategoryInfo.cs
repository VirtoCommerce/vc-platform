using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Modularity;

public class ModuleCategoryInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public List<string> Modules { get; set; } = new List<string>();
}
