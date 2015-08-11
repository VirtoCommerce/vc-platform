using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Search criteria for categories and/or items.
    /// </summary>
	public class ListEntrySearchCriteria
	{
		public ListEntrySearchCriteria()
		{
			Count = 20;
		}

        /// <summary>
        /// Gets or sets the response group to define which types of entries to search for.
        /// </summary>
        /// <value>
        /// The response group.
        /// </value>
		public ResponseGroup ResponseGroup { get; set; }
        /// <summary>
        /// Gets or sets the keyword to search for.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
		public string Keyword { get; set; }
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
		public string CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
		public string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the start index of total results from which entries should be returned.
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
		public int Start { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of results to return.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
		public int Count { get; set; }
	}
}
