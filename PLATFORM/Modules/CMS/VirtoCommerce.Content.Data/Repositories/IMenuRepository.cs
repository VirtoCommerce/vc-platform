using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Repositories
{
    public interface IMenuRepository : IDisposable
	{
		IEnumerable<MenuLinkList> GetListsByStoreId(string storeId);
		MenuLinkList GetListById(string listId);
		void UpdateList(MenuLinkList list);
		void DeleteList(string listId);
	}
}
