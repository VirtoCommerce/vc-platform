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
            
            if(product.Parents != null)
            {
                Path = product.Parents.Select(x => x.Name).ToArray();
                Outline = product.Parents.Select(x => x.Id).ToArray();
            }
			if (product.Links != null)
			{
				Links = product.Links.Select(x => new ListEntryLink(x) ).ToArray();
			}
		}
	}
}
