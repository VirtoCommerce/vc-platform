using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ApiClient.DataContracts.DynamicProperties
{
    public class DynamicProperty 
    {
        public string Id { get; set; }
        public string Name { get; set; }
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

        public ICollection<object> Values { get; set; }
        public ICollection<DynamicPropertyDictionaryItem> DictionaryItems { get; set; }
    }
}
