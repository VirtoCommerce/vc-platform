using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface ISupportSecurityScopes
    {
        IEnumerable<string> Scopes { get; set; }
    }
}
