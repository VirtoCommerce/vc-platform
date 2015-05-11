using System.Collections.Generic;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Banners
{
    public class Banner : Drop
    {
        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        public virtual string ContentType { get; set; }

        public virtual string Id { get; set; }

        public virtual bool IsMultilingual { get; set; }

        public virtual string Name { get; set; }

        public virtual IDictionary<string, string> Properties
        {
            get { return this._properties; }
            set { this._properties = value; }
        }

        #region Overrides of DropBase

        public override object BeforeMethod(string method)
        {
            return this._properties.ContainsKey(method) ? this._properties[method] : base.BeforeMethod(method);
        }

        #endregion
    }
}