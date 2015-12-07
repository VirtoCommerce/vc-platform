using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Base class for assets.
    /// </summary>
	public class AssetBase
	{
		public string Id { get; set; }
		public string RelativeUrl { get; set; }
		public string Url { get; set; }
        /// <summary>
        /// Gets or sets the asset type identifier.
        /// </summary>
        /// <value>
        /// The type identifier.
        /// </value>
		public string TypeId { get; set; }
        /// <summary>
        /// Gets or sets the asset group name.
        /// </summary>
        /// <value>
        /// The group.
        /// </value>
	    public string Group { get; set; }
        /// <summary>
        /// Gets or sets the asset name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
		public string Name { get; set; }
        /// <summary>
        /// Gets or sets the asset language.
        /// </summary>
        /// <value>
        /// The language code.
        /// </value>
		public string LanguageCode { get; set; }

        /// <summary>
        /// System flag used to mark that object was inherited from other
        /// </summary>
        public bool IsInherited { get; set; }

    }
}
