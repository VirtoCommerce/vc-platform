using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.CatalogModule.Web.Model
{
	public class ListEntryCategory : ListEntryBase
	{
		public static string TypeName = "category";

		public ListEntryCategory(Category category)
			: base(TypeName)
		{
			Id = category.Id;
			ImageUrl = "";
			Code = category.Code;
			Name = category.Name;

		}
	}
}
