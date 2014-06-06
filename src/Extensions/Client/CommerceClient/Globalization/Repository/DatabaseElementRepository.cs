using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Client.Globalization.Repository
{
    /// <summary>
    /// Class DatabaseElementRepository.
    /// </summary>
	public class DatabaseElementRepository : IElementRepository
	{
        /// <summary>
        /// The _repository
        /// </summary>
		private readonly IAppConfigRepository _repository;
        /// <summary>
        /// The _cache repository
        /// </summary>
		private readonly ICacheRepository _cacheRepository;

		#region Cache Constants

        public const string LocalizationCachePrefix = "_Localization";
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
        /// <param name="repository">The repository.</param>
        /// <param name="cacheRepository">The cache repository.</param>
		public DatabaseElementRepository(IAppConfigRepository repository, ICacheRepository cacheRepository)
		{
			_repository = repository;
			_cacheRepository = cacheRepository;
		}

        /// <summary>
        /// The _cache helper
        /// </summary>
		CacheHelper _cacheHelper;

        /// <summary>
        /// Gets the helper.
        /// </summary>
        /// <value>The helper.</value>
		private CacheHelper Helper
		{
			get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
		}

		#region IElementRepository

        /// <summary>
        /// Enabled the languages.
        /// </summary>
        /// <returns>IQueryable{CultureInfo}.</returns>
		public IQueryable<CultureInfo> EnabledLanguages()
		{
			return Helper.Get(GetCacheKey(LocalizeCulturesCacheKey),
				() => _repository.Localizations.Where(l => !string.IsNullOrWhiteSpace(l.LanguageCode))
					.Select(l => new CultureInfo(l.LanguageCode)),
					AppConfigConfiguration.Instance.Cache.LocalizationTimeout,
					AppConfigConfiguration.Instance.Cache.IsEnabled);
		}

        /// <summary>
        /// Get elements of this instance.
        /// </summary>
        /// <returns>IQueryable{Element}.</returns>
		public IQueryable<Element> Elements()
		{
			return GetLocalizations().AsQueryable().Select(x => new Element
				{
					Name = x.Name,
					Value = x.Value,
					Category = x.Category,
					Culture = x.LanguageCode
				});
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
            Localization loc = null;

            try
            {
                loc = GetLocalization(name, category, culture);
            }
            catch
            {
                //If db is down simply continue as if loc not exists
            }

			return loc == null ? null
					   : new Element
						   {
							   Name = loc.Name,
							   Value = loc.Value,
							   Category = loc.Category,
							   Culture = loc.LanguageCode
						   };
		}

		public DateTime GetStatusDate()
	    {
			// data is always up to date
		    return DateTime.UtcNow;
	    }

		public void SetStatusDate(DateTime lastModified)
	    {
	    }

	    /// <summary>
        /// Get Categories of this instance.
        /// </summary>
        /// <returns>IQueryable{ElementCategory}.</returns>
		public IQueryable<ElementCategory> Categories()
		{
			return Helper.Get(GetCacheKey(LocalizeCategoriesCacheKey),
				() => _repository.Localizations.Select(x => new ElementCategory
					{
						Category = x.Category,
						Culture = x.LanguageCode
					}).Distinct(),
					AppConfigConfiguration.Instance.Cache.LocalizationTimeout,
					AppConfigConfiguration.Instance.Cache.IsEnabled);
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

			if (Get(element.Name, element.Category, element.Culture) == null)
			{
				try
				{
                    var loc = new Localization
				    {
				        Name = element.Name,
				        Category = element.Category,
				        LanguageCode = element.Culture,
				        Value = element.Value
				    };


                    _repository.Add(loc);
					_repository.UnitOfWork.Commit();

					if (AppConfigConfiguration.Instance.Cache.IsEnabled)
					{
					    SynchronizeCache(loc);

						Helper.Add(GetCacheKey(LocalizeCategoriesCacheKey), new ElementCategory
							{
								Category = element.Category,
								Culture = element.Culture
							},
						           AppConfigConfiguration.Instance.Cache.LocalizationTimeout);

						Helper.Add(GetCacheKey(LocalizeCulturesCacheKey), new CultureInfo(element.Culture),
						           AppConfigConfiguration.Instance.Cache.LocalizationTimeout);
					}
				}
				catch
				{
					return false;
				}

				return true;
			}
			return false;
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
				var currentItem = GetLocalization(element.Name, element.Category, element.Culture);

				if (currentItem == null)
				{
					return false;
				}

				currentItem.Value = element.Value;
				_repository.Update(currentItem);
				_repository.UnitOfWork.Commit();

                SynchronizeCache(currentItem);
			}
			catch
			{
				return false;
			}

			return true;
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
				var currentItem = GetLocalization(element.Name, element.Category, element.Culture);

				if (currentItem == null)
				{
					return false;
				}

				_repository.Remove(currentItem);
				_repository.UnitOfWork.Commit();

                SynchronizeCache(currentItem, true);
			}
			catch
			{
				return false;
			}

			return true;
		}

        /// <summary>
        /// Clears this instance.
        /// </summary>
		public void Clear()
		{
			foreach (var loc in _repository.Localizations)
			{
				_repository.Remove(loc);
			}
			_repository.UnitOfWork.Commit();
			Helper.Clear();
		}

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
		public void AddCategory(string category, string culture)
		{
			//Do nothing
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
        /// <returns>Localization.</returns>
		private Localization GetLocalization(string name, string category, string culture)
        {
            var allLocalizations = GetLocalizations();

            return
                allLocalizations.FirstOrDefault(
                    it =>
                    it.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                    && it.Category.Equals(category, StringComparison.OrdinalIgnoreCase)
                    && it.LanguageCode.Equals(culture, StringComparison.OrdinalIgnoreCase));
		}

        /// <summary>
        /// Synchronizes the cache.
        /// </summary>
        /// <param name="localization">The localization.</param>
        /// <param name="remove">if set to <c>true</c> [remove].</param>
        private void SynchronizeCache(Localization localization, bool remove = false)
        {
            var allLocalizations = GetLocalizations().ToList();

            var exisiting = allLocalizations.FirstOrDefault(l => l.Name.Equals(localization.Name) && l.Category.Equals(localization.Category) &&
                                                                 l.LanguageCode.Equals(localization.LanguageCode));


            if (exisiting == null)
            {
                allLocalizations.Add(localization);
            }
            else if (remove)
            {
                allLocalizations.Remove(exisiting);
            }
            else
            {
                exisiting.Value = localization.Value;
            }

            //Update cache
            Helper.Remove(GetCacheKey(LocalizeElementsCacheKey));
            Helper.Add(GetCacheKey(LocalizeElementsCacheKey), allLocalizations.ToArray(), AppConfigConfiguration.Instance.Cache.LocalizationTimeout);
        }

        /// <summary>
        /// Gets the localizations.
        /// </summary>
        /// <returns>
        /// IQueryable{Localization}.
        /// </returns>
		private IEnumerable<Localization> GetLocalizations()
		{
			return Helper.Get(GetCacheKey(LocalizeElementsCacheKey), () =>  _repository.Localizations.ToArray(),
			   AppConfigConfiguration.Instance.Cache.LocalizationTimeout,
			   AppConfigConfiguration.Instance.Cache.IsEnabled);
		}

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="keyTemplate">The key template.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>System.String.</returns>
		private string GetCacheKey(string keyTemplate, params string[] keys)
		{
			return string.Format(keyTemplate, CacheHelper.CreateCacheKey(LocalizationCachePrefix, keys));
		}

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="keyTemplate">The key template.</param>
        /// <param name="element">The element.</param>
        /// <returns>System.String.</returns>
		private string GetCacheKey(string keyTemplate, Element element)
		{
			return GetCacheKey(keyTemplate, element.Name, element.Category, element.Culture);
		}

		#endregion
	}
}
