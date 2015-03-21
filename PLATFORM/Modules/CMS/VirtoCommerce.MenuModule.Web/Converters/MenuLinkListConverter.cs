using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.MenuModule.Web.Models;
using coreModels = VirtoCommerce.Content.Menu.Data.Models;

namespace VirtoCommerce.MenuModule.Web.Converters
{
	public static class MenuLinkListConverter
	{
		public static coreModels.MenuLinkList ToCoreModel(this webModels.MenuLinkList list)
		{
			var retVal = new coreModels.MenuLinkList
			             {
			                 Id = list.Id,
			                 Name = list.Name,
			                 StoreId = list.StoreId,
			                 Language = list.Language,
			                 MenuLinks = list.MenuLinks.Select(s => s.ToCoreModel()).ToList()
			             };

			return retVal;
		}

		public static webModels.MenuLinkList ToWebModel(this coreModels.MenuLinkList list)
		{
			var retVal = new webModels.MenuLinkList
			             {
			                 Id = list.Id,
			                 Name = list.Name,
			                 StoreId = list.StoreId,
			                 Language = list.Language,
			                 MenuLinks = list.MenuLinks.OrderByDescending(l=>l.Priority).Select(s => s.ToWebModel()).ToArray()
			             };

			return retVal;
		}
	}
}