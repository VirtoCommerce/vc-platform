using System.Collections.Generic;
using DotLiquid;

namespace VirtoCommerce.Web.Models
{
    public abstract class PropertiesDrop : Drop
    {
        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        public virtual IDictionary<string, string> Properties
        {
            get { return this._properties; }
            set { this._properties = value; }
        }

        public virtual string GetValue(string key)
        {
            return this.Properties.ContainsKey(key) ? this.Properties[key] : null;
        }

        public virtual void SetValue(string key, string value)
        {
            if (this.Properties.ContainsKey(key))
            {
                this.Properties[key] = value;
            }
            else
            {
                this.Properties.Add(key, value);
            }
        }

        #region Overrides of DropBase

        public override object BeforeMethod(string method)
        {
            return this._properties.ContainsKey(method) ? this._properties[method] : base.BeforeMethod(method);
        }

        #endregion
    }
}
