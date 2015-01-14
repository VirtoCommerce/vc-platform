namespace VirtoCommerce.Web.Core.DataContracts
{
    public class ItemCategory
    {
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }

        public ItemCategory VirtualCategories { get; set; }
    }
}
