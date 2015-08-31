using System.Linq;
using Omu.ValueInjecter;
using webModels = VirtoCommerce.Content.Web.Models;
using coreModels = VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Web.Converters
{
	public static class MenuLinkListConverter
	{
		public static coreModels.MenuLinkList ToCoreModel(this webModels.MenuLinkList list)
		{
            var retVal = new coreModels.MenuLinkList();
            retVal.InjectFrom(list);

            foreach(var link in list.MenuLinks)
            {
                retVal.MenuLinks.Add(link.ToCoreModel());
            }

            return retVal;
		}

		public static webModels.MenuLinkList ToWebModel(this coreModels.MenuLinkList list)
		{
		    if (list == null)
		        return null;

			var retVal = new webModels.MenuLinkList
			             {
			                 Id = list.Id,
			                 Name = list.Name,
			                 StoreId = list.StoreId,
			                 Language = list.Language
			             };

		    if (list.MenuLinks.Any())
		    {
		        retVal.MenuLinks = list.MenuLinks.OrderByDescending(l => l.Priority).Select(s => s.ToWebModel()).ToArray();
		    }

		    return retVal;
		}
	}
}