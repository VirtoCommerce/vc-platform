using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security;

public class TwoFactorPolicyContext
{
    public ApplicationUser User { get; set; }

    public string GrantType { get; set; }

    public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}
