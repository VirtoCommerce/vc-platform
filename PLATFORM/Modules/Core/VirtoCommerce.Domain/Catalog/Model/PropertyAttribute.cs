
namespace VirtoCommerce.Domain.Catalog.Model
{
    public class PropertyAttribute
    {
		public string Id { get; set; }
		public string PropertyId { get; set; }
        public Property Property { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}