using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Menu.Data.Repositories;

namespace VirtoCommerce.Content.Menu.Data.Services
{
	public class MenuServiceImpl : IMenuService
	{
		private readonly IMenuRepository _menuRepository;

		public MenuServiceImpl(IMenuRepository menuRepository)
		{
			if (menuRepository == null)
				throw new ArgumentNullException("menuRepository");

			_menuRepository = menuRepository;
		}

		public IEnumerable<Models.MenuLinkList> GetListsByStoreId(string storeId)
		{
			return _menuRepository.GetListsByStoreId(storeId);
		}

		public Models.MenuLinkList GetListById(string listId)
		{
			return _menuRepository.GetListById(listId);
		}

		public void UpdateList(Models.MenuLinkList list)
		{
			_menuRepository.UpdateList(list);
		}

		public void DeleteList(string listId)
		{
			_menuRepository.DeleteList(listId);
		}

		public bool CheckList(string storeId, string name, string language, string id)
		{
			var lists = _menuRepository.GetListsByStoreId(storeId);

			return !lists.Any(l => l.Name == name && l.Language == language && l.Id != id);
		}
	}
}
