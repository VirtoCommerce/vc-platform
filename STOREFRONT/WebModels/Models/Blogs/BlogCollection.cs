using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Models.Blogs
{
    public class BlogCollection : ItemCollection<Blog>
    {
        public BlogCollection(IEnumerable<Blog> collections)
            : base(collections)
        {
        }

        public override int TotalCount
        {
            get { return Size; }
        }
    }

}
