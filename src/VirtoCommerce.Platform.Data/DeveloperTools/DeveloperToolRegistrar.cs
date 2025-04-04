using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.DeveloperTools;

namespace VirtoCommerce.Platform.Data.DeveloperTools;

public class DeveloperToolRegistrar : IDeveloperToolRegistrar
{
    private readonly List<DeveloperToolDescriptor> _tools = [];

    public void RegisterDeveloperTool(DeveloperToolDescriptor tool)
    {
        ArgumentNullException.ThrowIfNull(tool);
        _tools.Add(tool);
    }

    public IList<DeveloperToolDescriptor> GetRegisteredTools()
    {
        return _tools.OrderBy(x => x.SortOrder).ToList();
    }
}
