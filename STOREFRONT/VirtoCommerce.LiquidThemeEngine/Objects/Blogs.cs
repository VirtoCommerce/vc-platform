using System;
using System.Collections.Generic;
using System.Linq;

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