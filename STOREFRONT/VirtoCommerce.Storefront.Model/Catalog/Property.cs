using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class Property : Entity
    {
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
        /// Current property values. Collection is used as a general placeholder to store both single and multi-value values.
        /// </summary>
        public List<PropertyValue> Values { get; set; }
    }
}
