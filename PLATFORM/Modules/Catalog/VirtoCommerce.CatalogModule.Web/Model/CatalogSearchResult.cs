namespace VirtoCommerce.CatalogModule.Web.Model
{
    public class CatalogSearchResult
    {
        public int ProductsTotalCount { get; set; }
        public Product[] Products { get; set; }
        public Category[] Categories { get; set; }
        public Catalog[] Catalogs { get; set; }
        public Aggregation[] Aggregations { get; set; }
    }
}
