using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Settings;

public interface IDeveloperToolRegistrar
{
    void RegisterDeveloperTool(DeveloperToolDescriptor tool, bool replace = false);
    IList<DeveloperToolDescriptor> GetRegisteredTools();
}
