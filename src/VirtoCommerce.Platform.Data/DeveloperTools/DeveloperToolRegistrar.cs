using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
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

    public IList<DeveloperToolDescriptor> GetRegisteredTools(ClaimsPrincipal claimsPrincipal)
    {
        ArgumentNullException.ThrowIfNull(claimsPrincipal);
        var isAdmin = claimsPrincipal.IsInRole(PlatformConstants.Security.SystemRoles.Administrator);
        return _tools.Where(x => isAdmin
                                 || x.Permission.IsNullOrEmpty()
                                 || claimsPrincipal.HasClaim(PlatformConstants.Security.Claims.PermissionClaimType, x.Permission))
            .OrderBy(x => x.SortOrder).ToList();
    }
}
