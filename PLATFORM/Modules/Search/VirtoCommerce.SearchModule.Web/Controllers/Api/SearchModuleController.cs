using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Serialization;
using CacheManager.Core;
using VirtoCommerce.CatalogModule.Web.Converters;
using VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.CatalogModule.Web.Security;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Inventory.Model;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.SearchModule.Data.Services;
using VirtoCommerce.SearchModule.Web.BackgroundJobs;
using VirtoCommerce.SearchModule.Web.Security;
using VirtoCommerce.SearchModule.Web.Services;
using Property = VirtoCommerce.Domain.Catalog.Model.Property;
using PropertyDictionaryValue = VirtoCommerce.Domain.Catalog.Model.PropertyDictionaryValue;
using webModel = VirtoCommerce.SearchModule.Web.Model;

namespace VirtoCommerce.SearchModule.Web.Controllers.Api
{
    [RoutePrefix("api/search")]
    public class SearchModuleController : ApiController
    {
        private const string _filteredBrowsingPropertyName = "FilteredBrowsing";

        private readonly ISearchProvider _searchProvider;
        private readonly ISearchConnection _searchConnection;
        private readonly SearchIndexJobsScheduler _scheduler;
        private readonly IStoreService _storeService;
        private readonly ISecurityService _securityService;
        private readonly IPermissionScopeService _permissionScopeService;
        private readonly IPropertyService _propertyService;
        private readonly IBrowseFilterService _browseFilterService;
        private readonly IItemBrowsingService _browseService;
        private readonly IInventoryService _inventoryService;
        private readonly IBlobUrlResolver _blobUrlResolver;
        private readonly ICatalogSearchService _catalogSearchService;
        private readonly ICacheManager<object> _cacheManager;

        public SearchModuleController(ISearchProvider searchProvider, ISearchConnection searchConnection, SearchIndexJobsScheduler scheduler,
            IStoreService storeService, ISecurityService securityService, IPermissionScopeService permissionScopeService,
            IPropertyService propertyService, IBrowseFilterService browseFilterService, IItemBrowsingService browseService,
            IInventoryService inventoryService, IBlobUrlResolver blobUrlResolver, ICatalogSearchService catalogSearchService, ICacheManager<object> cacheManager)
        {
            _searchProvider = searchProvider;
            _searchConnection = searchConnection;
            _scheduler = scheduler;
            _storeService = storeService;
            _securityService = securityService;
            _permissionScopeService = permissionScopeService;
            _propertyService = propertyService;
            _browseFilterService = browseFilterService;
            _browseService = browseService;
            _inventoryService = inventoryService;
            _blobUrlResolver = blobUrlResolver;
            _catalogSearchService = catalogSearchService;
            _cacheManager = cacheManager;
        }

        [HttpGet]
        [Route("catalogitem")]
        [ResponseType(typeof(ISearchResults))]
        [CheckPermission(Permission = SearchPredefinedPermissions.Debug)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Debug([FromUri]CatalogIndexedSearchCriteria criteria)
        {
            criteria = criteria ?? new CatalogIndexedSearchCriteria();
            var scope = _searchConnection.Scope;
            var searchResults = _searchProvider.Search(scope, criteria);
            return Ok(searchResults);
        }

        [HttpGet]
        [Route("catalogitem/rebuild")]
        [CheckPermission(Permission = SearchPredefinedPermissions.RebuildIndex)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IHttpActionResult Rebuild()
        {
            var jobId = _scheduler.ScheduleRebuildIndex();
            var result = new { Id = jobId };
            return Ok(result);
        }

        /// <summary>
        /// Get filter properties for store
        /// </summary>
        /// <remarks>
        /// Returns all store catalog properties: selected properties are ordered manually, unselected properties are ordered by name.
        /// </remarks>
        /// <param name="storeId">Store ID</param>
        /// <responce code="404">Store not found</responce>
        /// <responce code="200"></responce>
        [HttpGet]
        [Route("storefilterproperties/{storeId}")]
        [ResponseType(typeof(webModel.FilterProperty[]))]
        public IHttpActionResult GetFilterProperties(string storeId)
        {
            var store = _storeService.GetById(storeId);
            if (store == null)
            {
                return NotFound();
            }

            CheckCurrentUserHasPermissionForObjects(SearchPredefinedPermissions.ReadFilterProperties, store);

            var allProperties = GetAllCatalogProperties(store.Catalog);
            var selectedPropertyNames = GetSelectedFilterProperties(store);

            var filterProperties = allProperties
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g => ConvertToFilterProperty(g.FirstOrDefault(), selectedPropertyNames))
                .OrderBy(p => p.Name)
                .ToArray();

            // Keep the selected properties order
            var result = selectedPropertyNames
                .SelectMany(n => filterProperties.Where(p => string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase)))
                .Union(filterProperties.Where(p => !selectedPropertyNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase)))
                .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <param name="storeId">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <responce code="404">Store not found</responce>
        /// <responce code="204"></responce>
        [HttpPut]
        [Route("storefilterproperties/{storeId}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SetFilterProperties(string storeId, webModel.FilterProperty[] filterProperties)
        {
            var store = _storeService.GetById(storeId);
            if (store == null)
            {
                return NotFound();
            }

            CheckCurrentUserHasPermissionForObjects(SearchPredefinedPermissions.UpdateFilterProperties, store);

            var allProperties = GetAllCatalogProperties(store.Catalog);

            var selectedPropertyNames = filterProperties
                .Where(p => p.IsSelected)
                .Select(p => p.Name)
                .Distinct()
                .ToArray();

            // Keep the selected properties order
            var selectedProperties = selectedPropertyNames
                .SelectMany(n => allProperties.Where(p => string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            var attributes = selectedProperties
                .Select(ConvertToAttributeFilter)
                .GroupBy(a => a.Key)
                .Select(g => new AttributeFilter
                {
                    Key = g.Key,
                    Values = GetDistinctValues(g.SelectMany(a => a.Values)),
                    IsLocalized = g.Any(a => a.IsLocalized),
                    DisplayNames = GetDistinctNames(g.SelectMany(a => a.DisplayNames)),
                })
                .ToArray();

            SetFilteredBrowsingAttributes(store, attributes);
            _storeService.Update(new[] { store });

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Search for products and categories
        /// </summary>
        /// <param name="criteria">Search parameters</param>
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(CatalogSearchResult))]
        [ClientCache(Duration = 30)]
        public IHttpActionResult Search(Domain.Catalog.Model.SearchCriteria criteria)
        {
            criteria = criteria ?? new Domain.Catalog.Model.SearchCriteria();
            criteria.Normalize();
            criteria.ApplyRestrictionsForUser(User.Identity.Name, _securityService);

            var result = new Domain.Catalog.Model.SearchResult();

            if ((criteria.ResponseGroup & SearchResponseGroup.WithProducts) == SearchResponseGroup.WithProducts)
            {
                result = SearchProducts(criteria);
            }

            var catalogResponseGroup = criteria.ResponseGroup & (SearchResponseGroup.WithCatalogs | SearchResponseGroup.WithCategories);

            if (catalogResponseGroup != SearchResponseGroup.None)
            {
                criteria.ResponseGroup = catalogResponseGroup;
                var catalogResult = _catalogSearchService.Search(criteria);
                result.Catalogs = catalogResult.Catalogs;
                result.Categories = catalogResult.Categories;
            }

            return Ok(result.ToWebModel(_blobUrlResolver));
        }


        private Domain.Catalog.Model.SearchResult SearchProducts(Domain.Catalog.Model.SearchCriteria criteria)
        {
            var context = new Dictionary<string, object>
            {
                { "StoreId", criteria.StoreId },
            };

            var catalog = criteria.CatalogId;
            var categoryId = criteria.CategoryId;

            var serviceCriteria = new CatalogIndexedSearchCriteria
            {
                Locale = criteria.LanguageCode,
                Catalog = catalog.ToLowerInvariant(),
                IsFuzzySearch = true,
            };

            if (!string.IsNullOrWhiteSpace(criteria.Outline))
            {
                serviceCriteria.Outlines.Add(string.Format(CultureInfo.InvariantCulture, "{0}/{1}*", catalog, criteria.Outline));
                categoryId = criteria.Outline.Split('/').Last();
            }
            else
            {
                if (!string.IsNullOrEmpty(categoryId))
                {
                    serviceCriteria.Outlines.Add(string.Format(CultureInfo.InvariantCulture, "{0}/{1}*", catalog, categoryId));
                }
            }

            if (!string.IsNullOrEmpty(categoryId))
            {
                context.Add("CategoryId", categoryId);
            }

            #region Filters
            // Now fill in filters
            var filters = _cacheManager.Get("GetFilters-" + criteria.StoreId, "SearchProducts", TimeSpan.FromMinutes(5), () => _browseFilterService.GetFilters(context));

            // Add all filters
            foreach (var filter in filters)
            {
                serviceCriteria.Add(filter);
            }

            // apply terms
            var terms = ParseKeyValues(criteria.Terms);
            if (terms.Any())
            {
                var filtersWithValues = filters
                    .Where(x => (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => new { Filter = x, Values = x.GetValues() })
                    .ToList();

                foreach (var term in terms)
                {
                    var filter = filters.SingleOrDefault(x => x.Key.Equals(term.Key, StringComparison.OrdinalIgnoreCase)
                        && (!(x is PriceRangeFilter) || ((PriceRangeFilter)x).Currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase)));

                    // handle special filter term with a key = "tags", it contains just values and we need to determine which filter to use
                    if (filter == null && term.Key == "tags")
                    {
                        foreach (var termValue in term.Values)
                        {
                            // try to find filter by value
                            var foundFilter = filtersWithValues.FirstOrDefault(x => x.Values.Any(y => y.Id.Equals(termValue)));

                            if (foundFilter != null)
                            {
                                filter = foundFilter.Filter;

                                var appliedFilter = _browseFilterService.Convert(filter, term.Values);
                                serviceCriteria.Apply(appliedFilter);
                            }
                        }
                    }
                    else
                    {
                        var attributeFilter = filter as AttributeFilter;
                        if (attributeFilter != null && attributeFilter.Values == null)
                        {
                            var dynamicValues = new List<AttributeFilterValue>();
                            foreach (var value in term.Values)
                            {
                                dynamicValues.Add(new AttributeFilterValue()
                                {
                                    Id = value,
                                    Value = value
                                });
                            }
                            attributeFilter.Values = dynamicValues.ToArray();
                        }

                        var appliedFilter = _browseFilterService.Convert(filter, term.Values);
                        serviceCriteria.Apply(appliedFilter);
                    }
                }
            }
            #endregion

            #region Facets
            // apply facet filters
            var facets = ParseKeyValues(criteria.Facets);
            foreach (var facet in facets)
            {
                var filter = filters.SingleOrDefault(
                    x => x.Key.Equals(facet.Key, StringComparison.OrdinalIgnoreCase)
                        && (!(x is PriceRangeFilter)
                            || ((PriceRangeFilter)x).Currency.Equals(criteria.Currency, StringComparison.OrdinalIgnoreCase)));

                var appliedFilter = _browseFilterService.Convert(filter, facet.Values);
                serviceCriteria.Apply(appliedFilter);
            }
            #endregion

            //criteria.ClassTypes.Add("Product");
            serviceCriteria.RecordsToRetrieve = criteria.Take < 0 ? 10 : criteria.Take;
            serviceCriteria.StartingRecord = criteria.Skip;
            serviceCriteria.Pricelists = criteria.PricelistIds;
            serviceCriteria.Currency = criteria.Currency;
            serviceCriteria.StartDateFrom = criteria.StartDateFrom;
            serviceCriteria.SearchPhrase = criteria.Keyword;

            #region sorting

            if (!criteria.SortInfos.IsNullOrEmpty())
            {
                var sortInfo = criteria.SortInfos.FirstOrDefault();
                var isDescending = sortInfo.SortDirection == SortDirection.Descending;
                SearchSort sortObject = null;

                switch (sortInfo.SortColumn.ToLowerInvariant())
                {
                    case "price":
                        if (serviceCriteria.Pricelists != null)
                        {
                            sortObject = new SearchSort(
                                serviceCriteria.Pricelists.Select(
                                    priceList =>
                                        new SearchSortField(string.Format("price_{0}_{1}", serviceCriteria.Currency.ToLower(), priceList.ToLower()))
                                        {
                                            IgnoredUnmapped = true,
                                            IsDescending = isDescending,
                                            DataType = SearchSortField.DOUBLE
                                        })
                                    .ToArray());
                        }
                        break;
                    case "position":
                        sortObject =
                            new SearchSort(
                                new SearchSortField(string.Concat("sort", catalog, categoryId).ToLower())
                                {
                                    IgnoredUnmapped = true,
                                    IsDescending = isDescending
                                });
                        break;
                    case "name":
                    case "title":
                        sortObject = new SearchSort("name", isDescending);
                        break;
                    case "rating":
                        sortObject = new SearchSort(serviceCriteria.ReviewsAverageField, isDescending);
                        break;
                    case "reviews":
                        sortObject = new SearchSort(serviceCriteria.ReviewsTotalField, isDescending);
                        break;
                    default:
                        sortObject = CatalogIndexedSearchCriteria.DefaultSortOrder;
                        break;
                }

                serviceCriteria.Sort = sortObject;
            }

            #endregion

            var responseGroup = ItemResponseGroup.ItemInfo | ItemResponseGroup.ItemAssets | ItemResponseGroup.Seo;

            if ((criteria.ResponseGroup & SearchResponseGroup.WithProperties) == SearchResponseGroup.WithProperties)
            {
                responseGroup |= ItemResponseGroup.ItemProperties;
            }

            if ((criteria.ResponseGroup & SearchResponseGroup.WithVariations) == SearchResponseGroup.WithVariations)
            {
                responseGroup |= ItemResponseGroup.Variations;
            }

            if ((criteria.ResponseGroup & SearchResponseGroup.WithOutlines) == SearchResponseGroup.WithOutlines)
            {
                responseGroup |= ItemResponseGroup.Outlines;
            }

            //Load ALL products 
            var searchResults = _browseService.SearchItems(serviceCriteria, responseGroup);

            //// populate inventory
            ////if ((request.ResponseGroup & ItemResponseGroup.ItemProperties) == ItemResponseGroup.ItemProperties)
            //if ((criteria.ResponseGroup & SearchResponseGroup.WithProperties) == SearchResponseGroup.WithProperties)
            //{
            //    PopulateInventory(store.FulfillmentCenter, searchResults.Products);
            //}

            return searchResults;
        }


        protected void CheckCurrentUserHasPermissionForObjects(string permission, params object[] objects)
        {
            //Scope bound security check
            var scopes = objects.SelectMany(x => _permissionScopeService.GetObjectPermissionScopeStrings(x)).Distinct().ToArray();
            if (!_securityService.UserHasAnyPermission(User.Identity.Name, scopes, permission))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }


        private static string[] GetSelectedFilterProperties(Store store)
        {
            var result = new List<string>();

            var browsing = GetFilteredBrowsing(store);
            if (browsing != null && browsing.Attributes != null)
            {
                result.AddRange(browsing.Attributes.Select(a => a.Key));
            }

            return result.ToArray();
        }

        private static FilteredBrowsing GetFilteredBrowsing(Store store)
        {
            FilteredBrowsing result = null;

            var filterSettingValue = store.GetDynamicPropertyValue(_filteredBrowsingPropertyName, string.Empty);

            if (!string.IsNullOrEmpty(filterSettingValue))
            {
                var reader = new StringReader(filterSettingValue);
                var serializer = new XmlSerializer(typeof(FilteredBrowsing));
                result = serializer.Deserialize(reader) as FilteredBrowsing;
            }

            return result;
        }

        private static void SetFilteredBrowsingAttributes(Store store, AttributeFilter[] attributes)
        {
            var browsing = GetFilteredBrowsing(store) ?? new FilteredBrowsing();
            browsing.Attributes = attributes;
            var serializer = new XmlSerializer(typeof(FilteredBrowsing));
            var builder = new StringBuilder();
            var writer = new StringWriter(builder);
            serializer.Serialize(writer, browsing);
            var value = builder.ToString();

            var property = store.DynamicProperties.FirstOrDefault(p => p.Name == _filteredBrowsingPropertyName);

            if (property == null)
            {
                property = new DynamicObjectProperty { Name = _filteredBrowsingPropertyName };
                store.DynamicProperties.Add(property);
            }

            property.Values = new List<DynamicPropertyObjectValue>(new[] { new DynamicPropertyObjectValue { Value = value } });
        }

        private Property[] GetAllCatalogProperties(string catalogId)
        {
            var properties = _propertyService.GetAllCatalogProperties(catalogId);

            var result = properties
                .GroupBy(p => p.Id)
                .Select(g => g.FirstOrDefault())
                .OrderBy(p => p.Name)
                .ToArray();

            return result;
        }

        private static FilterDisplayName[] GetDistinctNames(IEnumerable<FilterDisplayName> names)
        {
            return names
                .Where(n => !string.IsNullOrEmpty(n.Language) && !string.IsNullOrEmpty(n.Name))
                .GroupBy(n => n.Language, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.FirstOrDefault())
                .OrderBy(n => n.Language)
                .ThenBy(n => n.Name)
                .ToArray();
        }

        private static AttributeFilterValue[] GetDistinctValues(IEnumerable<AttributeFilterValue> values)
        {
            return values
                .Where(v => !string.IsNullOrEmpty(v.Id) && !string.IsNullOrEmpty(v.Value))
                .GroupBy(v => v.Id, StringComparer.OrdinalIgnoreCase)
                .SelectMany(g => g
                    .GroupBy(g2 => g2.Language, StringComparer.OrdinalIgnoreCase)
                    .SelectMany(g2 => g2
                        .GroupBy(g3 => g3.Value, StringComparer.OrdinalIgnoreCase)
                        .Select(g3 => g3.FirstOrDefault())))
                .OrderBy(v => v.Id)
                .ThenBy(v => v.Language)
                .ThenBy(v => v.Value)
                .ToArray();
        }

        private static List<StringKeyValues> ParseKeyValues(string[] items)
        {
            var result = new List<StringKeyValues>();

            if (items != null)
            {
                var nameValueDelimeter = new[] { ':' };
                var valuesDelimeter = new[] { ',' };

                result.AddRange(items
                    .Select(item => item.Split(nameValueDelimeter, 2))
                    .Where(item => item.Length == 2)
                    .Select(item => new StringKeyValues { Key = item[0], Values = item[1].Split(valuesDelimeter, StringSplitOptions.RemoveEmptyEntries) })
                    .GroupBy(item => item.Key)
                    .Select(g => new StringKeyValues { Key = g.Key, Values = g.SelectMany(i => i.Values).Distinct().ToArray() })
                    );
            }

            return result;
        }

        private void PopulateInventory(FulfillmentCenter center, IEnumerable<CatalogProduct> products)
        {
            if (center == null || products == null || !products.Any())
                return;

            var inventories = _inventoryService.GetProductsInventoryInfos(products.Select(x => x.Id).ToArray()).ToList();

            foreach (var product in products)
            {
                var productInventory = inventories.FirstOrDefault(x => x.ProductId == product.Id && x.FulfillmentCenterId == center.Id);
                if (productInventory != null)
                    product.Inventories = new List<InventoryInfo> { productInventory };
            }
        }

        private static webModel.FilterProperty ConvertToFilterProperty(Property property, string[] selectedPropertyNames)
        {
            return new webModel.FilterProperty
            {
                Name = property.Name,
                IsSelected = selectedPropertyNames.Contains(property.Name, StringComparer.OrdinalIgnoreCase),
            };
        }

        private AttributeFilter ConvertToAttributeFilter(Property property)
        {
            var values = _propertyService.SearchDictionaryValues(property.Id, null);

            var result = new AttributeFilter
            {
                Key = property.Name,
                Values = values.Select(ConvertToAttributeFilterValue).ToArray(),
                IsLocalized = property.Multilanguage,
                DisplayNames = property.DisplayNames.Select(ConvertToFilterDisplayName).ToArray(),
            };

            return result;
        }

        private static FilterDisplayName ConvertToFilterDisplayName(PropertyDisplayName displayName)
        {
            var result = new FilterDisplayName
            {
                Language = displayName.LanguageCode,
                Name = displayName.Name,
            };

            return result;
        }

        private static AttributeFilterValue ConvertToAttributeFilterValue(PropertyDictionaryValue dictionaryValue)
        {
            var result = new AttributeFilterValue
            {
                Id = dictionaryValue.Alias,
                Value = dictionaryValue.Value,
                Language = dictionaryValue.LanguageCode,
            };

            return result;
        }

        private class StringKeyValues
        {
            public string Key { get; set; }
            public string[] Values { get; set; }
        }
    }
}
