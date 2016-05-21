using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Security
{
    public interface ISecurityOptions
    {
        IEnumerable<string> NonEditableUsers { get; }
    }
}
