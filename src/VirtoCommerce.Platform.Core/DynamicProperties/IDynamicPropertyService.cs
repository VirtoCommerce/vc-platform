using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    /// <summary>
    /// Represent runtime object extension functionality based on properties meta-information 
    /// </summary>
    public interface IDynamicPropertyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<DynamicProperty[]> GetDynamicPropertiesAsync(string[] ids);
        /// <summary>
        /// Update or create dynamic properties 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        Task<DynamicProperty[]> SaveDynamicPropertiesAsync(DynamicProperty[] properties);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyIds"></param>
        /// <returns></returns>
        Task DeleteDynamicPropertiesAsync(string[] propertyIds);
    }
}
