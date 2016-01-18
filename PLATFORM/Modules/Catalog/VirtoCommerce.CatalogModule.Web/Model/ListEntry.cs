using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Base class for all entries used in catalog categories browsing.
    /// </summary>
	public class ListEntry
	{
		public ListEntry(string typeName)
		{
			Type = typeName;
		}
		public string Id { get; set; }
        /// <summary>
        /// Gets or sets the type. E.g. "product", "category"
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
		public string Type { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this entry is active.
        /// </summary>
		public bool? IsActive { get; set; }
        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>
        /// The image URL.
        /// </value>
		public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the entry code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
		public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
		public string Name { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
		public ListEntryLink[] Links { get; set; }

        /// <summary>
        /// All entry parents ids
        /// </summary>
        public string[] Outline { get; set; }
        /// <summary>
        /// All entry parents names
        /// </summary>
        public string[] Path { get; set; }
    }
}
