using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Search;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Search;

namespace VirtoCommerce.Client
{
    public class CatalogClient
    {
        #region Cache Constants
        public const string SearchCacheKey = "S:{0}:{1}";
        public const string CatalogCacheKey = "C:C:{0}";
        public const string CategoryCacheKey = "C:CT:{0}:{1}";
        public const string CategoryItemRelationCacheKey = "C:CTIREL:{0}";
        public const string CategoryIdCacheKey = "C:CTID:{0}:{1}";
        public const string ChildCategoriesCacheKey = "C:CT:{0}:p:{1}";
        //public const string ItemCacheKey = "C:I:{0}:g:{1}";
        public const string ItemsCacheKey = "C:Is:{0}:g:{1}";
        public const string ItemsCodeCacheKey = "C:Isc:{0}:g:{1}";
        public const string ItemsSearchCacheKey = "C:Is:{0}:{1}";
        public const string ItemsQueryCacheKey = "C:Is:{0}";
        public const string PriceListCacheKey = "C:PL:{0}";
        public const string ItemVariationsCacheKey = "C:V:{0}";
        public const string ItemInvetoriesCacheKey = "C:INV:{0}:{1}";

        public const string PricesCacheKey = "C:P:{0}";
        public const string ItemPricesCacheKey = "C:P:{0}:{1}";
        public const string PricelistAssignmentCacheKey = "C:PLA:{0}";

        public const string PropertiesCacheKey = "C:PR:{0}";
        public const string PropertyValueCacheKey = "C:PRV:{0}:{1}:{2}:{3}";
        #endregion

        #region Private Variables
        private readonly bool _isEnabled;
        private readonly ICatalogRepository _catalogRepository;
        private readonly ICustomerSessionService _customerSession;
        private readonly ICacheRepository _cacheRepository;
        private readonly ICatalogService _catalogService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ISearchConnection _searchConnection;
        private readonly ICatalogOutlineBuilder _catalogOutlineBuilder;
        #endregion

        public CatalogClient(ICatalogRepository catalogRepository,
            ICatalogService catalogService,
            ICustomerSessionService customerSession,
            ICacheRepository cacheRepository,
            IInventoryRepository inventoryRepository,
            ICatalogOutlineBuilder catalogOutlineBuilder = null,
            ISearchConnection searchConnection = null)
        {
            _catalogService = catalogService;
            _catalogRepository = catalogRepository;
            _cacheRepository = cacheRepository;
            _customerSession = customerSession;
            _inventoryRepository = inventoryRepository;
            _searchConnection = searchConnection;
            _catalogOutlineBuilder = catalogOutlineBuilder;
            _isEnabled = CatalogConfiguration.Instance.Cache.IsEnabled;
        }

        public ICustomerSession CustomerSession
        {
            get
            {
                return _customerSession.CustomerSession;
            }
        }

        public ICatalogRepository CatalogRepository
        {
            get
            {
                return _catalogRepository;
            }
        }

        public CatalogBase GetCatalog(string catalogId, bool useCache = true)
        {
            var query = _catalogRepository.Catalogs.Where(x => x.CatalogId.Equals(catalogId, StringComparison.OrdinalIgnoreCase));

            return Helper.Get(
                string.Format(CatalogCacheKey, catalogId),
                () => (query).SingleOrDefault(),
                CatalogConfiguration.Instance.Cache.CatalogTimeout,
                _isEnabled && useCache);
        }

        /// <summary>
        /// Gets CategoryItemRelations by item id
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public CategoryItemRelation[] GetCategoryItemRelations(string itemId, bool useCache = true)
        {
            var query = _catalogRepository.CategoryItemRelations.Where(x => x.ItemId.Equals(itemId, StringComparison.OrdinalIgnoreCase));

            return Helper.Get(
                string.Format(CategoryItemRelationCacheKey, itemId),
                () => (query).ToArray(),
                CatalogConfiguration.Instance.Cache.CategoryTimeout,
                _isEnabled && useCache);
        }

        public CatalogOutline BuildCategoryOutline(string catalogId, CategoryBase category)
        {
            return _catalogOutlineBuilder.BuildCategoryOutline(catalogId, category);
        }

        #region Item Methods

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="bycode">if set to <c>true</c> [bycode].</param>
        /// <returns></returns>
        public Item GetItem(string id, bool useCache = true, bool bycode = false)
        {
            return GetItem(id, ItemResponseGroups.ItemSmall, useCache, bycode);
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="responseGroup">The response group.</param>
        /// <param name="useCache">if set to <c>true</c> [use cache].</param>
        /// <param name="bycode">if set to <c>true</c> [bycode].</param>
        /// <returns></returns>
        public Item GetItem(string id, ItemResponseGroups responseGroup, bool useCache = true, bool bycode = false)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var items = bycode ? GetItemsByCode(new[] { id }, useCache, responseGroup) : GetItems(new[] { id }, useCache, responseGroup);
                if (items != null && items.Any())
                    return items[0];
            }

            return null;
        }

        /// <summary>
        /// Gets the item. Additionally filters by catalog
        /// </summary>
        /// <param name="id">The id of item.</param>
        /// <param name="responseGroup">The response group.</param>
        /// <param name="catalogId">The catalog id.</param>
        /// <param name="useCache">if set to <c>true</c> uses cache.</param>
        /// <param name="bycode">if set to <c>true</c> get item by code.</param>
        /// <returns></returns>
        public Item GetItem(string id, ItemResponseGroups responseGroup, string catalogId, bool useCache = true, bool bycode = false)
        {
            var item = GetItem(id, responseGroup, useCache, bycode);

            if (item != null)
            {
                if (item.CatalogId == catalogId)
                    return item;

                var relations = item.CategoryItemRelations.ToArray();

                if (!responseGroup.HasFlag(ItemResponseGroups.ItemCategories))
                {
                    relations = GetCategoryItemRelations(id);
                }

                foreach (var rel in relations)
                {
                    if (rel.CatalogId == catalogId)
                        return item;

                    var category = GetCategoryById(rel.CategoryId);

                    if (category != null)
                    {
                        if (category.LinkedCategories.Any(link => link.CatalogId == catalogId))
                        {
                            return item;
                        }
                    }
                }
            }

            return null;
        }

        public Item[] GetItems(string[] ids)
        {
            return GetItems(ids, true, ItemResponseGroups.ItemSmall);
        }

        public Item[] GetItems(string[] ids, bool useCache, ItemResponseGroups responseGroup)
        {
            if (ids == null || !ids.Any())
                return null;

            var query = _catalogRepository.Items.Where(x => ids.Contains(x.ItemId));
            query = IncludeGroups(query, responseGroup);

            return Helper.Get(
                string.Format(ItemsCacheKey, CacheHelper.CreateCacheKey("", ids), responseGroup),
                () => (query).ToArray(),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                _isEnabled && useCache);
        }

        public Item[] GetItemsByCode(string[] ids)
        {
            return GetItemsByCode(ids, true, ItemResponseGroups.ItemSmall);
        }

        public Item[] GetItemsByCode(string[] codes, bool useCache, ItemResponseGroups responseGroup)
        {
            if (codes == null || !codes.Any())
                return null;

            var query = _catalogRepository.Items.Where(x => codes.Contains(x.Code));
            query = IncludeGroups(query, responseGroup);

            return Helper.Get(
                string.Format(ItemsCodeCacheKey, CacheHelper.CreateCacheKey("", codes), responseGroup),
                () => (query).ToArray(),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                _isEnabled && useCache);
        }

        private IQueryable<Item> IncludeGroups(IQueryable<Item> query, ItemResponseGroups responseGroups)
        {
            if (responseGroups.HasFlag(ItemResponseGroups.ItemAssets))
            {
                query = query.Expand(p => p.ItemAssets);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemAssociations))
            {
                query = query.Expand(p => p.AssociationGroups.Select(a => a.Associations.Select(s => s.CatalogItem)));
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemCategories))
            {
                query = query.Expand(p => p.CategoryItemRelations);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemEditorialReviews))
            {
                query = query.Expand(p => p.EditorialReviews);
            }

            if (responseGroups.HasFlag(ItemResponseGroups.ItemProperties))
            {
                query = query.Expand(p => p.ItemPropertyValues);
            }

            return query;
        }
        #endregion

        #region Properties Methods
        public PropertySet GetPropertySet(string propertySetId)
        {
            var sets = GetPropertySets();
            var set = sets.Where(x => x.PropertySetId.Equals(propertySetId, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
            return set;
        }

        public PropertySet[] GetPropertySets(bool useCache = true)
        {
            return Helper.Get(
                string.Format(PropertiesCacheKey, "all"),
                () => (from p in _catalogRepository.PropertySets select p)
                    .Expand("PropertySetProperties/Property/PropertyValues")
                    .Expand("PropertySetProperties/Property/PropertyAttributes")
                    .ToArray(),
                CatalogConfiguration.Instance.Cache.PropertiesTimeout,
                _isEnabled && useCache);
        }

        public PropertyValueBase GetPropertyValueByName(StorageEntity storageEntity, string name, bool expandProperties = false, string locale = "", bool useCache = true)
        {
            string id;
            string catalogId;
            IEnumerable<PropertyValueBase> properties;

            var item = storageEntity as Item;
            var category = storageEntity as Category;

            if (item != null)
            {
                id = item.ItemId;
                catalogId = item.CatalogId;
                properties = item.ItemPropertyValues;
            }
            else if (category != null)
            {
               
                id = category.CategoryId;
                catalogId = category.CatalogId;
                properties = category.CategoryPropertyValues;
                if (!properties.Any() && expandProperties)
                {
                    //This will return expanded category with properties
                    category = GetCategoryByIdInternal(id) as Category;

                    if(category !=null)
                    {
                        properties = category.CategoryPropertyValues;
                    }
                }

            }
            else
            {
                return null;
            }


            if (string.IsNullOrEmpty(locale))
            {
                locale = CultureInfo.CurrentUICulture.Name;
            }

            return Helper.Get(
               string.Format(PropertyValueCacheKey, id, name, locale, storageEntity.GetType().Name),
               () =>
               {

                   var avaialbleProps = (from p in properties
                                         where p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                         select p).ToArray();

                   var catalog = GetCatalog(catalogId);

                   if (avaialbleProps.Any())
                   {

                       return (from p in avaialbleProps
                               where string.IsNullOrEmpty(p.Locale) || locale.Equals(p.Locale, StringComparison.InvariantCultureIgnoreCase)
                               select p).FirstOrDefault() ??

                               (from p in avaialbleProps
                                where string.IsNullOrEmpty(p.Locale) ||
                                catalog.DefaultLanguage.Equals(p.Locale, StringComparison.InvariantCultureIgnoreCase)
                                select p).FirstOrDefault();
                   }

                   return null;
               },
               CatalogConfiguration.Instance.Cache.PropertiesTimeout,
               _isEnabled && useCache);
        }

        public Property[] GetProperties(bool useCache = true)
        {
            var sets = GetPropertySets(useCache);

            var propertiesRel = sets.SelectMany(m => m.PropertySetProperties).ToArray();
            var properties = propertiesRel.Select(x => x.Property).ToArray();

            return properties;
        }

        public Property GetProperty(string key)
        {
            var properties = GetProperties();
            if (properties != null)
            {
                var props = (from p in properties where p.Name.Equals(key, StringComparison.OrdinalIgnoreCase) select p).ToArray();
                return props.Count() > 1 ? props.First() : props.SingleOrDefault();
            }

            return null;
        }

        public string GetPropertyName(string key)
        {
            var prop = GetProperty(key);
            return prop != null ? prop.Name : String.Empty;
        }
        #endregion

        #region Category Methods
        private CategoryBase GetCategoryInternal(string catalogId, string code)
        {
            //Get simple category
            CategoryBase category = _catalogRepository.Categories.OfType<Category>()
                .Expand(c => c.LinkedCategories)
                .Expand(c => c.CategoryPropertyValues)
                .FirstOrDefault(x => (x.CatalogId == catalogId) && (x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));

            if (category == null)
            {
                //Get linked category
                category = _catalogRepository.Categories.OfType<LinkedCategory>()
                    .Expand(c => c.LinkedCategories)
                    .FirstOrDefault(x => (x.CatalogId == catalogId) && (x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));
                if (category != null && category.IsActive)
                {
                    //Get simple category from linked catalog
                    category = _catalogRepository.Categories.OfType<Category>()
                        .Expand(p => p.CategoryPropertyValues)
                        .FirstOrDefault(x => (x.CatalogId == ((LinkedCategory)category).LinkedCatalogId) && (x.Code.Equals(code, StringComparison.OrdinalIgnoreCase)));
                }
            }

            return category;
        }

        private CategoryBase GetCategoryByIdInternal(string id)
        {
            //Get simple category
            CategoryBase category = _catalogRepository.Categories.OfType<Category>()
                .Expand(c => c.LinkedCategories)
                .Expand(c => c.CategoryPropertyValues)
                .FirstOrDefault(x => x.CategoryId.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                //Get linked category
                category = _catalogRepository.Categories.OfType<LinkedCategory>()
                    .Expand(c => c.LinkedCategories)
                    .FirstOrDefault(x => x.CategoryId.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (category != null && category.IsActive)
                {
                    //Get simple category from linked catalog
                    category = _catalogRepository.Categories.OfType<Category>()
                        .Expand(p => p.CategoryPropertyValues)
                        .FirstOrDefault(x => (x.CatalogId == ((LinkedCategory)category).LinkedCatalogId) && (x.CategoryId.Equals(id, StringComparison.OrdinalIgnoreCase)));
                }
            }

            return category;
        }



        public CategoryBase GetCategory(string code, bool useCache = true)
        {

            return Helper.Get(
                string.Format(CategoryCacheKey, _customerSession.CustomerSession.CatalogId, code),
                () => GetCategoryInternal(_customerSession.CustomerSession.CatalogId, code),
                CatalogConfiguration.Instance.Cache.CategoryTimeout,
                _isEnabled && useCache);
        }

        public CategoryBase GetCategoryById(string id, bool useCache = true)
        {

            return Helper.Get(
                string.Format(CategoryIdCacheKey, _customerSession.CustomerSession.CatalogId, id),
                () => GetCategoryByIdInternal(id),
                CatalogConfiguration.Instance.Cache.CategoryTimeout,
                _isEnabled && useCache);
        }

        #endregion

        #region Search
        public CatalogItemSearchResults SearchItems(CatalogItemSearchCriteria criteria, bool useCache)
        {
            var scope = _searchConnection.Scope;

            return Helper.Get(
                string.Format(SearchCacheKey, scope, criteria.CacheKey),
                () => _catalogService.SearchItems(scope, criteria),
                SearchConfiguration.Instance.Cache.FiltersTimeout,
                _isEnabled);
        }
        #endregion

        #region Variations

        public ItemRelation[] GetItemRelations(string itemId)
        {
            return Helper.Get(
                string.Format(ItemVariationsCacheKey, itemId),
                () => _catalogRepository.ItemRelations.Where(ir => ir.ParentItemId == itemId).Expand(ir => ir.ChildItem).Expand(ir => ir.ChildItem.ItemPropertyValues).ToArray(),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                _isEnabled);
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Gets item inventory
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="fulfillmentCenterId">The fulfillment center id.</param>
        /// <returns></returns>
        public Inventory GetItemInventory(string itemId, string fulfillmentCenterId)
        {
            return Helper.Get(String.Format(ItemInvetoriesCacheKey, itemId, fulfillmentCenterId),
                () => _inventoryRepository.Inventories.SingleOrDefault(i => i.FulfillmentCenterId.Equals(fulfillmentCenterId, StringComparison.OrdinalIgnoreCase) &&
                                                                         i.Sku.Equals(itemId, StringComparison.OrdinalIgnoreCase)),
                CatalogConfiguration.Instance.Cache.ItemTimeout,
                _isEnabled);
        }

        /// <summary>
        /// Gets items invetories
        /// </summary>
        /// <param name="itemIds">The item ids.</param>
        /// <param name="fulfillmentCenterId">The fulfillment center id.</param>
        /// <returns></returns>
        public IEnumerable<Inventory> GetItemInventories(string[] itemIds, string fulfillmentCenterId)
        {
            return Helper.Get(String.Format(ItemInvetoriesCacheKey, CacheHelper.CreateCacheKey("", itemIds), fulfillmentCenterId),
                () => _inventoryRepository.Inventories.Where(i => i.FulfillmentCenterId.Equals(fulfillmentCenterId, StringComparison.OrdinalIgnoreCase) && itemIds.Contains(i.Sku)).ToArray(),
                CatalogConfiguration.Instance.Cache.ItemCollectionTimeout,
                _isEnabled);
        }

        /// <summary>
        /// Get item availability collection for given items
        /// </summary>
        /// <param name="itemIds"></param>
        /// <param name="fulfillmentCenterId"></param>
        /// <returns></returns>
        public ItemAvailability[] GetItemAvailability(string[] itemIds, string fulfillmentCenterId)
        {
            var inventories = GetItemInventories(itemIds, fulfillmentCenterId);
            return GetItems(itemIds).Select(item =>
                GetItemAvailabilityInternal(item, inventories.SingleOrDefault(i => i.Sku == item.ItemId))).ToArray();
        }

        /// <summary>
        /// Gets the item availability.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="fulfillmentCenterId">The fulfillment center id.</param>
        /// <returns></returns>
        public ItemAvailability GetItemAvailability(string itemId, string fulfillmentCenterId)
        {
            var item = GetItem(itemId);
            var inventory = item.TrackInventory ? GetItemInventory(itemId, fulfillmentCenterId) : null;
            return GetItemAvailabilityInternal(item, inventory);
        }

        private ItemAvailability GetItemAvailabilityInternal(Item item, Inventory inventory)
        {
            var retVal = new ItemAvailability
            {
                MinQuantity = item.MinQuantity,
                MaxQuantity = item.MaxQuantity,
                ItemId = item.ItemId
            };

            if (item.IsBuyable
                && item.StartDate < CustomerSession.CurrentDateTime
                && (!item.EndDate.HasValue || item.EndDate > CustomerSession.CurrentDateTime))
            {
                if (item.TrackInventory)
                {
                    if (inventory != null && (InventoryStatus)inventory.Status == InventoryStatus.Enabled)
                    {
                        var inStock = inventory.InStockQuantity - inventory.ReservedQuantity;

                        if (inStock > 0
                            && (item.AvailabilityRule == (int)AvailabilityRule.Always
                                || item.AvailabilityRule == (int)AvailabilityRule.WhenInStock))
                        {
                            retVal.MaxQuantity = inStock;
                            retVal.Availability = ItemStoreAvailabity.InStore;
                            retVal.MaxQuantity = Math.Max(retVal.MinQuantity, retVal.MaxQuantity);
                            retVal.MinQuantity = Math.Min(retVal.MinQuantity, retVal.MaxQuantity);
                            return retVal;
                        }
                        if (inventory.AllowBackorder
                            && inventory.BackorderAvailabilityDate.HasValue
                            && (item.AvailabilityRule == (int)AvailabilityRule.Always
                                || item.AvailabilityRule == (int)AvailabilityRule.OnBackorder))
                        {
                            retVal.MaxQuantity = inStock + inventory.BackorderQuantity;

                            if (retVal.MaxQuantity > 0)
                            {
                                retVal.Availability = ItemStoreAvailabity.AvailableForBackOrder;
                                retVal.Date = inventory.BackorderAvailabilityDate;
                                retVal.MaxQuantity = Math.Max(retVal.MinQuantity, retVal.MaxQuantity);
                                retVal.MinQuantity = Math.Min(retVal.MinQuantity, retVal.MaxQuantity);
                                return retVal;
                            }
                        }

                        if (inventory.AllowPreorder
                            && inventory.PreorderAvailabilityDate.HasValue)
                        {

                            retVal.MaxQuantity = inventory.PreorderQuantity;

                            if (retVal.MaxQuantity > 0 &&
                                (item.AvailabilityRule == (int)AvailabilityRule.Always ||
                                 item.AvailabilityRule == (int)AvailabilityRule.OnPreorder))
                            {
                                retVal.Availability = ItemStoreAvailabity.AvailableForPreOrder;
                                retVal.Date = inventory.PreorderAvailabilityDate;
                                retVal.MaxQuantity = Math.Max(retVal.MinQuantity, retVal.MaxQuantity);
                                retVal.MinQuantity = Math.Min(retVal.MinQuantity, retVal.MaxQuantity);
                                return retVal;
                            }
                        }
                    }
                }
                else
                {
                    retVal.MaxQuantity = item.MaxQuantity;
                    if (retVal.MaxQuantity > 0
                        && (item.AvailabilityRule == (int)AvailabilityRule.Always
                            || item.AvailabilityRule == (int)AvailabilityRule.WhenInStock))
                    {
                        retVal.Availability = ItemStoreAvailabity.InStore;
                    }
                    retVal.MaxQuantity = Math.Max(retVal.MinQuantity, retVal.MaxQuantity);
                    retVal.MinQuantity = Math.Min(retVal.MinQuantity, retVal.MaxQuantity);
                }
            }

            return retVal;
        }

        #endregion

        CacheHelper _cacheHelper;


        public CacheHelper Helper
        {
            get { return _cacheHelper ?? (_cacheHelper = new CacheHelper(_cacheRepository)); }
        }
    }

    [Flags]
    public enum ItemResponseGroups
    {
        ItemInfo = 0,
        ItemAssets = 1 << 1,
        ItemProperties = 1 << 2,
        ItemCategories = 1 << 3,
        ItemAssociations = 1 << 4,
        ItemEditorialReviews = 1 << 5,
        ItemCustomerReviews = 1 << 6,
        ItemSmall = ItemInfo | ItemAssets | ItemProperties,
        ItemMedium = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews,
        ItemLarge = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews | ItemCategories | ItemCustomerReviews
    }

    public enum ItemStoreAvailabity
    {
        OutOfStore = 0,
        InStore,
        AvailableForBackOrder,
        AvailableForPreOrder
    }

    public struct ItemAvailability
    {
        public DateTime? Date { get; set; }
        public ItemStoreAvailabity Availability { get; set; }
        public decimal MaxQuantity { get; set; }
        public decimal MinQuantity { get; set; }
        public string ItemId { get; set; }

        public bool IsAvailable
        {
            get
            {
                switch (Availability)
                {
                    case ItemStoreAvailabity.OutOfStore:
                        return false;
                    case ItemStoreAvailabity.AvailableForBackOrder:
                    case ItemStoreAvailabity.AvailableForPreOrder:
                        if (!Date.HasValue || Date.Value > DateTime.Now)
                        {
                            return false;
                        }
                        break;
                }
                return true;
            }
        }
    }
}
