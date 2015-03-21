#region
using System;
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class BaseAddressFormModel
    {
        #region Constructors and Destructors
        public BaseAddressFormModel()
        {
            this.Address = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Public Properties
        public Dictionary<string, string> Address { get; set; }

        public string form_type { get; set; }
        #endregion

        #region Public Methods and Operators
        public string GetValue(string key)
        {
            return this.Address.ContainsKey(key) ? this.Address[key] : null;
        }

        public void SetValue(string key, string value)
        {
            if (this.Address.ContainsKey(key))
            {
                this.Address[key] = value;
            }
            else
            {
                this.Address.Add(key, value);
            }
        }
        #endregion
    }
}