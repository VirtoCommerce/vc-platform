using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class PropertyValue : Entity
    {
        /// <summary>
        /// Name of the property that this value belongs to.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Property value type of the value.
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Value id in case this value is for property which supports dictionary values.
        /// </summary>
        public string ValueId { get; set; }

        /// <summary>
        /// Actual value of property value.
        /// </summary>
        public Object Value { get; set; }
    }
}
