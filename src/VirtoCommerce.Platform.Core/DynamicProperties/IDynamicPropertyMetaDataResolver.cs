using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    /// <summary>
    /// the interface contains the methods to resolve dynamic property meta-data by passed parameters (name, objectType, other)
    /// </summary>
    public interface IDynamicPropertyMetaDataResolver
    {
        Task<DynamicProperty> GetByNameAsync(string objectType, string propertyName);
    }
}
