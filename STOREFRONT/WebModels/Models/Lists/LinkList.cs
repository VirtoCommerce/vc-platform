using System.Collections.Generic;
using DotLiquid;

namespace VirtoCommerce.Web.Models.Lists
{
    public class LinkList : Drop
    {
        public string Id { get; set; }

        public string Handle { get; set; }

        public string Title { get; set; }

        public IEnumerable<Link> Links { get; set; }
    }
}