using System.Collections.Generic;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Banners
{
    public class Banner : Drop
    {
        private IDictionary<string, string> _properties = new Dictionary<string, string>();

        public string ContentType { get; set; }

        public string Id { get; set; }

        public bool IsMultilingual { get; set; }

        public string Name { get; set; }

        public IDictionary<string, string> Properties
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