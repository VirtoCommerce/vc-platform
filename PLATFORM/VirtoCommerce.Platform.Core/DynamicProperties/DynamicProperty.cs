namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public bool IsArray { get; set; }
        public bool IsDictionary { get; set; }
        public DynamicPropertyValueType ValueType { get; set; }

        public DynamicPropertyName[] DisplayNames { get; set; }
        public DynamicPropertyDictionaryItem[] DictionaryItems { get; set; }
        public DynamicPropertyObjectValue[] ObjectValues { get; set; }
    }
}
