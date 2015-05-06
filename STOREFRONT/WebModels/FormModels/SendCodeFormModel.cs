using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class SendCodeFormModel
    {
        public IDictionary<string, object> Context { get; set; }

        public string form_type { get; set; }

        public SendCodeFormModel()
        {
            this.Context = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> Providers
        {
            get
            {
                object retValue;
                return this.Context.TryGetValue("providers", out retValue) ? retValue as IEnumerable<string> : null;
            }
            set
            {
                this.SetValue("providers", value);
            }
        }

        [Required]
        public string SelectedProvider
        {
            get
            {
                object retValue;
                return this.Context.TryGetValue("selected_provider", out retValue) ? retValue as string : null;
            }
            set
            {
                this.SetValue("selected_provider", value);
            }
        }

        public string ReturnUrl
        {
            get
            {
                object retValue;
                return this.Context.TryGetValue("return_url", out retValue) ? retValue as string : null;
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
                object retValue;
                return this.Context.TryGetValue("remember_me", out retValue) ? (bool)retValue : false;
            }
            set
            {
                this.SetValue("remember_me", value);
            }
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