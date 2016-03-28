using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Merchandising Catalog.
    /// </summary>
    public class Catalog
    {
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Catalog"/> is virtual or common.
        /// </summary>
        /// <value>
        ///   <c>true</c> if virtual; otherwise, <c>false</c>.
        /// </value>
        public bool IsVirtual { get; set; }
        /// <summary>
        /// Gets the language from languages list marked as default.
        /// </summary>
        /// <value>
        /// The default language.
        /// </value>
        public CatalogLanguage DefaultLanguage
        {
            get
            {
                if (Languages != null)
                {
                    return Languages.FirstOrDefault(x => x.IsDefault);
                }
                return null;
            }
        }
        /// <summary>
        /// Gets or sets the catalog languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public List<CatalogLanguage> Languages { get; set; }
        /// <summary>
        /// Gets or sets the catalog properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
		public List<Property> Properties { get; set; }

        public string[] SecurityScopes { get; set; }
    }
}
