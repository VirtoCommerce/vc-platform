using System.Collections.Generic;

namespace VirtoCommerce.ApiClient.DataContracts.Marketing
{
    public class DynamicContentItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ContentType { get; set; }

        public string FolderId { get; set; }

        public string Outline { get; set; }

        public string Path { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}