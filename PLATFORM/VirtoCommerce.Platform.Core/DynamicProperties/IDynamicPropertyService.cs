namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyService
    {
        string[] GetObjectTypes();
        DynamicProperty[] GetProperties(string objectType);
        DynamicPropertyDictionaryItem[] GetDictionaryItems(string propertyId);
        void SaveProperties(DynamicProperty[] properties);
        void DeleteProperties(string[] propertyIds);

        DynamicProperty[] GetObjectValues(string objectType, string objectId);
        void SaveObjectValues(DynamicProperty[] properties);
        void DeleteObjectValues(string objectType, string objectId);
    }
}
