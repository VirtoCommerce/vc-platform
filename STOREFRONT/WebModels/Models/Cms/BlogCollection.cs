using System.Collections.Generic;

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
    }

}
