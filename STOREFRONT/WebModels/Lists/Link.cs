using DotLiquid;

namespace VirtoCommerce.Web.Models.Lists
{
    public class Link : Drop
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public string Handle { get; set; }

        public bool Active { get; set; }
    }
}