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

        public string UserFullName
        {
            get
            {
                return this.GetValue("user_full_name");
            }
            set
            {
                this.SetValue("user_full_name", value);
            }
        }

        public string GetValue(string key)
        {
            return this.Context.ContainsKey(key) ? this.Context["key"] as string : null;
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