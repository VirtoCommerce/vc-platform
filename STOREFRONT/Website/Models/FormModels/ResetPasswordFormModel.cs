using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ResetPasswordFormModel
    {
        public IDictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }

        public ResetPasswordFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

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

        public string Code
        {
            get
            {
                return this.GetValue("code");
            }
            set
            {
                this.SetValue("code", value);
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

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Compare("Password")]
        public string PasswordConfirmation
        {
            get
            {
                return this.GetValue("password_confirmation");
            }
            set
            {
                this.SetValue("password_confirmation", value);
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