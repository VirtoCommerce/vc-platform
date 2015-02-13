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
		private object _lockObject = new object();

		private IFileRepository _repository;

		public ThemeServiceImpl(IFileRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public Models.ThemeItem[] GetThemes(string storeId)
		{
			var themePath = GetThemePath(storeId, string.Empty);

			var items = _repository.GetContentItems(themePath, string.Empty);
			var themes = items.Where(i => i.ContentType == Models.ContentType.Directory).Select(i => ThemeItemConverter.ContentItem2ThemeItem(i));

			return themes.ToArray();
		}

		public Models.ContentItem[] GetContentItems(string storeId, string themeName, string path)
		{
			var themePath = GetThemePath(storeId, themeName);
			return _repository.GetContentItems(themePath, path);
		}

		public Models.ContentItem GetContentItem(string storeId, string themeName, string path)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
				return _repository.GetContentItem(themePath, path);
			}
		}

		public void SaveContentItem(string storeId, string themeName, Models.ContentItem item)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
				_repository.SaveContentItem(themePath, item);
			}
		}

		public void DeleteContentItem(string storeId, string themeName, Models.ContentItem item)
		{
			lock (_lockObject)
			{
				var themePath = GetThemePath(storeId, themeName);
				_repository.DeleteContentItem(themePath, item);
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
