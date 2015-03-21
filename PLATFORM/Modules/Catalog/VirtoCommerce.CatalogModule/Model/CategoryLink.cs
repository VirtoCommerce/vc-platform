
using VirtoCommerce.Foundation.Frameworks;
namespace VirtoCommerce.CatalogModule.Model
{
	public class CategoryLink : ValueObject<CategoryLink>
    {
		public string CatalogId { get; set; }
        public string CategoryId { get; set; }
    }
}
