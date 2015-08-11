
namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Additional metainformation for a Property
    /// </summary>
    public class PropertyAttribute
    {
        public string Id { get; set; }
        public Property Property { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
}