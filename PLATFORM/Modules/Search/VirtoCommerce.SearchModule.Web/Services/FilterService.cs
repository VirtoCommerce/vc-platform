using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;

namespace VirtoCommerce.SearchModule.Web.Services
{
    public class FilterService : IBrowseFilterService
    {
        private readonly IStoreService _storeService;
        private ISearchFilter[] _filters;

        public FilterService(IStoreService storeService)
        {
            _storeService = storeService;
        }

        public ISearchFilter[] GetFilters(IDictionary<string, object> context)
        {
            if (_filters != null)
            {
                return _filters;
            }

            var filters = new List<ISearchFilter>();

            if (context.ContainsKey("StoreId")) // include store filters
            {
                var store = _storeService.GetById(context["StoreId"].ToString());

                var browsing = GetFilteredBrowsing(store);
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

            _filters = filters.ToArray();
            return _filters;
        }


        private static FilteredBrowsing GetFilteredBrowsing(Store store)
        {
            FilteredBrowsing result = null;

            var filterSettingValue = store.GetDynamicPropertyValue("FilteredBrowsing", string.Empty);

            if (!string.IsNullOrEmpty(filterSettingValue))
            {
                var reader = new StringReader(filterSettingValue);
                var serializer = new XmlSerializer(typeof(FilteredBrowsing));
                result = serializer.Deserialize(reader) as FilteredBrowsing;
            }

            return result;
        }
    }
}
