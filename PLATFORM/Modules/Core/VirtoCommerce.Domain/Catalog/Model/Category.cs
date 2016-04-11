using System.Collections.Generic;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class Category : AuditableEntity, ILinkSupport, ISeoSupport, IHasOutlines
    {
        public Category()
        {
            IsActive = true;
        }
        public string CatalogId { get; set; }
        public Catalog Catalog { get; set; }

        public string ParentId { get; set; }
        public string Code { get; set; }
        public string TaxType { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsVirtual { get; set; }
        public int Level { get; set; }
        public Category[] Parents { get; set; }

        public int Priority { get; set; }

        public bool? IsActive { get; set; }

        public ICollection<Category> Children { get; set; }
        public ICollection<Property> Properties { get; set; }
        public ICollection<PropertyValue> PropertyValues { get; set; }
        public ICollection<CategoryLink> Links { get; set; }
        public string SeoObjectType { get { return GetType().Name; } }
        public ICollection<SeoInfo> SeoInfos { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Outline> Outlines { get; set; }
    }
}