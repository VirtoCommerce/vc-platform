using System;
using System.Linq;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            //ResponseGroup = ItemResponseGroup.ItemMedium;
            Outline = string.Empty;
            LanguageCode = "en-US";
            Currency = "USD";
            Take = 20;
        }

        public string StoreId { get; set; }
        public SearchResponseGroup ResponseGroup { get; set; }
        public string Keyword { get; set; }

        /// <summary>
        /// Search  in all children categories for specified catalog or categories
        /// </summary>
        public bool SearchInChildren { get; set; }

        public string CategoryId { get; set; }

        private string[] _categoriesIds;
        public string[] CategoriesIds
        {
            get
            {
                if (_categoriesIds == null && !string.IsNullOrEmpty(CategoryId))
                {
                    _categoriesIds = new[] { CategoryId };
                }
                return _categoriesIds;
            }
            set
            {
                _categoriesIds = value;
            }

        }

        public string CatalogId { get; set; }

        private string[] _catalogIds;
        public string[] CatalogsIds
        {
            get
            {
                if (_catalogIds == null && !string.IsNullOrEmpty(CatalogId))
                {
                    _catalogIds = new[] { CatalogId };
                }
                return _catalogIds;
            }
            set
            {
                _catalogIds = value;
            }

        }
        public string LanguageCode { get; set; }

        /// <summary>
        /// Product ore category code
        /// </summary>
        public string Code { get; set; }

        public string Sort { get; set; }
        public string SortOrder { get; set; }

        //Hides direct linked categories in virtual category displayed only linked category content without itself
        public bool HideDirectLinkedCategories { get; set; }
        /// <summary>
        /// For filtration by specified properties values
        /// </summary>
        public PropertyValue[] PropertyValues { get; set; }

        public string Currency { get; set; }
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        /// <summary>
        /// All products have index date less that specified
        /// </summary>
        public DateTime? IndexDate { get; set; }

        public string PricelistId { get; set; }

        private string[] _pricelistsIds;
        public string[] PricelistIds
        {
            get
            {
                if (_pricelistsIds == null && !string.IsNullOrEmpty(PricelistId))
                {
                    _pricelistsIds = new[] { PricelistId };
                }
                return _pricelistsIds;
            }
            set
            {
                _pricelistsIds = value;
            }
        }

        /// <summary>
        /// Gets or sets search terms collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Terms { get; set; }

        /// <summary>
        /// Gets or sets the facets collection
        /// Item format: name:value1,value2,value3
        /// </summary>
        public string[] Facets { get; set; }

        /// <summary>
        /// Category1/Category2
        /// </summary>
        public string Outline { get; set; }

        public DateTime? StartDateFrom { get; set; }

        public override string ToString()
        {
            var parts = new[]
            {
                StoreId,
                ((int)ResponseGroup).ToString(),
                Keyword,
                CategoryId,
                CatalogId,
                Skip.ToString(),
                Take.ToString(),
                SearchInChildren.ToString(),
                PropertyValues != null ? string.Join(";", PropertyValues.Select(x=>x.ToString())) : null
            };
            return string.Join("-", parts.Where(x => !string.IsNullOrEmpty(x)).ToArray());
        }
    }
}
