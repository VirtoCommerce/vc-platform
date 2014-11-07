using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntryCategory : ListEntryBase
	{
		public ListEntryCategory(Category category)
		{
			Type = "category";
			Id = category.Id;
			ImageUrl = "";
			Code = category.Code;
			Name = category.Name;
		}
	}
}
