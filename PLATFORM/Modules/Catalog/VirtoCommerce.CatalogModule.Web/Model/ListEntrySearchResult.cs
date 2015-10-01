using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Class representing the result of ListEntries search.
    /// </summary>
	public class ListEntrySearchResult
	{
		public ListEntrySearchResult()
		{
			ListEntries = new List<ListEntry>();
		}
        /// <summary>
        /// Gets or sets the total entries count matching the search criteria.
        /// </summary>
        /// <value>
        /// The total count.
        /// </value>
		public int TotalCount { get; set; }


        /// <summary>
        /// Gets or sets the list entries.
        /// </summary>
        /// <value>
        /// The list entries.
        /// </value>
		public ICollection<ListEntry> ListEntries { get; set; }
	}
}
