using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Models.Cms
{
    public class BlogCollection : ItemCollection<Blog>
    {
        public BlogCollection(IEnumerable<Blog> collections)
            : base(collections)
        {
        }

        public override int TotalCount
        {
            get { return this.Size; }
        }

        public override object BeforeMethod(string method)
        {
            if (this.Root == null) return null;
            return this.Root.SingleOrDefault(x => x.Handle == method);
        }
    }

}
