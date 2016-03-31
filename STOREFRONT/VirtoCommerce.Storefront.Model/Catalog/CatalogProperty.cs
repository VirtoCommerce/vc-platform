using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogProperty : Entity
    {
        public CatalogProperty()
        {
            LocalizedValues = new List<LocalizedString>();
            DisplayNames = new List<LocalizedString>();
        }
        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of object this property is applied to.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Property value type
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Dictionary value id
        /// </summary>
        public string ValueId { get; set; }
        /// <summary>
        /// Property values for all languages
        /// </summary>
        public ICollection<LocalizedString> LocalizedValues { get; set; }
        /// <summary>
        /// Property value in current language
        /// </summary>
        public string Value { get; set; }

        public string DisplayName { get; set; }
        public ICollection<LocalizedString> DisplayNames { get; set; }
    }
}
