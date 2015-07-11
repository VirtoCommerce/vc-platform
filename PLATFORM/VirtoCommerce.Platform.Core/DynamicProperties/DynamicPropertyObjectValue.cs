namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyObjectValue
    {
        public DynamicProperty Property { get; set; }
        public string ObjectId { get; set; }
        public string Locale { get; set; }
        public string[] Values { get; set; }
    }
}
