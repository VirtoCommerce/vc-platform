using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using linq = System.Linq.Expressions;

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
        private linq.Expression<Func<Localization, bool>> _baseFilter;

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
        public CachedDatabaseElementRepository(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IElementRepository cacheRepository, linq.Expression<Func<Localization, bool>> baseFilter)
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
                    // limit languages by Language setting;
                    var languageSetting = _repository.Settings.Where(s => s.Name.Contains("Lang"))
                                    .Expand(s => s.SettingValues)
                                    .FirstOrDefault();

                    // the expression is: (x=>x.LanguageCode == "en-US" || x.LanguageCode == "ja-JP" || ....)
                    var parameter = linq.Expression.Parameter(typeof(Localization), "x");
                    linq.Expression condition = linq.Expression.Constant(false);
                    foreach (var name in languageSetting.SettingValues)
                    {
                        condition = linq.Expression.OrElse(
                            condition,
                            linq.Expression.Equal(
                                linq.Expression.Property(parameter, "LanguageCode"),
                                linq.Expression.Constant(name.ShortTextValue)));
                    }

                    var expression = linq.Expression.Lambda<Func<Localization, bool>>(condition, parameter);

                    var query = GetLocalizationsEnumerable(_repository).Where(expression);
                    foreach (var loc in query)
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
            return _cacheRepository.Get(name, category, culture);
        }

        public DateTime GetStatusDate()
        {
            return _cacheRepository.GetStatusDate();
        }

        public void SetStatusDate(DateTime lastModified)
        {
            _cacheRepository.SetStatusDate(lastModified);
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

            _cacheRepository.Add(element);


            Task.Run(() =>
            {
                using (var _repository = _repositoryFactory.GetRepositoryInstance())
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
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
            });

            return true;
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

        private IQueryable<Localization> GetLocalizationsEnumerable(IAppConfigRepository _repository)
        {
            var query = _repository.Localizations;
            if (_baseFilter != null)
                query = query.Where(_baseFilter);
            return query;
        }

        #endregion
    }
}
