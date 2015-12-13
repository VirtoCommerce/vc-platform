using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchCriteria
    {
        public CatalogSearchCriteria()
        {
            ResponseGroup = CatalogSearchResponseGroup.WithProducts;
            PageNumber = 1;
            PageSize = 20;
        }

        public CatalogSearchResponseGroup ResponseGroup { get; set; }
        public string CatalogId { get; set; }
        public string CategoryId { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
        public string Keyword { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Term[] Terms { get; set; }
    }
}
