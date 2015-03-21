using System;
using System.Linq;

namespace VirtoCommerce.Client.Globalization.Repository
{
	/// <summary>
	/// Class CacheElementRepository.
	/// </summary>
	public class CacheElementRepository : IElementRepository
	{
		#region Fields
		/// <summary>
		/// The cached elements
		/// </summary>
		readonly System.Collections.Hashtable _cachedElements = new System.Collections.Hashtable(new ElementCacheKeyEqualityComparer());
		/// <summary>
		/// The inner
		/// </summary>
		private readonly IElementRepository _innerRepository;
		#endregion
		private const string StatusDateKeyInner = "~1ntern@|_st@tus_c@che_key!";

		#region .ctor
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheElementRepository" /> class.
		/// </summary>
		/// <param name="innerRepository">The inner repository.</param>
		public CacheElementRepository(IElementRepository innerRepository)
		{
			_innerRepository = innerRepository;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Enabled the languages.
		/// </summary>
		/// <returns>IQueryable{CultureInfo}.</returns>
		public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
		{
			return _innerRepository.EnabledLanguages();
		}

		/// <summary>
		/// Elements of this instance.
		/// </summary>
		/// <returns>IQueryable{Element}.</returns>
		public IQueryable<Element> Elements()
		{
			return _innerRepository.Elements();
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
			var cachedKey = new ElementCacheKey(name, category, culture);
			var element = _cachedElements[cachedKey] as Element;
			if (element == null)
			{
				var statusElement = GetStatusElement(culture);
				if (statusElement == null)
				{
					var preloadedCategoryLocalizations = _innerRepository.Elements().Where(it => it.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase));
					foreach (var preloadedCategoryLocalization in preloadedCategoryLocalizations)
					{
						AddCache(preloadedCategoryLocalization);
					}

					SetStatusElement(default(DateTime), culture);
					element = _cachedElements[cachedKey] as Element;
				}
			}
			return element;
		}

		public DateTime GetStatusDate()
		{
			return _innerRepository.GetStatusDate();
		}

		public void SetStatusDate(DateTime lastModified)
		{
			_innerRepository.SetStatusDate(lastModified);

			SetStatusElement(lastModified);
		}

		/// <summary>
		/// Adds the cache.
		/// </summary>
		/// <param name="element">The element.</param>
		public void AddCache(Element element)
		{
			var cacheKey = new ElementCacheKey(element);
			_cachedElements[cacheKey] = element;
		}
		/// <summary>
		/// Removes the cache.
		/// </summary>
		/// <param name="element">The element.</param>
		public void RemoveCache(Element element)
		{
			var cacheKey = new ElementCacheKey(element);
			lock (_cachedElements)
			{
				if (_cachedElements.ContainsKey(cacheKey))
				{
					_cachedElements.Remove(cacheKey);
				}
			}
		}
		/// <summary>
		/// Clears the cache.
		/// </summary>
		public void ClearCache()
		{
			lock (_cachedElements)
			{
				_cachedElements.Clear();
			}
		}
		/// <summary>
		/// Categories of this instance.
		/// </summary>
		/// <returns>IQueryable{ElementCategory}.</returns>
		public IQueryable<ElementCategory> Categories()
		{
			return _innerRepository.Categories();
		}

		/// <summary>
		/// Adds the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Add(Element element)
		{
			var r = _innerRepository.Add(element);
			if (r)
			{
				AddCache(element);
			}
			return r;
		}

		/// <summary>
		/// Updates the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Update(Element element)
		{
			var r = _innerRepository.Update(element);
			if (r)
			{
				RemoveCache(element);
			}
			return r;
		}

		/// <summary>
		/// Removes the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool Remove(Element element)
		{
			var r = _innerRepository.Remove(element);
			if (r)
			{
				RemoveCache(element);
			}
			return r;
		}

		/// <summary>
		/// Adds the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		public void AddCategory(string category, string culture)
		{
			_innerRepository.AddCategory(category, culture);
		}

		/// <summary>
		/// Removes the category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="culture">The culture.</param>
		/// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
		public bool RemoveCategory(string category, string culture)
		{
			var r = _innerRepository.RemoveCategory(category, culture);
			ClearCache();
			return r;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
		{
			_innerRepository.Clear();
			_cachedElements.Clear();
		}
		#endregion

		private Element GetStatusElement(string culture)
		{
			var cachedKey = new ElementCacheKey(StatusDateKeyInner, null, culture);
			return _cachedElements[cachedKey] as Element;
		}

		private void SetStatusElement(DateTime lastModified, string culture = "")
		{
			var element = new Element { Name = StatusDateKeyInner, Culture = culture, Value = lastModified.ToString() };
			AddCache(element);
		}
	}
}
