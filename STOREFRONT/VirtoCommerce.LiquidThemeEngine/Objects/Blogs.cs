using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    public class Blogs : ItemCollection<Blog>
    {
        public Blogs(IEnumerable<Blog> blogs)
            : base(blogs)
        {
        }

        public override object BeforeMethod(string method)
        {
            var retVal = base.BeforeMethod(method);
            if (!method.IsNullOrEmpty())
            {
                retVal = this.SingleOrDefault(x => x.Handle.Equals(method, StringComparison.OrdinalIgnoreCase));
            }
            return retVal;
        }

        #region ItemCollection Members
        public override bool Contains(object value)
        {
            return false;
        }
        #endregion
    }
}