using System;
using System.Collections.Generic;

namespace VirtoCommerce.Web.ViewModels
{
    public class BaseCustomerFormViewModel
    {
        public Dictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }

        public BaseCustomerFormViewModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string GetValue(string name)
        {
            return this.Customer.ContainsKey(name) ? this.Customer[name] : null;
        }

        public void SetValue(string name, string value)
        {
            if (this.Customer.ContainsKey(name))
            {
                this.Customer[name] = value;
            }
            else
            {
                this.Customer.Add(name, value);
            }
        }
    }
}