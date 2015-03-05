using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webModels = VirtoCommerce.MenuModule.Web.Models;
using coreModels = VirtoCommerce.Content.Menu.Data.Models;

namespace VirtoCommerce.MenuModule.Web.Converters
{
	public static class MenuLinkConverter
	{
		public static coreModels.MenuLink ToCoreModel(this webModels.MenuLink link)
		{
			var retVal = new coreModels.MenuLink();

			retVal.Id = link.Id;
			retVal.Name = link.Name;
			retVal.Link = link.Link;
			retVal.MenuLinkListId = link.MenuLinkListId;

			return retVal;
		}

		public static webModels.MenuLink ToWebModel(this coreModels.MenuLink link)
		{
			var retVal = new webModels.MenuLink();

			retVal.Id = link.Id;
			retVal.Name = link.Name;
            retVal.Link = link.Link;
			retVal.MenuLinkListId = link.MenuLinkListId;

			return retVal;
		}
	}
}