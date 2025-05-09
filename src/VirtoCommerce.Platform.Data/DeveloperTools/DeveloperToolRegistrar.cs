using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.DeveloperTools;
using static VirtoCommerce.Platform.Core.PlatformConstants.Security.Claims;

namespace VirtoCommerce.Platform.Data.DeveloperTools;

public class DeveloperToolRegistrar : IDeveloperToolRegistrar
{
    private readonly List<DeveloperToolDescriptor> _tools = [];

    public void RegisterDeveloperTool(DeveloperToolDescriptor tool)
    {
        ArgumentNullException.ThrowIfNull(tool);
        _tools.Add(tool);
    }

    public IList<DeveloperToolDescriptor> GetRegisteredTools(ClaimsPrincipal claimsPrincipal)
    {
        var isAdmin = claimsPrincipal != null && claimsPrincipal.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);

        return _tools
            .Where(x =>
                isAdmin ||
                string.IsNullOrEmpty(x.Permission) ||
                claimsPrincipal != null && claimsPrincipal.HasClaim(PermissionClaimType, x.Permission))
            .OrderBy(x => x.SortOrder)
            .ToList();
    }
}
