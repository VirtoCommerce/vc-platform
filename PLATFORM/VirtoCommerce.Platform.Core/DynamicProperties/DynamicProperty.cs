namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public bool IsArray { get; set; }
        public DynamicPropertyValueType ValueType { get; set; }
        public DynamicPropertyValue[] Values { get; set; }
        public DynamicPropertyName[] LocalizedNames { get; set; }
    }
}
