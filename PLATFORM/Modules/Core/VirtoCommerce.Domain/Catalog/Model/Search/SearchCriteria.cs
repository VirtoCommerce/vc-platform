using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

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
        /// <summary>
        /// Also search in variations 
        /// </summary>
        public bool SearchInVariations { get; set; }

        public string CategoryId { get; set; }

        private string[] _categoriesIds;
        public string[] CategoryIds
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
        public string[] CatalogIds
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
        /// Product or category code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Sorting expression property1:asc;property2:desc
        /// </summary>
        public string Sort { get; set; }

        public SortInfo[] SortInfos
        {
            get
            {
                return SortInfo.Parse(Sort).ToArray();
            }
        }

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

        /// <summary>
        /// Search also in hidden categories and products
        /// </summary>
        public bool WithHidden { get; set; }

        /// <summary>
        /// Search only buyable products
        /// </summary>
        public bool? OnlyBuyable { get; set; }

        /// <summary>
        /// Search only inventory tracking products 
        /// </summary>
        public bool? OnlyWithTrackingInventory { get; set; }

        /// <summary>
        /// Search product with specified type
        /// </summary>
        public string ProductType { get; set; }

        /// <summary>
        /// Search product with specified types
        /// </summary>
        private string[] _productTypes;
        public string[] ProductTypes
        {
            get
            {
                if (_productTypes == null && !string.IsNullOrEmpty(ProductType))
                {
                    _productTypes = new[] { ProductType };
                }
                return _productTypes;
            }
            set
            {
                _productTypes = value;
            }
        }

        public DateTime? StartDateFrom { get; set; }

        public void Normalize()
        {
            Keyword = Keyword.EmptyToNull();
            Sort = Sort.EmptyToNull();
            CatalogId = CatalogId.EmptyToNull();
            CategoryId = CategoryId.EmptyToNull();
            Sort = Sort.EmptyToNull();
            Sort = Sort.EmptyToNull();

            if (!string.IsNullOrEmpty(Keyword))
            {
                Keyword = Keyword.EscapeSearchTerm();
            }

            if (PricelistIds != null)
            {
                PricelistIds = PricelistIds.Where(id => id != null).ToArray();

                if (!PricelistIds.Any())
                {
                    PricelistIds = null;
                }
            }
        }

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
