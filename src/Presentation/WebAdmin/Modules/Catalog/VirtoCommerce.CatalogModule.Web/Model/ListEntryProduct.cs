using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntryProduct : ListEntryBase
	{
		public ListEntryProduct(Product product)
		{
			Type = "product";
			Id = product.Id;
			ImageUrl = product.ImgSrc;
			Code = product.Code;
			Name = product.Name;
		}
	}
}
