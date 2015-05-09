using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models.Lists
{
    public class LinkLists : ItemCollection<LinkList>
    {
        #region Constructors and Destructors
        public LinkLists(IEnumerable<LinkList> lists)
            : base(lists)
        {
        }
        #endregion

        #region Public Methods and Operators
        public override object BeforeMethod(string method)
        {
            return this.Root.SingleOrDefault(x => x.Handle.Equals(method, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

    }
}