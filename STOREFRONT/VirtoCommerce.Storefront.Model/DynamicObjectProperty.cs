using System;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    public class DynamicObjectProperty
    {
        public DynamicObjectProperty()
        {
            Values = new List<DynamicPropertyObjectValue>();
            DisplayNames = new List<DynamicPropertyName>();
        }

        /// <summary>
        /// Gets or Sets ObjectId
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// Gets or Sets Values
        /// </summary>
        public ICollection<DynamicPropertyObjectValue> Values { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets ObjectType
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Gets or Sets IsArray
        /// </summary>
        public bool? IsArray { get; set; }

        /// <summary>
        /// Gets or Sets IsDictionary
        /// </summary>
        public bool? IsDictionary { get; set; }

        /// <summary>
        /// Gets or Sets IsMultilingual
        /// </summary>
        public bool? IsMultilingual { get; set; }

        /// <summary>
        /// Gets or Sets IsRequired
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// Gets or Sets ValueType
        /// </summary>
        public string ValueType { get; set; }

        /// <summary>
        /// Gets or Sets DisplayNames
        /// </summary>
        public ICollection<DynamicPropertyName> DisplayNames { get; set; }

        /// <summary>
        /// Gets or Sets CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Gets or Sets CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or Sets ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; }
    }
}