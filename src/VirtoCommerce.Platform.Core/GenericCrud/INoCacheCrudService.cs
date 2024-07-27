using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud;

/// <summary>
/// Generic interface to use with CRUD services without using cache.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface INoCacheCrudService<T>
    where T : Entity
{
    /// <summary>
    /// Returns a list of model instances for specified IDs without using cache.
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="responseGroup"></param>
    /// <param name="clone">If false, returns data without cloning. This consumes less memory, but the returned data must not be modified.</param>
    /// <returns></returns>
    Task<IList<T>> GetNoCacheAsync(IList<string> ids, string responseGroup = null, bool clone = true);
}
