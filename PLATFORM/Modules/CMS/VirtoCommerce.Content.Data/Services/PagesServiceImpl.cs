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
using System.Text.RegularExpressions;

namespace VirtoCommerce.Content.Data.Services
{
	public class PagesServiceImpl : IPagesService
	{
		private readonly object _lockObject = new object();
		private readonly Func<IContentRepository> _repositoryFactory;
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _tempPath;

		public PagesServiceImpl(Func<IContentRepository> repositoryFactory)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			_repositoryFactory = repositoryFactory;
		}

		public PagesServiceImpl(Func<IContentRepository> repositoryFactory, IBlobStorageProvider blobProvider, string tempPath)
		{
			if (repositoryFactory == null)
				throw new ArgumentNullException("repositoryFactory");

			if (blobProvider == null)
				throw new ArgumentNullException("blobProvider");

			_repositoryFactory = repositoryFactory;
			_blobProvider = blobProvider;
			_tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
		}

		public IEnumerable<Models.Page> GetPages(string storeId, GetPagesCriteria criteria)
		{
			var path = string.Format("{0}/", storeId);
			using (var repository = _repositoryFactory())
			{
				var pages = repository.GetPages(path, criteria);

				foreach (var page in pages)
				{
					page.Path = FixPath(GetPageMainPath(storeId, page.Language), page.Path);
					page.ContentType = ContentTypeUtility.GetContentType(page.Name, page.ByteContent);
				}

				return pages.Select(p => p.AsPage()).ToArray();
			}
		}

		public Models.Page GetPage(string storeId, string pageName, string language)
		{
			var fullPath = GetFullName(storeId, pageName, language);
			using (var repository = _repositoryFactory())
			{
				var item = repository.GetPage(fullPath);

				if (item == null)
				{
					return null;
				}

				item.Path = FixPath(GetPageMainPath(storeId, language), item.Path);
                if (string.IsNullOrEmpty(item.ContentType))
                {
                    item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);
                }

				return item.AsPage();
			}
		}

		public void SavePage(string storeId, Models.Page page)
		{
			var fullPath = GetFullName(storeId, page.PageName, page.Language);

			page.Id = fullPath;

			using (var repository = _repositoryFactory())
			{
				repository.SavePage(fullPath, page.AsContentPage());
			}
		}

		public void DeletePage(string storeId, Page[] pages)
		{
			using (var repository = _repositoryFactory())
			{
				foreach (var page in pages)
				{
					var fullPath = GetFullName(storeId, page.PageName, page.Language);

					repository.DeletePage(fullPath);
				}
			}
		}

        public void DeleteBlog(string storeId, string blogName)
        {
            var path = string.Format("{0}/", storeId);
            using (var repository = _repositoryFactory())
            {
                var pages = repository.GetPages(path, null);
                pages = pages.Where(p => p.Id.Split('/').Length > 4 && p.Id.Split('/')[2].Equals("blogs") && p.Id.Split('/')[3].Equals(blogName));
                foreach(var page in pages.ToArray())
                {
                    repository.DeletePage(page.Id);
                }
            }
        }

        public void UpdateBlog(string storeId, string blogName, string oldBlogName)
        {
            var path = string.Format("{0}/", storeId);
            using (var repository = _repositoryFactory())
            {
                var pages = repository.GetPages(path, null);
                pages = pages.Where(p => p.Id.Split('/').Length > 4 && p.Id.Split('/')[2].Equals("blogs") && p.Id.Split('/')[3].Equals(oldBlogName));
                foreach (var page in pages.ToArray())
                {
                    repository.DeletePage(page.Id);
                    var regex = new Regex(Regex.Escape(oldBlogName));
                    page.Id = regex.Replace(page.Id, blogName, 1);
                    page.Path = regex.Replace(page.Path, blogName, 1);
                    page.Name = regex.Replace(page.Name, blogName, 1);
                    repository.SavePage(GetFullName(storeId, page.Name, page.Language), page);
                }
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
