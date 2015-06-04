using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Services
{
	public interface IPagesService
	{
		IEnumerable<Page> GetPages(string storeId, GetPagesCriteria criteria);
		Page GetPage(string storeId, string pageName, string language);
		void SavePage(string storeId, Page page);
		void DeletePage(string storeId, Page[] pages);
		bool CheckList(string storeId, string name, string language);
	}
}
