using System;
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
        if (id == null)
        {
            return null;
        }

        var entities = await crudService.GetAsync(new[] { id }, responseGroup, clone);

        return entities?.FirstOrDefault();
    }

    [Obsolete("Use GetAsync() or GetNoCloneAsync()")]
    public static Task<IList<TModel>> GetByIdsAsync<TModel>(this ICrudService<TModel> crudService, IList<string> ids, string responseGroup = null, bool clone = true)
        where TModel : Entity
    {
        return crudService.GetAsync(ids, responseGroup, clone);
    }

    /// <summary>
    /// Returns data from the database without using cache.
    /// </summary>
    public static Task<IList<TModel>> GetNoCacheAsync<TModel>(this ICrudService<TModel> crudService, IList<string> ids, string responseGroup = null, bool clone = true)
        where TModel : Entity
    {
        return crudService.AsNoCache().GetNoCacheAsync(ids, responseGroup, clone);
    }

    /// <summary>
    /// Returns data from the database without using cache.
    /// </summary>
    public static async Task<TModel> GetNoCacheAsync<TModel>(this ICrudService<TModel> crudService, string id, string responseGroup = null, bool clone = true)
        where TModel : Entity
    {
        if (id is null)
        {
            return null;
        }

        var entities = await crudService.AsNoCache().GetNoCacheAsync([id], responseGroup, clone);

        return entities?.FirstOrDefault();
    }

    public static INoCacheCrudService<TModel> AsNoCache<TModel>(this ICrudService<TModel> crudService)
        where TModel : Entity
    {
        if (crudService is not INoCacheCrudService<TModel> noCacheService)
        {
            throw new NotSupportedException("Underlying service does not support no cache search.");
        }

        return noCacheService;
    }
}
