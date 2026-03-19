using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud;

/// <summary>
/// Generic interface for outer entities to use with CRUD services.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOuterEntityService<T> : ICrudService<T>
    where T : Entity, IHasOuterId
{
    /// <summary>
    /// Returns a list of model instances for specified outer IDs (integration key)
    /// </summary>
    /// <param name="outerIds">List of outer IDs</param>
    /// <param name="responseGroup"></param>
    /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
    /// <returns></returns>
    Task<IList<T>> GetByOuterIdsAsync(IList<string> outerIds, string responseGroup = null, bool clone = true);
}
