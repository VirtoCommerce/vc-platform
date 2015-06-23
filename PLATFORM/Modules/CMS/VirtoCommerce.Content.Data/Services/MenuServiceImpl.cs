using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Content.Data.Repositories;

namespace VirtoCommerce.Content.Data.Services
{
	public class MenuServiceImpl : IMenuService
	{
        private readonly Func<IMenuRepository> _menuRepositoryFactory;

		public MenuServiceImpl(Func<IMenuRepository> menuRepositoryFactory)
		{
            if (menuRepositoryFactory == null)
                throw new ArgumentNullException("menuRepositoryFactory");

            _menuRepositoryFactory = menuRepositoryFactory;
		}

		public IEnumerable<Models.MenuLinkList> GetListsByStoreId(string storeId)
		{
           return _menuRepositoryFactory().GetListsByStoreId(storeId);
		}

	    public Models.MenuLinkList GetListById(string listId)
	    {
	        return _menuRepositoryFactory().GetListById(listId);
	    }

	    public void UpdateList(Models.MenuLinkList list)
	    {
	        _menuRepositoryFactory().UpdateList(list);
	    }

	    public void DeleteList(string listId)
	    {
	        _menuRepositoryFactory().DeleteList(listId);
	    }

	    public bool CheckList(string storeId, string name, string language, string id)
		{
	        using (var repository = _menuRepositoryFactory())
	        {
	            var lists = repository.GetListsByStoreId(storeId);

	            return !lists.Any(l => l.Name == name && l.Language == language && l.Id != id);
	        }
		}
	}
}
