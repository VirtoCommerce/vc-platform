using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public class DynamicPropertyDictionaryItem : Drop
    {
        public string Id { get; set; }
        public string PropertyId { get; set; }
        public string Name { get; set; }
    }
}
