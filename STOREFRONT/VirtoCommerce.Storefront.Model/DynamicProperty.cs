using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model
{
    public class DynamicProperty : Entity
    {
        public DynamicProperty()
        {
            Values = new List<LocalizedString>();
            DictionaryItems = new List<DynamicPropertyDictionaryItem>();
            DisplayNames = new List<LocalizedString>();
        }

        public string Name { get; set; }

        public LocalizedString DisplayName { get; set; }
        public ICollection<LocalizedString> DisplayNames { get; set; }
        /// <summary>
        /// Defines whether a property supports multiple values.
        /// </summary>
        public bool IsArray { get; set; }
        /// <summary>
        /// Dictionary has a predefined set of values. User can select one or more of them and cannot enter arbitrary values.
        /// </summary>
        public bool IsDictionary { get; set; }

        public bool IsRequired { get; set; }

        public string ValueType { get; set; }

        //Selected scalar values
        public ICollection<LocalizedString> Values { get; set; }
        //Selected dictionary values
        public ICollection<DynamicPropertyDictionaryItem> DictionaryValues { get; set; }
        //All possible dictionary values
        public ICollection<DynamicPropertyDictionaryItem> DictionaryItems { get; set; }
    }

  
}