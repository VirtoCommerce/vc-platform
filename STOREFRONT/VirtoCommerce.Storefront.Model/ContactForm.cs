using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model
{
    public class ContactForm
    {
        public IDictionary<string, string> Contact { get; set; }
        public string form_type { get; set; }

        public ContactForm()
        {
            this.Contact = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }


        public string GetValue(string key)
        {
            return Contact.ContainsKey(key) ? Contact[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (Contact.ContainsKey(key))
            {
                Contact[key] = value;
            }
            else
            {
                Contact.Add(key, value);
            }
        }
    }
}
