using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.GenericCrud
{
    public interface ICrudService<T> where T : Entity
    {
        Task<T[]> GetByIdsAsync(string[] ids, string responseGroup = null);
        Task<T> GetByIdAsync(string id, string responseGroup = null);
        Task SaveChangesAsync(T[] models);
        Task DeleteAsync(string[] ids, bool softDelete = false);
    }
}
