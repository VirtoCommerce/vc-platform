using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Converters;
using System.Web.Hosting;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.Content.Data.Services
{
	public class PagesServiceImpl : IPagesService
	{
		private readonly object _lockObject = new object();
		private readonly IContentRepository _repository;
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _tempPath;

		public PagesServiceImpl(IContentRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public PagesServiceImpl(IContentRepository repository, IBlobStorageProvider blobProvider, string tempPath)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			if (blobProvider == null)
				throw new ArgumentNullException("blobProvider");

			_repository = repository;
			_blobProvider = blobProvider;
			_tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
		}

		public IEnumerable<Models.Page> GetPages(string storeId, GetPagesCriteria criteria)
		{
			var path = string.Format("{0}/", storeId);
			var pages = _repository.GetPages(path, criteria);

			foreach (var page in pages)
			{
				page.Path = FixPath(GetPageMainPath(storeId, page.Language), page.Path);
				page.ContentType = ContentTypeUtility.GetContentType(page.Name, page.ByteContent);
			}

			return pages.Select(p => p.AsPage());
		}

		public Models.Page GetPage(string storeId, string pageName, string language)
		{
			var fullPath = GetFullName(storeId, pageName, language);
			var item = _repository.GetPage(fullPath);

			if(item == null)
			{
				return null;
			}

			item.Path = FixPath(GetPageMainPath(storeId, language), item.Path);
			item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);

			return item.AsPage();
		}

		public void SavePage(string storeId, Models.Page page)
		{
			var fullPath = GetFullName(storeId, page.PageName, page.Language);

			page.Id = fullPath;

			_repository.SavePage(fullPath, page.AsContentPage());
		}

		public void DeletePage(string storeId, Page[] pages)
		{
			foreach (var page in pages)
			{
				var fullPath = GetFullName(storeId, page.PageName, page.Language);

				_repository.DeletePage(fullPath);
			}
		}

		public bool CheckList(string storeId, string name, string language)
		{
			var page = GetPage(storeId, name, language);
			if (page != null)
			{
				return false;
			}

			return true;
		}

		private string FixPath(string pageMainPath, string path)
		{
			return path.ToLowerInvariant().Replace(pageMainPath.ToLowerInvariant(), string.Empty).Trim('/');
		}

		private string GetPageMainPath(string storeId, string language)
		{
			return string.Format("{0}/{1}", storeId, language);
		}

		private string GetFullName(string storeId, string pageName, string language)
		{
			return string.Format("{0}/{1}/{2}", storeId, language, pageName);
		}
	}
}
