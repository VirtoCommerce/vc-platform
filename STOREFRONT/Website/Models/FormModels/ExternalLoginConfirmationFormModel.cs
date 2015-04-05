using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ExternalLoginConfirmationFormModel
    {
        public IDictionary<string, string> Context { get; set; }

        public string form_type { get; set; }

        public ExternalLoginConfirmationFormModel()
        {
            this.Context = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
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

        public string LoginProvider
        {
            get
            {
                return this.GetValue("login_provider");
            }
            set
            {
                this.SetValue("login_provider", value);
            }
        }

        public string ReturnUrl
        {
            get
            {
                return this.GetValue("return_url");
            }
            set
            {
                this.SetValue("return_url", value);
            }
        }

        public string GetValue(string key)
        {
            return this.Context.ContainsKey(key) ? this.Context[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (this.Context.ContainsKey(key))
            {
                this.Context[key] = value;
            }
            else
            {
                this.Context.Add(key, value);
            }
        }
    }
}