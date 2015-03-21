#region
using System;
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class BaseCustomerFormModel
    {
        #region Constructors and Destructors
        public BaseCustomerFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Public Properties
        public Dictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }
        #endregion

        #region Public Methods and Operators
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
        #endregion
    }
}