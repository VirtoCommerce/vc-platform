using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts
{
    public class Category
    {
        public string Code { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Category> Parents { get; set; }

        public IEnumerable<SeoKeyword> Seo { get; set; }

        public ItemImage Image { get; set; }

        public bool Virtual { get; set; }
    }
}