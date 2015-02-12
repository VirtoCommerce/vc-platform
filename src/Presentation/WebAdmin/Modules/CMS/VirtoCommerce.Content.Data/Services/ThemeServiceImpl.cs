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
		private IFileRepository _repository;
		private IThemeRepository _themeRepository;

		public ThemeServiceImpl(IFileRepository repository, IThemeRepository themeRepository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
			_themeRepository = themeRepository;
		}

		public Models.ThemeItem[] GetThemes(string storeId)
		{
			var storeRelation = GetStoreRelation(storeId);

			var items = _repository.GetContentItems(string.Empty);
			var themes = items.Where(i => i.ContentType == Models.ContentType.Directory).Select(i => ThemeItemConverter.ContentItem2ThemeItem(i));

			if (storeRelation != null)
			{
				var activeTheme = themes.FirstOrDefault(t => t.ThemeName == storeRelation.ThemeName);
				if (activeTheme != null)
					activeTheme.IsActive = true;
			}

			return themes.ToArray();
		}

		public void SetThemeAsActive(string storeId, string themeName)
		{
			var storeRelation = GetStoreRelation(storeId);

			if(storeRelation != null)
			{
				storeRelation.ThemeName = themeName;
				_themeRepository.Update(storeRelation);
			}
			else
			{
				storeRelation = new ThemeStoreRelation();

				storeRelation.ThemeName = themeName;
				storeRelation.StoreId = storeId;
			}
		}

		public Models.ContentItem[] GetContentItems(string path)
		{
			return _repository.GetContentItems(path);
		}

		public Models.ContentItem GetContenttItem(string path)
		{
			return _repository.GetContentItem(path);
		}

		public void SaveContentItem(Models.ContentItem item)
		{
			_repository.SaveContentItem(item);
		}

		public void DeleteContentItem(Models.ContentItem item)
		{
			_repository.DeleteContentItem(item);
		}

		private ThemeStoreRelation GetStoreRelation(string storeId)
		{
			return _themeRepository.ThemeStoreRelations.FirstOrDefault(r => r.StoreId == storeId);
		}
	}
}
