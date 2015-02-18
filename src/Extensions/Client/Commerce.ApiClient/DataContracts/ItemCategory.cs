namespace VirtoCommerce.Web.Core.DataContracts
{
    public class ItemCategory
    {
        #region Public Properties

        public string CatalogId { get; set; }

        public string CategoryId { get; set; }

        public ItemCategory VirtualCategories { get; set; }

        #endregion
    }
}
