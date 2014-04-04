using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Client.Globalization.Repository
{
	/// <summary>
	/// Class CachedDatabaseElementRepository.
	/// </summary>
	public class CachedDatabaseElementRepository : IElementRepository
	{
		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		/// <summary>
		/// The _cache repository
		/// </summary>
		private readonly IElementRepository _cacheRepository;
		private Func<Localization, bool> _baseFilter;

		#region Cache Constants

		/// <summary>
		/// The localize element cache key
		/// </summary>
		public const string LocalizeElementCacheKey = "LE:C:{0}";
		/// <summary>
		/// The localize elements cache key
		/// </summary>
		public const string LocalizeElementsCacheKey = "LES:C";
		/// <summary>
		/// The localize cultures cache key
		/// </summary>
		public const string LocalizeCulturesCacheKey = "LEC:C";
		/// <summary>
		/// The localize categories cache key
		/// </summary>
		public const string LocalizeCategoriesCacheKey = "LEC:CAT";

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseElementRepository"/> class.
		/// </summary>
		/// <param name="repositoryFactory">The repository factory.</param>
		/// <param name="cacheRepository">The cache repository.</param>
		public CachedDatabaseElementRepository(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IElementRepository cacheRepository, Func<Localization, bool> baseFilter)
		{
			_repositoryFactory = repositoryFactory;
			_cacheRepository = cacheRepository;
			_baseFilter = baseFilter;
		}

		#region IElementRepository

		/// <summary>
		/// Enabled the languages.
		/// </summary>
		/// <returns>IQueryable{CultureInfo}.</returns>
		public IQueryable<CultureInfo> EnabledLanguages()
		{
			if (_cacheRepository.EnabledLanguages().Any())
			{
				return _cacheRepository.EnabledLanguages();
			}

			var _repository = _repositoryFactory.GetRepositoryInstance();
			return GetLocalizationsEnumerable(_repository).Where(l => !string.IsNullOrWhiteSpace(l.LanguageCode))
					.Select(l => new CultureInfo(l.LanguageCode)).AsQueryable();
		}

		/// <summary>
		/// Get elements of this instance.
		/// </summary>
		/// <returns>IQueryable{Element}.</returns>
		public IQueryable<Element> Elements()
		{
			if (!_cacheRepository.Elements().Any())
			{
				using (var _repository = _repositoryFactory.GetRepositoryInstance())
				{
					foreach (var loc in GetLocalizationsEnumerable(_repository))
					{
						//Update cache with items
						_cacheRepository.Add(GetElement(loc));
					}
				}
			}

			return _cacheRepository.Elements();
		}

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns>Element.</returns>
		public Element Get(string name, string category, string culture)
		{
			var result = _cacheRepository.Get(name, category, culture);
			if (result == null)
			{
				try
				{
					// caching
					if (PreloadLocalizations(culture))
					{
						result = _cacheRepository.Get(name, category, culture);
					}
				}
				catch (Exception e)
				{
					Debug.WriteLine(e.ToString());
					//If db is down simply continue as if loc not exists
				}
			}

			return result;
		}

		public DateTime GetStatusDate(string culture)
		{
			return _cacheRepository.GetStatusDate(culture);
		}

		public void SetStatusDate(string culture)
		{
			_cacheRepository.SetStatusDate(culture);
		}

		/// <summary>
		/// Get Categories of this instance.
		/// </summary>
		/// <returns>IQueryable{ElementCategory}.</returns>
		public IQueryable<ElementCategory> Categories()
		{
			if (_cacheRepository.Categories().Any())
			{
				return _cacheRepository.Categories();
			}

			var _repository = _repositoryFactory.GetRepositoryInstance();
			return GetLocalizationsEnumerable(_repository).Select(x => new ElementCategory
					{
						Category = x.Category,
						Culture = x.LanguageCode
					}).Distinct().AsQueryable();
		}

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Add(Element element)
		{
			if (element == null)
			{
				return false;
			}

			try
			{
				using (var _repository = _repositoryFactory.GetRepositoryInstance())
				{
					_repository.Add(new Localization
					{
						Name = element.Name,
						Category = element.Category,
						LanguageCode = element.Culture,
						Value = element.Value
					});
					_repository.UnitOfWork.Commit();
				}

				_cacheRepository.Add(element);

				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.ToString());
				return false;
			}
		}

		/// <summary>
		/// Updates the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Update(Element element)
		{
			if (element == null)
			{
				return false;
			}
			try
			{
				using (var _repository = _repositoryFactory.GetRepositoryInstance())
				{
					var currentItem = GetLocalization(element.Name, element.Category, element.Culture, _repository);
					if (currentItem == null)
					{
						return false;
					}

					currentItem.Value = element.Value;
					_repository.Update(currentItem);
					_repository.UnitOfWork.Commit();
				}
				return _cacheRepository.Update(element);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Removes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Remove(Element element)
		{
			if (element == null)
			{
				return false;
			}

			try
			{
				using (var repository = _repositoryFactory.GetRepositoryInstance())
				{
					var currentItem = GetLocalization(element.Name, element.Category, element.Culture, repository);
					if (currentItem == null)
					{
						return false;
					}

					repository.Remove(currentItem);
					repository.UnitOfWork.Commit();
				}
				return _cacheRepository.Remove(element);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			// don't delete anything from real DB.
			//using (var repository = _repositoryFactory.GetRepositoryInstance())
			//{
			//    foreach (var loc in GetLocalizationsEnumerable(repository))
			//    {
			//        repository.Remove(loc);
			//    }
			//    repository.UnitOfWork.Commit();
			//}

			_cacheRepository.Clear();
		}

		/// <summary>
		/// Adds the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		public void AddCategory(string category, string culture)
		{
			//Do nothing in real DB
			_cacheRepository.AddCategory(category, culture);
		}

		/// <summary>
		/// Removes the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool RemoveCategory(string category, string culture)
		{
			return false;
		}

		#endregion

		#region Private Helpers

		/// <summary>
		/// Gets the localization.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <param name="repository"></param>
		/// <returns>Localization.</returns>
		private Localization GetLocalization(string name, string category, string culture, IAppConfigRepository repository)
		{
			// 'it => true && ... ' is a workaround to prevent searching by key (getting exception if Localization not found).
			return GetLocalizationsEnumerable(repository).Where(it =>
				it.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
				it.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
				it.LanguageCode.Equals(culture, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
		}

		private bool PreloadLocalizations(string culture)
		{
			var result = false;
			var dt = _cacheRepository.GetStatusDate(culture);
			if (dt == DateTime.MinValue)
			{
				using (var _repository = _repositoryFactory.GetRepositoryInstance())
				{
					var preloadedCategoryLocalizations = GetLocalizationsEnumerable(_repository).Where(it => it.LanguageCode.Equals(culture, StringComparison.OrdinalIgnoreCase));
					foreach (var preloadedCategoryLocalization in preloadedCategoryLocalizations)
					{
						_cacheRepository.Add(GetElement(preloadedCategoryLocalization));
					}
				}

				SetStatusDate(culture);
				result = true;
			}

			return result;
		}

		/// <summary>
		/// Gets the cache key.
		/// </summary>
		/// <param name="keyTemplate">The key template.</param>
		/// <param name="element">The element.</param>
		/// <returns>System.String.</returns>
		//private string GetCacheKey(string keyTemplate, Element element)
		//{
		//    return GetCacheKey(keyTemplate, element.Name, element.Category, element.Culture);
		//}

		/// <summary>
		/// Gets the cache key.
		/// </summary>
		/// <param name="keyTemplate">The key template.</param>
		/// <param name="element">The element.</param>
		/// <returns>System.String.</returns>
		//private string GetCacheKey(string keyTemplate, Localization element)
		//{
		//    return GetCacheKey(keyTemplate, element.Name, element.Category, element.LanguageCode);
		//}

		private static Element GetElement(Localization loc)
		{
			return new Element
			{
				Name = loc.Name,
				Value = loc.Value,
				Category = loc.Category,
				Culture = loc.LanguageCode
			};
		}

		private IEnumerable<Localization> GetLocalizationsEnumerable(IAppConfigRepository _repository)
		{
			IEnumerable<Localization> query = _repository.Localizations;
			if (_baseFilter != null)
				query = query.Where(_baseFilter);
			return query;
		}

		#endregion
	}
}
