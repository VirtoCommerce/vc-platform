using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Schemas;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.Foundation.Stores.Repositories;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{

    public class FilterService : IBrowseFilterService
    {
        private readonly Func<IStoreRepository> _storeRepository;
        private readonly ICacheRepository _cacheRepository;

        private readonly Func<ICatalogRepository> _catalogRepository;
        private ISearchFilter[] _filters;

        public FilterService(Func<IStoreRepository> storeRepository, Func<ICatalogRepository> catalogRepository, ICacheRepository cacheRepository)
        {
            _storeRepository = storeRepository;
            _cacheRepository = cacheRepository;
            _catalogRepository = catalogRepository;
        }

        #region Private Helpers
        /// <summary>
        /// Gets the store browse filters.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>Filtered browsing</returns>
        private FilteredBrowsing GetStoreBrowseFilters(Store store)
        {
            var filter = (from s in store.Settings where s.Name == "FilteredBrowsing" select s.LongTextValue).FirstOrDefault();
            if (!string.IsNullOrEmpty(filter))
            {
                var filterString = filter;
                var serializer = new XmlSerializer(typeof(FilteredBrowsing));
                var reader = new StringReader(filterString);
                var browsing = serializer.Deserialize(reader) as FilteredBrowsing;
                return browsing;
            }

            return null;
        }

        #endregion

        public ISearchFilter[] GetFilters(IDictionary<string, object> context)
        {
            if (_filters != null) return _filters;

            var filters = new List<ISearchFilter>();
            if (context.ContainsKey("CategoryId")) // include sub categories
            {
                /*
            // get category filters
                using (var repository = _catalogRepository())
                {
                    var children = repository.GetChildCategoriesById(context["CategoryId"]);
                    if (children != null)
                    {
                        var categoryFilter = new CategoryFilter { Key = "__outline" };
                        var listOfValues = (from child in children.OfType<Category>()
                            let outline =
                                String.Format(
                                    "{0}*",
                                    catalogClient.BuildCategoryOutline(
                                        _customerSession.CustomerSession.CatalogId,
                                        child))
                            select
                                new CategoryFilterValue
                                {
                                    Id = child.CategoryId,
                                    Outline = outline,
                                    Name = child.DisplayName()
                                }).ToList();

                        // add filters only if found any
                        if (listOfValues.Count > 0)
                        {
                            categoryFilter.Values = listOfValues.ToArray();
                            filters.Add(categoryFilter);
                        }
                    }
                }
                 * */
            }

            if (context.ContainsKey("StoreId")) // include store filters
            {
                using (var repository = _storeRepository())
                {
                    var storeId = context["StoreId"].ToString();
                    var store = repository.Stores.ExpandAll().SingleOrDefault(s => s.StoreId == storeId);

                    var browsing = GetStoreBrowseFilters(store);
                    if (browsing != null)
                    {
                        if (browsing.Attributes != null)
                        {
                            filters.AddRange(browsing.Attributes);
                        }
                        if (browsing.AttributeRanges != null)
                        {
                            filters.AddRange(browsing.AttributeRanges);
                        }
                        if (browsing.Prices != null)
                        {
                            filters.AddRange(browsing.Prices);
                        }
                    }
                }
            }

            _filters = filters.ToArray();
            return _filters;
        }
    }
}
