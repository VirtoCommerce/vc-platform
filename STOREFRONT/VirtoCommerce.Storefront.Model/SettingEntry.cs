using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public class SettingEntry 
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string[] AllowedValues { get; set; }
        public string DefaultValue { get; set; }
        public bool IsArray { get; set; }
        public string[] ArrayValues { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
