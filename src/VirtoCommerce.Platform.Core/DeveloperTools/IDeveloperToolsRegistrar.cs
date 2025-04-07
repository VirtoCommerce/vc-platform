using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.DeveloperTools;

public interface IDeveloperToolRegistrar
{
    void RegisterDeveloperTool(DeveloperToolDescriptor tool);
    IList<DeveloperToolDescriptor> GetRegisteredTools();
}
