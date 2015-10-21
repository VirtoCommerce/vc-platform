using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Storefront.Model
{
    public class DynamicProperty
    {
        public DynamicProperty()
        {
            Values = new List<string>();
            DisplayNames = new List<DynamicPropertyDisplayName>();
        }
        public string Name { get; set; }
        public string ValueType { get; set; }
        public ICollection<string> Values { get; set; }
        public ICollection<DynamicPropertyDisplayName> DisplayNames { get; set; }
    }

    public class DynamicPropertyDisplayName
    {
        /// <summary>
        /// Language ID, e.g. en-US.
        /// </summary>
        public string Locale { get; set; }
        public string Name { get; set; }

    }
}