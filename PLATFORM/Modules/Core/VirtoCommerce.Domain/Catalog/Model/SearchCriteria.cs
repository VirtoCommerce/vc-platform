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
        public string CategoryId { get; set; }
        public string CatalogId { get; set; }
		public string Code { get; set; }
		public string SeoKeyword { get; set; }

        public List<PropertyValue> PropertyValues { get; set; }

        public int Start { get; set; }

        public int Count { get; set; }

		/// <summary>
		/// All products have index date less that specified
		/// </summary>
		public DateTime? IndexDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [get all categories]. Needed not to filter root categories only
        /// Flat structure is returned and tree constructed by using ParentId in frontend sitemap
        /// </summary>
        /// <value>
        ///   <c>true</c> if [get all categories]; otherwise, <c>false</c>.
        /// </value>
        public bool GetAllCategories { get; set; }

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
				GetAllCategories.ToString(), 
				PropertyValues != null ? string.Join(";", PropertyValues.Select(x=>x.ToString())) : null
			};
			return string.Join("-", parts.Where(x=>!String.IsNullOrEmpty(x)).ToArray());
		}
    }
}
