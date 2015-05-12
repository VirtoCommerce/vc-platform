using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class VerifyCodeFormModel
    {
        public IDictionary<string, object> Context { get; set; }

        public string form_type { get; set; }

        public VerifyCodeFormModel()
        {
            this.Context = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public string Provider
        {
            get
            {
                return (string)this.GetValue("provider");
            }
            set
            {
                this.SetValue("provider", value);
            }
        }

        [Required]
        public string Code
        {
            get
            {
                return (string)this.GetValue("code");
            }
            set
            {
                this.SetValue("code", value);
            }
        }

        public string ReturnUrl
        {
            get
            {
                return (string)this.GetValue("return_url");
            }
            set
            {
                this.SetValue("return_url", value);
            }
        }

        public bool RememberMe
        {
            get
            {
                var value = this.GetValue("remember_me");

                return value != null ? (bool)value : false;
            }
            set
            {
                this.SetValue("remember_me", value);
            }
        }

        public object GetValue(string key)
        {
            return this.Context.ContainsKey(key) ? this.Context[key] : null;
        }

        public void SetValue(string key, object value)
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