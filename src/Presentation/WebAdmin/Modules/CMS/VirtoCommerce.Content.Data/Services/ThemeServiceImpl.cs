using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Content.Data.Converters;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;

namespace VirtoCommerce.Content.Data.Services
{
	public class ThemeServiceImpl : IThemeService
	{
		private readonly object _lockObject = new object();
		private readonly IFileRepository _repository;

		public ThemeServiceImpl(IFileRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public Theme[] GetThemes(string storeId)
		{
			var themePath = GetThemePath(storeId, string.Empty);

			var items = _repository.GetContentItems(themePath);
			var themes = items.Where(i => i.ContentType == ContentType.Directory).Select(x=>x.AsTheme()); 
			return themes.ToArray();
		}

		public ThemeAsset[] GetThemeAssets(string storeId, string themeName)
		{
			var themePath = GetThemePath(storeId, themeName);
			return _repository.GetContentItems(themeName).Select(c=>c.AsThemeAsset()).ToArray();
		}

		public ThemeAsset GetThemeAsset(string storeId, string themeName, string path)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
				return _repository.GetContentItem(path).AsThemeAsset();
			}
		}

		public void SaveThemeAsset(string storeId, string themeName, Models.ThemeAsset asset)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
				_repository.SaveContentItem(asset.AsContentItem());
			}
		}

        public void DeleteThemeAssets(string storeId, string themeName, string[] assetIds)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
			    foreach (var assetId in assetIds)
			    {
                    _repository.DeleteContentItem(new ContentItem() { Id = assetId });    
			    }
				
			}
		}

		private string GetStorePath(string storeId)
		{
			return string.Format("{0}/", storeId);
		}

		private string GetThemePath(string storeId, string themeName)
		{
			if(string.IsNullOrEmpty(themeName))
			{
				return string.Format("{0}/", storeId);
			}
			return string.Format("{0}/{1}/", storeId, themeName);
		}

		private string GetFullPath(string storeId, string themeName, string path)
		{
			return string.Format("{0}/{1}/{2}", storeId, themeName, path);
		}
    }
}
