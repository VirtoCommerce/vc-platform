using System;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Model
{
    public class DynamicContentItem
    {
        public DynamicContentItem()
        {
            DynamicProperties = new List<DynamicProperty>();
        }

        public string ContentType { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Description { get; set; }

        public ICollection<DynamicProperty> DynamicProperties { get; set; }

        public string FolderId { get; set; }

        public string Id { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Name { get; set; }

        public string ObjectType { get; set; }

        public string Outline { get; set; }

        public string Path { get; set; }
    }
}