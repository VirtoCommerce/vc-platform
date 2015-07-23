using System;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyService
    {
        string[] GetAvailableObjectTypeNames();
        string GetObjectTypeName(Type type);

        DynamicProperty[] GetProperties(string objectType);
        void SaveProperties(DynamicProperty[] properties);
        void DeleteProperties(string[] propertyIds);

        DynamicPropertyDictionaryItem[] GetDictionaryItems(string propertyId);
        void SaveDictionaryItems(string propertyId, DynamicPropertyDictionaryItem[] items);
        void DeleteDictionaryItems(string[] itemIds);

        void LoadDynamicPropertyValues(IHasDynamicProperties owner);
        void SaveDynamicPropertyValues(IHasDynamicProperties owner);
        void DeleteDynamicPropertyValues(IHasDynamicProperties owner);
    }
}
