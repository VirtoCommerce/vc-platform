using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Menu.Data.Models;

namespace VirtoCommerce.Content.Menu.Data.Repositories
{
	public interface IMenuRepository
	{
		IEnumerable<MenuLinkList> GetListsByStoreId(string storeId);
		MenuLinkList GetListById(string listId);
		void UpdateList(MenuLinkList list);
		void DeleteList(string listId);
	}
}
