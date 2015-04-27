using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.Content.Data.Repositories
{
	public interface IPagesRepository
	{
		Page GetPage(string path);

		IEnumerable<Page> GetPages(string path);

		void SavePage(string path, Page page);

		void DeletePage(string path);
	}
}
