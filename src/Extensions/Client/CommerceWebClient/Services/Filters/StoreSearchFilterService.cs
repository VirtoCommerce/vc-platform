using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Web.Client.Services.Filters
{
    using System.IO;
    using System.Xml.Serialization;

    using VirtoCommerce.Client;
    using VirtoCommerce.Client.Extensions;
    using VirtoCommerce.Foundation.Catalogs.Model;
    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Customers.Services;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Schemas;
    using VirtoCommerce.Foundation.Stores.Model;

    public class StoreSearchFilterService : ISearchFilterService
    {
        private ISearchFilter[] _filters;
        private Store _store;

        private readonly ICustomerSessionService _customerSession;
        private readonly StoreClient _storeClient;

        public StoreSearchFilterService(StoreClient client, ICustomerSessionService customerSession)
        {
            _storeClient = client;
            _customerSession = customerSession;
        }

        public ISearchFilter[] Filters
        {
            get { return _filters ?? (_filters = GetStoreAllFilters(CurrentStore)); }
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
                TextReader reader = new StringReader(filterString);
                var browsing = serializer.Deserialize(reader) as FilteredBrowsing;
                return browsing;
            }

            return null;
        }

        private Store CurrentStore
        {
            get
            {
                return this._store ?? (this._store = this._storeClient.GetCurrentStore());
            }
        }

        /// <summary>
        /// Gets the store all filters.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns>ISearchFilter[][].</returns>
        private ISearchFilter[] GetStoreAllFilters(Store store)
        {
            var filters = new List<ISearchFilter>();

            var catalogClient = ClientContext.Clients.CreateCatalogClient();

            // get category filters
            var children = catalogClient.GetChildCategoriesById(_customerSession.CustomerSession.CategoryId);
            if (children != null)
            {
                var categoryFilter = new CategoryFilter { Key = "__outline" };
                var listOfValues = new List<CategoryFilterValue>();
                foreach (var child in children.OfType<Category>())
                {
                    var outline = String.Format("{0}*", catalogClient.BuildCategoryOutline(_customerSession.CustomerSession.CatalogId, child));
                    var val = new CategoryFilterValue() { Id = child.Code, Outline  = outline, Name = child.Name}; // TODO: get localized name
                    listOfValues.Add(val);
                }

                // add filters only if found any
                if (listOfValues.Count > 0)
                {
                    categoryFilter.Values = listOfValues.ToArray();
                    filters.Add(categoryFilter);
                }

                /*
                var categoryFilter = new AttributeFilter{ IsLocalized = false, Key = "__outline"};

                var listOfValues = new List<AttributeFilterValue>();
                foreach (var child in children.OfType<Category>())
                {
                    var outline = String.Format("{0}*", catalogClient.BuildCategoryOutline(_customerSession.CustomerSession.CatalogId, child));
                    var val = new AttributeFilterValue() { Id = outline, Value = outline };
                    listOfValues.Add(val);
                }

                // add filters only if found any
                if (listOfValues.Count > 0)
                {
                    categoryFilter.Values = listOfValues.ToArray();
                    filters.Add(categoryFilter);
                }
                 * */
            }


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

            return filters.ToArray();
        }

        #endregion
    }
}
