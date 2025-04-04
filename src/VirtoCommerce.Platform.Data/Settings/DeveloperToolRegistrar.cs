using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings;

public class DeveloperToolRegistrar : IDeveloperToolRegistrar
{
    private readonly List<DeveloperToolDescriptor> _tools = new();

    public void RegisterDeveloperTool(DeveloperToolDescriptor tool, bool replace = false)
    {
        if (tool == null)
        {
            throw new ArgumentNullException(nameof(tool));
        }
        if (_tools.Any(t => t.Name.Equals(tool.Name, StringComparison.OrdinalIgnoreCase)))
        {
            if (replace)
            {
                _tools.RemoveAll(t => t.Name.Equals(tool.Name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                throw new InvalidOperationException($"A developer tool with the name '{tool.Name}' is already registered. Use 'replace' parameter to override it.");
            }
        }
        _tools.Add(tool);
    }
    public IList<DeveloperToolDescriptor> GetRegisteredTools()
    {
        return _tools.ToList();
    }
}
