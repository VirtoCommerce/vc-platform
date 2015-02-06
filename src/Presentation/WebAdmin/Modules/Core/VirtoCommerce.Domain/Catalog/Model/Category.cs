using System.Collections.Generic;

namespace VirtoCommerce.Domain.Catalog.Model
{
	public class Category : ILinkSupport, ISeoSupport
	{
		public string CatalogId { get; set; }
		public Catalog Catalog { get; set; }

		public string Id { get; set; }
		public string ParentId { get; set; }
	    public string Code { get; set; }

		public string Name { get; set; }
		public string Path	{  get;  set; }
		public bool Virtual { get; set; }
		public Category[] Parents { get; set; }

	    public int Priority { get; set; }

	    public bool IsActive { get; set; }

        public ICollection<Category> Children { get; set; }
		public ICollection<PropertyValue> PropertyValues { get; set; }
		public ICollection<CategoryLink> Links { get; set; }
		public ICollection<SeoInfo> SeoInfos { get; set; }
	}
}