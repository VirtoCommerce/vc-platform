using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Pages.Data.Repositories;

namespace VirtoCommerce.Content.Pages.Data.Services
{
	public class PagesServiceImpl : IPagesService
	{
		private readonly IPagesRepository _pagesRepository;

		public PagesServiceImpl(IPagesRepository pagesRepository)
		{
			if (pagesRepository == null)
				throw new ArgumentNullException("pagesRepository");

			_pagesRepository = pagesRepository;
		}

		public IEnumerable<Models.ShortPageInfo> GetPages(string storeId)
		{
			var path = string.Format("{0}/", storeId);
			return _pagesRepository.GetPages(path);
		}

		public Models.Page GetPage(string storeId, string pageName)
		{
			var fullPath = GetFullName(storeId, pageName);

			return _pagesRepository.GetPage(fullPath);
		}

		public void SavePage(string storeId, Models.Page page)
		{
			var fullPath = GetFullName(storeId, page.Name);

			page.Path = fullPath;

			_pagesRepository.SavePage(fullPath, page);
		}

		public void DeletePage(string storeId, string[] pageNames)
		{
			foreach(var pageName in pageNames)
			{
				var fullPath = GetFullName(storeId, pageName);

				_pagesRepository.DeletePage(fullPath);
			}
		}

		private string GetFullName(string storeId, string pageName)
		{
			return string.Format("{0}/{1}.liquid", storeId, pageName);
		}
	}
}
