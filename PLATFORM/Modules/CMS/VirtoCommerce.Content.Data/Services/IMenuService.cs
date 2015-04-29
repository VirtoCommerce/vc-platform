using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IMenuService
	{
		IEnumerable<MenuLinkList> GetListsByStoreId(string storeId);
		MenuLinkList GetListById(string listId);
		void UpdateList(MenuLinkList list);
		void DeleteList(string listId);
		bool CheckList(string storeId, string name, string language, string id);
	}
}
