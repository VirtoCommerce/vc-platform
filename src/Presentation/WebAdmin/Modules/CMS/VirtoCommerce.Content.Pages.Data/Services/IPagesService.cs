using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Pages.Data.Models;

namespace VirtoCommerce.Content.Pages.Data.Services
{
	public interface IPagesService
	{
		IEnumerable<ShortPageInfo> GetPages(string storeId);
		Page GetPage(string storeId, string pageName, string language);
		void SavePage(string storeId, Page page);
		void DeletePage(string storeId, ShortPageInfo[] pages);
		bool CheckList(string storeId, string name, string language);
	}
}
