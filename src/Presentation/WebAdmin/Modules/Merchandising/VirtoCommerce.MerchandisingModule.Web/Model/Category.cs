using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web2.Model
{
    public class Category
    {
        public string Id { get; set; }
        
        public string ParentId { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }

		public Dictionary<string, string> Parents { get; set; }
        
        public bool Virtual { get; set; }
    }
}
