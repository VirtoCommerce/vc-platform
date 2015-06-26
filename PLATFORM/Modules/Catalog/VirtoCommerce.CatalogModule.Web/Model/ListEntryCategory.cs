using System.Linq;

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
			IsActive = category.IsActive;
			if (category.Links != null)
			{
				Links = category.Links.Select(x => new ListEntryLink(x) ).ToArray();
			}
		}
    }
}
