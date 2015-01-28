using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Association
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string ItemId { get; set; }
        public string Type { get; set; }
    }
}
