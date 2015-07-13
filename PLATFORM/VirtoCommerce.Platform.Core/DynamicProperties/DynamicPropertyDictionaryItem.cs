namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public class DynamicPropertyDictionaryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DynamicPropertyDictionaryItemName[] DisplayNames { get; set; }
    }
}
