using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Pages : ItemCollection<Page>
    {
        public Pages(IEnumerable<Page> pages)
            : base(pages)
        {
        }

        public override object BeforeMethod(string method)
        {
            return this.SingleOrDefault(x => x.Handle.Equals(method, StringComparison.OrdinalIgnoreCase));
        }

        #region ItemCollection Members
        public override bool Contains(object value)
        {
            return false;
        }
        #endregion
    }
}