using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud
{
    /// <summary>
    /// Generic interface to use with CRUD services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICrudService<T>
        where T : Entity
    {
        /// <summary>
        /// Returns a list of model instances for specified IDs.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="responseGroup"></param>
        /// <param name="clone">If false, returns data from the cache without cloning. This consumes less memory, but the returned data must not be modified.</param>
        /// <returns></returns>
        Task<IList<T>> GetAsync(IList<string> ids, string responseGroup = null, bool clone = true);

        Task SaveChangesAsync(IList<T> models);
        Task DeleteAsync(IList<string> ids, bool softDelete = false);
    }
}
