using System;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.Forms
{
    public class ContactFormModel
    {
        public IDictionary<string, string> Contact { get; set; }
        public string form_type { get; set; }

        public ContactFormModel()
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