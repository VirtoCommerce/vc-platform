using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntryProduct : ListEntryBase
	{
		public static string TypeName = "product";
		public ListEntryProduct(Product product)
			: base(TypeName)
		{
			Id = product.Id;
			ImageUrl = product.ImgSrc;
			Code = product.Code;
			Name = product.Name;
			if (product.Links != null)
			{
				Links = product.Links.Select(x => new ListEntryLink(x) ).ToArray();
			}
		}
	}
}
