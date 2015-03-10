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
			var retVal = new coreModels.MenuLinkList();

			retVal.Id = list.Id;
			retVal.Name = list.Name;
			retVal.StoreId = list.StoreId;
			retVal.Language = list.Language;

			retVal.MenuLinks = list.MenuLinks.Select(s => s.ToCoreModel()).ToList();

			return retVal;
		}

		public static webModels.MenuLinkList ToWebModel(this coreModels.MenuLinkList list)
		{
			var retVal = new webModels.MenuLinkList();

			retVal.Id = list.Id;
			retVal.Name = list.Name;
			retVal.StoreId = list.StoreId;
			retVal.Language = list.Language;

			retVal.MenuLinks = list.MenuLinks.Select(s => s.ToWebModel());

			return retVal;
		}
	}
}