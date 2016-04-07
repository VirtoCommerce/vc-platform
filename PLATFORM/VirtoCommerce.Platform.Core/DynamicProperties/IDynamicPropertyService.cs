using System;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    /// <summary>
    /// Represent runtime object extension functionality based on properties meta-information 
    /// </summary>
    public interface IDynamicPropertyService
    {
        /// <summary>
        /// Register new type name which can support dynamic properties
        /// </summary>
        /// <param name="typeName"></param>
        void RegisterType(string typeName);
        /// <summary>
        /// Return all available types names supported dynamic properties
        /// </summary>
        /// <returns></returns>
        string[] GetAvailableObjectTypeNames();
        /// <summary>
        /// Return string type name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetObjectTypeName(Type type);

        /// <summary>
        /// Return all dynamic properties defined for type
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        DynamicProperty[] GetProperties(string objectType);
        /// <summary>
        /// Update or create dynamic properties 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        DynamicProperty[] SaveProperties(DynamicProperty[] properties);
        void DeleteProperties(string[] propertyIds);

        DynamicPropertyDictionaryItem[] GetDictionaryItems(string propertyId);
        void SaveDictionaryItems(string propertyId, DynamicPropertyDictionaryItem[] items);
        void DeleteDictionaryItems(string[] itemIds);

        /// <summary>
        /// Deep loads and populate dynamic properties values for object
        /// </summary>
        /// <param name="owner"></param>
        void LoadDynamicPropertyValues(IHasDynamicProperties owner);
        /// <summary>
        /// Deep save dynamic properties values for object
        /// </summary>
        /// <param name="owner"></param>
        void SaveDynamicPropertyValues(IHasDynamicProperties owner);
        void DeleteDynamicPropertyValues(IHasDynamicProperties owner);
    }
}
