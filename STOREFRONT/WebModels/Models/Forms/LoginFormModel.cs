using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class LoginFormModel
    {
        public IDictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }

        public LoginFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Id
        {
            get
            {
                return "customer_login";
            }
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

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password
        {
            get
            {
                return this.GetValue("password");
            }
            set
            {
                this.SetValue("password", value);
            }
        }

        public bool RememberMe
        {
            get
            {
                bool retValue = false;
                bool.TryParse(this.GetValue("remember_me"), out retValue);

                return retValue;
            }
            set
            {
                this.SetValue("remember_me", value.ToString());
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