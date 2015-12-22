using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public class ContactUsForm
    {
        public ContactUsForm()
        {
            Contact = new Dictionary<string, object>();
        }

        public IDictionary<string, object> Contact { get; set; }
        public string FormType { get; set; }
    }
}
