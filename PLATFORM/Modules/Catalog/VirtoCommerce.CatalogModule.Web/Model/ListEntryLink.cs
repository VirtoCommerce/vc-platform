using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Information to define linking information from item or category to category.
    /// </summary>
	public class ListEntryLink
	{
		public ListEntryLink()
		{
		}
		public ListEntryLink(CategoryLink link)
		{
			CatalogId = link.CatalogId;
			CategoryId = link.CategoryId;
			ListEntryId = link.SourceItemId;
		}

        /// <summary>
        /// Gets or sets the list entry identifier.
        /// </summary>
        /// <value>
        /// The list entry identifier.
        /// </value>
		public string ListEntryId { get; set; }
        /// <summary>
        /// Gets or sets the type of the list entry. E.g. "product", "category"
        /// </summary>
        /// <value>
        /// The type of the list entry.
        /// </value>
		public string ListEntryType { get; set; }

        /// <summary>
        /// Gets or sets the target catalog identifier.
        /// </summary>
        /// <value>
        /// The catalog identifier.
        /// </value>
		public string CatalogId { get; set; }
        /// <summary>
        /// Gets or sets the target category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
		public string CategoryId { get; set; }
	}
}
