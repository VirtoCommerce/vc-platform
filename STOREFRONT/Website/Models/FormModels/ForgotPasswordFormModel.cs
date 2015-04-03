using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ForgotPasswordFormModel
    {
        public IDictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }

        public ForgotPasswordFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        [Required]
        [EmailAddress]
        public string Email
        {
            get
            {
                return this.GetValue("email");
            }
            set
            {
                this.SetValue("email", value);
            }
        }

        public string GetValue(string key)
        {
            return this.Customer.ContainsKey(key) ? this.Customer[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (this.Customer.ContainsKey(key))
            {
                this.Customer[key] = value;
            }
            else
            {
                this.Customer.Add(key, value);
            }
        }
    }
}