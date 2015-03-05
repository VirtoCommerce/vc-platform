using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using VirtoCommerce.Content.Data.Converters;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Foundation.Assets.Repositories;

namespace VirtoCommerce.Content.Data.Services
{
	public class ThemeServiceImpl : IThemeService
	{
		private readonly object _lockObject = new object();
		private readonly IFileRepository _repository;
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _tempPath;

		public ThemeServiceImpl(IFileRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public ThemeServiceImpl(IFileRepository repository, IBlobStorageProvider blobProvider, string tempPath)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			if (blobProvider == null)
				throw new ArgumentNullException("blobProvider");

			_repository = repository;
			_blobProvider = blobProvider;
			_tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
		}

		public IEnumerable<Theme> GetThemes(string storeId)
		{
			var themePath = GetThemePath(storeId, string.Empty);

			var items = _repository.GetThemes(themePath);
			return items;
		}

		public IEnumerable<ThemeAsset> GetThemeAssets(string storeId, string themeName, bool loadContent = false)
		{
			var themePath = GetThemePath(storeId, themeName);
			var items = _repository.GetContentItems(themePath, loadContent);

			foreach (var item in items)
			{
				item.Path = FixPath(themePath, item.Path);
				item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);
			}

			return items.Select(c => c.AsThemeAsset());
		}

		public ThemeAsset GetThemeAsset(string storeId, string themeId, string path)
		{
			lock (_lockObject)
			{
				var fullPath = GetFullPath(storeId, themeId, path);
				var item = _repository.GetContentItem(fullPath);

				item.Path = FixPath(GetThemePath(storeId, themeId), item.Path);
				item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);

				var retVal = item.AsThemeAsset();

				return item.AsThemeAsset();
			}
		}

		public void SaveThemeAsset(string storeId, string themeId, Models.ThemeAsset asset)
		{
			lock (_lockObject)
			{
				var fullPath = GetFullPath(storeId, themeId, asset.Id);

				_repository.SaveContentItem(fullPath, asset.AsContentItem());
			}
		}

		public void DeleteThemeAssets(string storeId, string themeId, params string[] assetIds)
		{
			lock (_lockObject)
			{
				foreach (var assetId in assetIds)
				{
					var fullPath = GetFullPath(storeId, themeId, assetId);

					_repository.DeleteContentItem(fullPath);
				}

			}
		}

		private string GetStorePath(string storeId)
		{
			return string.Format("{0}/", storeId);
		}

		private string GetThemePath(string storeId, string themeName)
		{
			if (string.IsNullOrEmpty(themeName))
			{
				return string.Format("{0}", storeId);
			}
			return string.Format("{0}/{1}", storeId, themeName);
		}

		private string GetFullPath(string storeId, string themeName, string path)
		{
			return string.Format("{0}/{1}/{2}", storeId, themeName, path);
		}

		private string FixPath(string themePath, string path)
		{
			return path.Replace(themePath, string.Empty).Trim('/');
		}

	}
}
