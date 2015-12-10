using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Xml.Serialization;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.SearchModule.Web.BackgroundJobs;
using webModel = VirtoCommerce.SearchModule.Web.Model;

namespace VirtoCommerce.SearchModule.Web.Controllers.Api
{
    [RoutePrefix("api/search")]
    [ApiExplorerSettings(IgnoreApi = true)]
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

        public SearchModuleController(ISearchProvider searchProvider, ISearchConnection searchConnection, SearchIndexJobsScheduler scheduler, IStoreService storeService, ISecurityService securityService, IPermissionScopeService permissionScopeService, IPropertyService propertyService)
        {
            _searchProvider = searchProvider;
            _searchConnection = searchConnection;
            _scheduler = scheduler;
            _storeService = storeService;
            _securityService = securityService;
            _permissionScopeService = permissionScopeService;
            _propertyService = propertyService;
        }

        [HttpGet]
        [Route("catalogitem")]
        [ResponseType(typeof(ISearchResults))]
        [CheckPermission(Permission = "VirtoCommerce.Search:Debug")]
        public IHttpActionResult Search([FromUri]CatalogIndexedSearchCriteria criteria)
        {
            criteria = criteria ?? new CatalogIndexedSearchCriteria();
            var scope = _searchConnection.Scope;
            var searchResults = _searchProvider.Search(scope, criteria);
            return Ok(searchResults);
        }

        [HttpGet]
        [Route("catalogitem/rebuild")]
        [CheckPermission(Permission = "VirtoCommerce.Search:Index:Rebuild")]
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
        /// <param name="id">Store ID</param>
        /// <responce code="404">Store not found</responce>
        /// <responce code="200"></responce>
        [HttpGet]
        [Route("storefilterproperties/{id}")]
        [ResponseType(typeof(webModel.FilterProperty[]))]
        public IHttpActionResult GetFilterProperties(string id)
        {
            var store = _storeService.GetById(id);
            if (store == null)
            {
                return NotFound();
            }

            CheckCurrentUserHasPermissionForObjects("read", store);

            var allProperties = GetAllCatalogProperties(store.Catalog);
            var selectedPropertyNames = GetSelectedFilterProperties(store);

            var filterProperties = allProperties
                .GroupBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
                .Select(g => ConvertToFilterProperty(g.FirstOrDefault(), selectedPropertyNames))
                .OrderBy(p => p.Name)
                .ToArray();

            // Keep the selected properties order
            var result = selectedPropertyNames
                .SelectMany(n => filterProperties.Where(p => string.Equals(p.Name, n)))
                .Union(filterProperties.Where(p => !selectedPropertyNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase)))
                .ToArray();

            return Ok(result);
        }

        /// <summary>
        /// Set filter properties for store
        /// </summary>
        /// <param name="id">Store ID</param>
        /// <param name="filterProperties"></param>
        /// <responce code="404">Store not found</responce>
        /// <responce code="204"></responce>
        [HttpPut]
        [Route("storefilterproperties/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult SetFilterProperties(string id, webModel.FilterProperty[] filterProperties)
        {
            var store = _storeService.GetById(id);
            if (store == null)
            {
                return NotFound();
            }

            CheckCurrentUserHasPermissionForObjects("read", store);

            var allProperties = GetAllCatalogProperties(store.Catalog);

            var selectedPropertyNames = filterProperties
                .Where(p => p.IsSelected)
                .Select(p => p.Name)
                .Distinct()
                .ToArray();

            // Keep the selected properties order
            var selectedDictionaryProperties = selectedPropertyNames
                .SelectMany(n => allProperties.Where(p => string.Equals(p.Name, n, StringComparison.OrdinalIgnoreCase)))
                .ToArray();

            var attributes = selectedDictionaryProperties
                .Select(ConvertToAttributeFilter)
                .GroupBy(a => a.Key)
                .Select(g => new AttributeFilter { Key = g.Key, Values = GetDistinctValues(g.SelectMany(p => p.Values)) })
                .ToArray();

            SetFilteredBrowsingAttributes(store, attributes);
            _storeService.Update(new[] { store });

            return StatusCode(HttpStatusCode.NoContent);
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


        private string[] GetSelectedFilterProperties(Store store)
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
            //var properties = _propertyService.GetAllCatalogProperties(catalogId);
            var properties = _propertyService.GetAllProperties();

            var result = properties
                .GroupBy(p => p.Id)
                .Select(g => g.FirstOrDefault())
                .OrderBy(p => p.Name)
                .ToArray();

            return result;
        }

        private static AttributeFilterValue[] GetDistinctValues(IEnumerable<AttributeFilterValue> values)
        {
            return values
                .Where(v => !string.IsNullOrEmpty(v.Id) && !string.IsNullOrEmpty(v.Value))
                .GroupBy(v => v.Id, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.FirstOrDefault())
                .OrderBy(v => v.Value)
                .ToArray();
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
            };

            return result;
        }

        private static AttributeFilterValue ConvertToAttributeFilterValue(PropertyDictionaryValue dictionaryValue)
        {
            var result = new AttributeFilterValue
            {
                Id = dictionaryValue.Alias,
                Value = dictionaryValue.Value,
            };

            return result;
        }
    }
}
