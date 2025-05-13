using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.Platform.Core.Common;

public static class CrudServiceExtensions
{
    /// <summary>
    /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
    /// </summary>
    public static Task<IList<TModel>> GetNoCloneAsync<TModel>(this ICrudService<TModel> crudService, IList<string> ids, string responseGroup = null)
        where TModel : Entity
    {
        return crudService.GetAsync(ids, responseGroup, clone: false);
    }

    /// <summary>
    /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
    /// </summary>
    public static Task<TModel> GetNoCloneAsync<TModel>(this ICrudService<TModel> crudService, string id, string responseGroup = null)
        where TModel : Entity
    {
        return crudService.GetByIdAsync(id, responseGroup, clone: false);
    }

    public static async Task<TModel> GetByIdAsync<TModel>(this ICrudService<TModel> crudService, string id, string responseGroup = null, bool clone = true)
        where TModel : Entity
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }

        var entities = await crudService.GetAsync([id], responseGroup, clone);

        return entities?.FirstOrDefault();
    }

    /// <summary>
    /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
    /// </summary>
    public static Task<IList<TModel>> GetByOuterIdsNoCloneAsync<TModel>(this IOuterEntityService<TModel> outerEntityService, IList<string> outerIds, string responseGroup = null)
        where TModel : Entity, IHasOuterId
    {
        return outerEntityService.GetByOuterIdsAsync(outerIds, responseGroup, clone: false);
    }

    /// <summary>
    /// Returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.
    /// </summary>
    public static async Task<TModel> GetByOuterIdNoCloneAsync<TModel>(this IOuterEntityService<TModel> outerEntityService, string outerId, string responseGroup = null)
        where TModel : Entity, IHasOuterId
    {
        if (string.IsNullOrEmpty(outerId))
        {
            return null;
        }

        var entities = await outerEntityService.GetByOuterIdsAsync([outerId], responseGroup, clone: false);

        return entities?.FirstOrDefault();
    }
}
