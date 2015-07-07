namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyService
    {
        string[] GetObjectTypes();
        DynamicProperty[] GetProperties(string objectType);
        void SaveProperties(DynamicProperty[] properties);
        void DeleteProperties(string[] propertyIds);
        //void DeleteProperties(string objectType);

        DynamicProperty[] GetObjectProperties(string objectType, string objectId);
        void SaveObjectProperties(DynamicProperty[] properties);
        void DeleteObjectValues(string objectType, string objectId);
    }
}
