using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Common;

public interface IProtectedStaticPathsSource
{
    /// <summary>
    /// Request-path prefixes excluded from anonymous static file serving.
    /// </summary>
    IEnumerable<string> GetPaths();
}
