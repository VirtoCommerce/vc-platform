using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class DynamicContentItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ContentType
        {
            get; set; 
        }

        public bool IsMultilingual
        {
            get; set; 
        }

        public PropertyDictionary Properties { get; set; }
    }

    public class DynamicContentItemGroup
    {
        public DynamicContentItemGroup(string groupName)
        {
            GroupName = groupName;
            Items = new List<DynamicContentItem>();
        }

        public List<DynamicContentItem> Items { get; private set; } 

        public string GroupName { get; set; }
    }
}
