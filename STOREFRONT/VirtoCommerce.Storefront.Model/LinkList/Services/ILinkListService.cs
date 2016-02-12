using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.LinkList.Services
{
    public interface IMenuLinkListService
    {
        Task<MenuLinkList[]> LoadAllStoreLinkListsAsync(string storeId);
    }
}
