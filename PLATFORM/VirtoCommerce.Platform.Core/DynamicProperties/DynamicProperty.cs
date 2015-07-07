namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicProperty
    {
        public string Name { get; set; }
        public string ObjectType { get; set; }
        public string ObjectId { get; set; }
        public DynamicPropertyValueType ValueType { get; set; }

        public string Value { get; set; }
        public bool IsArray { get; set; }
        public string[] ArrayValues { get; set; }

        public bool IsLocaleDependent { get; set; }
        public DynamicPropertyName[] LocalizedNames { get; set; }
        public DynamicPropertyValue[] LocalizedValues { get; set; }
    }
}
