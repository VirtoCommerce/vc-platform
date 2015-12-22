using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public class DynamicPropertyDictionaryItem
    {
        public DynamicPropertyDictionaryItem()
        {
            DisplayNames = new List<LocalizedString>();
        }
        public string Id { get; set; }
        public string PropertyId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public ICollection<LocalizedString> DisplayNames { get; set; }
    }
}
