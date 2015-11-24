using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            PropertyValues = new List<PropertyValue>();
            Count = 20;
        }
        public ResponseGroup ResponseGroup { get; set; }
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
                if (_categoriesIds == null && !String.IsNullOrEmpty(CategoryId))
                {
                    _categoriesIds = new string[] { CategoryId };
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
                if(_catalogIds == null && !String.IsNullOrEmpty(CatalogId))
                {
                    _catalogIds = new string[] { CatalogId };
                }
                return _catalogIds;
            }
            set
            {
                _catalogIds = value;
            }

        }
        public string LanguageCode { get; set; }
        public string Currency { get; set; }
        /// <summary>
        /// Product ore category code
        /// </summary>
		public string Code { get; set; }
	
        public string Sort { get; set; }
        public string[] Facets { get; set; }
		//Hides direct linked categories in virtual category displayed only linked category content without itself
		public bool HideDirectLinkedCategories { get; set; }
        public List<PropertyValue> PropertyValues { get; set; }

        public int Start { get; set; }

        public int Count { get; set; }

		/// <summary>
		/// All products have index date less that specified
		/// </summary>
		public DateTime? IndexDate { get; set; }

    
		public override string ToString()
		{
			var parts = new string[]
			{
				ResponseGroup.GetHashCode().ToString(), 
				Keyword, 
				CategoryId, 
				CatalogId, 
				Start.ToString(), 
				Count.ToString(), 
				SearchInChildren.ToString(), 
				PropertyValues != null ? string.Join(";", PropertyValues.Select(x=>x.ToString())) : null
			};
			return string.Join("-", parts.Where(x=>!String.IsNullOrEmpty(x)).ToArray());
		}
    }
}
