using System.Linq;

namespace VirtoCommerce.CatalogModule.Web.Model
{
    /// <summary>
    /// Category ListEntry record.
    /// </summary>
    public class ListEntryCategory : ListEntry
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
            if(category.Parents != null)
            {
                Path = category.Parents.Select(x => x.Name).ToArray();
                Outline = category.Parents.Select(x => x.Id).ToArray();
            }
 
            if (category.Links != null)
            {
                Links = category.Links.Select(x => new ListEntryLink(x)).ToArray();
            }
        }
    }
}
