using System;
using System.Collections.Generic;

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ExternalLoginFormModel
    {
        public IDictionary<string, string> Context { get; set; }

        public string form_type { get; set; }

        public ExternalLoginFormModel()
        {
            this.Context = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string Id
        {
            get
            {
                return "external_login";
            }
        }

        public string AuthenticationType
        {
            get
            {
                return this.GetValue("authentication_type");
            }
            set
            {
                this.SetValue("authentication_type", value);
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