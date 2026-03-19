using System.Collections.Generic;
using System.Security.Claims;

namespace VirtoCommerce.Platform.Core.DeveloperTools;

public interface IDeveloperToolRegistrar
{
    void RegisterDeveloperTool(DeveloperToolDescriptor tool);
    IList<DeveloperToolDescriptor> GetRegisteredTools(ClaimsPrincipal claimsPrincipal);
}
