using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using VirtoCommerce.Domain.Search.Filters;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;
using VirtoCommerce.Domain.Store.Model;
using VirtoCommerce.Domain.Store.Services;

namespace VirtoCommerce.MerchandisingModule.Web.Services
{
    public class FilterService : IBrowseFilterService
    {
        private readonly IStoreService _storeService;
        private ISearchFilter[] _filters;

        public FilterService(IStoreService storeService)
        {
            _storeService = storeService;
        }

        #region Public Methods and Operators

        public ISearchFilter[] GetFilters(IDictionary<string, object> context)
        {
            if (this._filters != null)
            {
                return _filters;
            }

            var filters = new List<ISearchFilter>();

            if (context.ContainsKey("StoreId")) // include store filters
            {
                var store = _storeService.GetById(context["StoreId"].ToString());

                var browsing = this.GetStoreBrowseFilters(store);
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

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the store browse filters.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>Filtered browsing</returns>
        private FilteredBrowsing GetStoreBrowseFilters(Store store)
        {
            var filterSetting = store.Settings.FirstOrDefault(x => x.Name == "FilteredBrowsing");
            if (filterSetting != null && filterSetting.Value != null)
            {
                var serializer = new XmlSerializer(typeof(FilteredBrowsing));
                var reader = new StringReader(filterSetting.Value.ToString());
                var browsing = serializer.Deserialize(reader) as FilteredBrowsing;
                return browsing;
            }

            return null;
        }

        #endregion
    }
}
