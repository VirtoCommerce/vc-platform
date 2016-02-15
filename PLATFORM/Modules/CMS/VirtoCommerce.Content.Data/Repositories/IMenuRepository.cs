using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Repositories
{
    public interface IMenuRepository : IRepository
	{
        IEnumerable<MenuLinkList> GetAllLinkLists();
        IEnumerable<MenuLinkList> GetListsByStoreId(string storeId);
		MenuLinkList GetListById(string listId);
	}
}
