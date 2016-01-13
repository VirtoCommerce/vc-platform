using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Product ListEntry record.
    /// </summary>
	public class ListEntryProduct : ListEntry
	{
		public static string TypeName = "product";
		public string ProductType { get; set; }
		public ListEntryProduct(Product product)
			: base(TypeName)
		{
			Id = product.Id;
			ImageUrl = product.ImgSrc;
			Code = product.Code;
			Name = product.Name;
			ProductType = product.ProductType;
			IsActive = product.IsActive ?? true;
            if(product.Category != null)
            {
                Parents = product.Category.Parents.Concat(new[] { new KeyValuePair<string, string>(product.Category.Id, product.Category.Name) });
            }
			if (product.Links != null)
			{
				Links = product.Links.Select(x => new ListEntryLink(x) ).ToArray();
			}
		}
	}
}
