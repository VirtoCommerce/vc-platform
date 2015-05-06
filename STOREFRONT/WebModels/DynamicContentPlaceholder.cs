using System.Collections.Generic;

namespace VirtoCommerce.Web.Models
{
    public class DynamicContentPlaceholder
    {
        public string PlaceholderId { get; set; }

        public ICollection<string> Items { get; set; }
    }
}