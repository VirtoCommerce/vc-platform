using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class RegisterFormModel
    {
        public IDictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }

        public RegisterFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Id
        {
            get
            {
                return "create_customer";
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

        public string FirstName
        {
            get
            {
                return this.GetValue("first_name");
            }
            set
            {
                this.SetValue("first_name", value);
            }
        }

        public string LastName
        {
            get
            {
                return this.GetValue("last_name");
            }
            set
            {
                this.SetValue("last_name", value);
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