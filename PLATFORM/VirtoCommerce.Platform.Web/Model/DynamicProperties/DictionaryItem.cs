namespace VirtoCommerce.Platform.Web.Model.DynamicProperties
{
    public class DictionaryItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DictionaryItemName[] DisplayNames { get; set; }
    }
}
