using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using VirtoCommerce.Foundation.AppConfig;
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
        /// <summary>
        /// The _repository
        /// </summary>
        private readonly IAppConfigRepository _repository;
        /// <summary>
        /// The _cache repository
        /// </summary>
        private readonly IElementRepository _cacheRepository;

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
        /// <param name="repository">The repository.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public CachedDatabaseElementRepository(IAppConfigRepository repository, IElementRepository cacheRepository)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
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

            return _repository.Localizations.Where(l => !string.IsNullOrWhiteSpace(l.LanguageCode))
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
                foreach (var loc in _repository.Localizations)
                {
                    //Update cache with items
                    _cacheRepository.Add(new Element
                    {
                        Name = loc.Name,
                        Value = loc.Value,
                        Category = loc.Category,
                        Culture = loc.LanguageCode
                    });
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
            name = GetCacheKey(name);
            var result = _cacheRepository.Get(name, category, culture);
            if (result == null)
            {
                try
                {
                    var loc = GetLocalization(name, category, culture);
                    if (loc != null)
                        result = new Element
                        {
                            Name = loc.Name,
                            Value = loc.Value,
                            Category = loc.Category,
                            Culture = loc.LanguageCode
                        };
                }
                catch
                {
                    //If db is down simply continue as if loc not exists
                }
            }
            return result;
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

            return _repository.Localizations.Select(x => new ElementCategory
                    {
                        Category = x.Category,
                        Culture = x.LanguageCode
                    }).Distinct();
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
                    _repository.Add(new Localization
                        {
                            Name = element.Name,
                            Category = element.Category,
                            LanguageCode = element.Culture,
                            Value = element.Value
                        });
                    _repository.UnitOfWork.Commit();

                    //Helper.Add(GetCacheKey(LocalizeElementCacheKey, element), element);

                    //Helper.Add(GetCacheKey(LocalizeCategoriesCacheKey), new ElementCategory
                    //    {
                    //        Category = element.Category,
                    //        Culture = element.Culture
                    //    });

                    //Helper.Add(GetCacheKey(LocalizeCulturesCacheKey), new CultureInfo(element.Culture));
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

                _cacheRepository.Update(element);
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

                _cacheRepository.Remove(element);
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
            _cacheRepository.Clear();
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
            return _repository.Localizations.Where(it =>
                it.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                it.Category.Equals(category, StringComparison.OrdinalIgnoreCase) &&
                it.LanguageCode.Equals(culture, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        const string EscapeCharacters = "[][: ~`'!@#$%^&*()+=/?{}.,|\\\\\"]";
        
        private static string GetCacheKey(string keyName)
        {
            return Regex.Replace(keyName, EscapeCharacters, "_");
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

        #endregion
    }
}
