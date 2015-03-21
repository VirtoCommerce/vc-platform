using System;
using System.Activities;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Currencies;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Services;

namespace VirtoCommerce.OrderWorkflow
{
    /// <summary>
    /// Checks if items are still available, are in stock and the prices (if changed, notification is generated).
    /// Also copies latest values from the catalog into the lineitem (quantities, prices, ids and so on).
    /// </summary>
	public class ValidateLineItemsActivity : OrderActivityBase
    {
        #region Repositories
        /// <summary>
        /// The _catalog repository
        /// </summary>
        ICatalogRepository _catalogRepository;
        /// <summary>
        /// Gets or sets the catalog repository.
        /// </summary>
        /// <value>The catalog repository.</value>
        protected ICatalogRepository CatalogRepository
        {
            get { return _catalogRepository ?? (_catalogRepository = ServiceLocator.GetInstance<ICatalogRepository>()); }
	        set
            {
                _catalogRepository = value;
            }
        }

        /// <summary>
        /// The _store repository
        /// </summary>
        IStoreRepository _storeRepository;
        /// <summary>
        /// Gets or sets the store repository.
        /// </summary>
        /// <value>The store repository.</value>
        protected IStoreRepository StoreRepository
        {
            get { return _storeRepository ?? (_storeRepository = ServiceLocator.GetInstance<IStoreRepository>()); }
	        set
            {
                _storeRepository = value;
            }
        }

        /// <summary>
        /// The _pricelist repository
        /// </summary>
        IPricelistRepository _pricelistRepository;
        /// <summary>
        /// Gets or sets the pricelist repository.
        /// </summary>
        /// <value>The pricelist repository.</value>
        protected IPricelistRepository PricelistRepository
        {
            get { return _pricelistRepository ?? (_pricelistRepository = ServiceLocator.GetInstance<IPricelistRepository>()); }
	        set
            {
                _pricelistRepository = value;
            }
        }

        /// <summary>
        /// The _customer session service
        /// </summary>
        ICustomerSessionService _customerSessionService;
        /// <summary>
        /// Gets or sets the customer session service.
        /// </summary>
        /// <value>The customer session service.</value>
        protected ICustomerSessionService CustomerSessionService
        {
            get {
	            return _customerSessionService ??
	                   (_customerSessionService = ServiceLocator.GetInstance<ICustomerSessionService>());
            }
	        set
            {
                _customerSessionService = value;
            }
        }

        /// <summary>
        /// The _currency service
        /// </summary>
        ICurrencyService _currencyService;
        /// <summary>
        /// Gets or sets the currency service.
        /// </summary>
        /// <value>The currency service.</value>
        protected ICurrencyService CurrencyService
        {
            get { return _currencyService ?? (_currencyService = ServiceLocator.GetInstance<ICurrencyService>()); }
	        set
            {
                _currencyService = value;
            }
        }


        /// <summary>
        /// The _inventory repository
        /// </summary>
        IInventoryRepository _inventoryRepository;
        /// <summary>
        /// Gets or sets the inventory repository.
        /// </summary>
        /// <value>The inventory repository.</value>
        protected IInventoryRepository InventoryRepository
        {
            get { return _inventoryRepository ?? (_inventoryRepository = ServiceLocator.GetInstance<IInventoryRepository>()); }
	        set
            {
                _inventoryRepository = value;
            }
        }

        /// <summary>
        /// The _price list eval context
        /// </summary>
        IPriceListAssignmentEvaluationContext _priceListEvalContext;
        /// <summary>
        /// Gets or sets the price list eval context.
        /// </summary>
        /// <value>The price list eval context.</value>
        protected IPriceListAssignmentEvaluationContext PriceListEvalContext
        {
            get {
	            return _priceListEvalContext ??
	                   (_priceListEvalContext = ServiceLocator.GetInstance<IPriceListAssignmentEvaluationContext>());
            }
	        set
            {
                _priceListEvalContext = value;
            }
        }

        /// <summary>
        /// The _price list evaluator
        /// </summary>
        IPriceListAssignmentEvaluator _priceListEvaluator;
        /// <summary>
        /// Gets or sets the price list evaluator.
        /// </summary>
        /// <value>The price list evaluator.</value>
        protected IPriceListAssignmentEvaluator PriceListEvaluator
        {
            get {
	            return _priceListEvaluator ??
	                   (_priceListEvaluator = ServiceLocator.GetInstance<IPriceListAssignmentEvaluator>());
            }
	        set
            {
                _priceListEvaluator = value;
            }
        }

        /// <summary>
        /// The _cache repository
        /// </summary>
		ICacheRepository _cacheRepository;
        /// <summary>
        /// Gets or sets the cache repository.
        /// </summary>
        /// <value>The cache repository.</value>
		protected ICacheRepository CacheRepository
		{
			get { return _cacheRepository ?? (_cacheRepository = ServiceLocator.GetInstance<ICacheRepository>()); }
			set
			{
				_cacheRepository = value;
			}
		}
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateLineItemsActivity"/> class.
        /// </summary>
        public ValidateLineItemsActivity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateLineItemsActivity"/> class.
        /// </summary>
        /// <param name="inventoryRepository">The inventory repository.</param>
        /// <param name="catalogRepository">The catalog repository.</param>
        /// <param name="storeRepository">The store repository.</param>
        /// <param name="customerService">The customer service.</param>
        /// <param name="priceListRepository">The price list repository.</param>
        /// <param name="currencyService">The currency service.</param>
        /// <param name="priceListEvaluator">The price list evaluator.</param>
        /// <param name="priceListEvalContext">The price list eval context.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        public ValidateLineItemsActivity(IInventoryRepository inventoryRepository, 
			ICatalogRepository catalogRepository, 
            IStoreRepository storeRepository, 
			ICustomerSessionService customerService, 
			IPricelistRepository priceListRepository,
            ICurrencyService currencyService, 
			IPriceListAssignmentEvaluator priceListEvaluator, 
			IPriceListAssignmentEvaluationContext priceListEvalContext,
			ICacheRepository cacheRepository)
        {
            _inventoryRepository = inventoryRepository;
            _catalogRepository = catalogRepository;
            _storeRepository = storeRepository;
            _customerSessionService = customerService;
            _pricelistRepository = priceListRepository;
            _currencyService = currencyService;
            _priceListEvalContext = priceListEvalContext;
	        _cacheRepository = cacheRepository;
	        _priceListEvaluator = priceListEvaluator;
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Execute(CodeActivityContext context)
		{
			base.Execute(context);

            if (ServiceLocator == null)
                return;

            if (CurrentOrderGroup == null || CurrentOrderGroup.OrderForms.Count == 0)
                return;

            ValidateItems();
		}

        /// <summary>
        /// Validates the items.
        /// </summary>
		private void ValidateItems()
		{
			var orderForms = CurrentOrderGroup.OrderForms.ToArray();
			var lineItems = orderForms.SelectMany(x => x.LineItems.ToArray());
			var validLineItems = lineItems.Where(x => !String.IsNullOrEmpty(x.CatalogItemId) && !x.CatalogItemId.StartsWith("@")).ToArray();

			string catalogId = null;

            // get the store and compare
			if (validLineItems.Any())
			{
                var store = new StoreClient(StoreRepository, CustomerSessionService, CacheRepository).GetStoreById(CurrentOrderGroup.StoreId);
                catalogId = store.Catalog;
			}

            if (!String.IsNullOrEmpty(catalogId))
            {
                var catalogHelper = new CatalogClient(CatalogRepository, null, CustomerSessionService, CacheRepository, InventoryRepository);
                var catalogItemIds = (from i in validLineItems select i.CatalogItemId).ToArray();
                var items = catalogHelper.GetItems(catalogItemIds);

                foreach (var lineItem in validLineItems)
                {
                    var item = items.SingleOrDefault(i => i.ItemId.Equals(lineItem.CatalogItemId, StringComparison.OrdinalIgnoreCase));

                    if (item != null && catalogHelper.GetItemAvailability(item.ItemId, lineItem.FulfillmentCenterId).IsAvailable)
                    {
                        //Inventory Info
                        if (item.TrackInventory)
                        {
                            var inventory = catalogHelper.GetItemInventory(lineItem.CatalogItemId, lineItem.FulfillmentCenterId);
                            PopulateInventoryInfo(inventory, lineItem);
                        }

                        //Variation Info
                        PopulateVariationInfo(item, lineItem);
                    }
                    else // remove item from the list
                    {
                        RemoveLineItem(lineItem);
                    }
                }
            }
            else // since no catalog is associated with a store, remove all items
            {
                RemoveAllLineItems();
            }
		}

        /// <summary>
        /// Removes all line items.
        /// </summary>
        private void RemoveAllLineItems()
        {
            foreach (var form in CurrentOrderGroup.OrderForms)
            {
                while (form.LineItems.Count > 0)
                    RemoveLineItem(form.LineItems[0]);
            }
        }


        /// <summary>
        /// Removes the line item.
        /// </summary>
        /// <param name="lineItem">The line item.</param>
        private void RemoveLineItem(LineItem lineItem)
        {
	        foreach (var form in CurrentOrderGroup.OrderForms.Where(form => form.LineItems.Contains(lineItem)))
	        {
		        form.LineItems.Remove(lineItem);

                foreach (var shipmentLineItem in form.Shipments.SelectMany(s => s.ShipmentItems.ToArray()).Where(x => x.LineItem == lineItem))
                {
                    shipmentLineItem.Shipment.ShipmentItems.Remove(shipmentLineItem);
                }
	        }
        }

        /// <summary>
        /// Populates the variation information.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="lineItem">The line item.</param>
	    private void PopulateVariationInfo(Item item, LineItem lineItem)
		{
			if (item == null)
			{
				return;
			}

			lineItem.MaxQuantity = item.MaxQuantity;
			lineItem.MinQuantity = item.MinQuantity;

			var priceListHelper = new PriceListClient(PricelistRepository, CustomerSessionService, PriceListEvaluator, PriceListEvalContext, CacheRepository);

			var itemPrice = priceListHelper.GetLowestPrice(item.ItemId, lineItem.Quantity);

			if (itemPrice == null)
			{
				RemoveLineItem(lineItem);
				return;
			}

			// Get old and new prices for comparison.
			var oldPrice = lineItem.ListPrice;

            // new logic is to have the price always be replaced
            var newPrice = itemPrice.Sale ?? itemPrice.List;

			newPrice = Math.Round(newPrice, 2); // Comparison. The LineItem.ListPrice property rounds values.

			// Get old and new currencies for comparison.
			var oldCurrencyCode = CurrentOrderGroup.BillingCurrency;
			var newCurrencyCode = CustomerSessionService.CustomerSession.Currency;
			if (string.IsNullOrEmpty(oldCurrencyCode))
			{
				oldCurrencyCode = newCurrencyCode;
			}

			// Check for price changes OR currency changes.
			if (oldPrice == newPrice && oldCurrencyCode == newCurrencyCode)
			{
				return;
			}

			// Set new price and currency vales on line item.
			lineItem.ListPrice = newPrice;
            lineItem.PlacedPrice = newPrice;
            lineItem.ExtendedPrice = lineItem.PlacedPrice * lineItem.Quantity;
			CurrentOrderGroup.BillingCurrency = newCurrencyCode;
		}

        /// <summary>
        /// Populates the inventory information.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="lineItem">The line item.</param>
		private void PopulateInventoryInfo(Inventory inventory, LineItem lineItem)
		{
			
			if (inventory == null)
			{
				return;
			}

			// Inventory info
			lineItem.AllowBackorders = inventory.AllowBackorder &&
					 inventory.BackorderAvailabilityDate.HasValue &&
					 inventory.BackorderAvailabilityDate.Value <= DateTime.Now;

			lineItem.AllowPreorders = inventory.AllowPreorder &&
								inventory.PreorderAvailabilityDate.HasValue &&
								inventory.PreorderAvailabilityDate.Value <= DateTime.Now;


			//Init quantities once
			lineItem.BackorderQuantity = lineItem.BackorderQuantity == 0 ? inventory.BackorderQuantity : lineItem.BackorderQuantity;
			lineItem.InStockQuantity = lineItem.InStockQuantity == 0 ? inventory.InStockQuantity - inventory.ReservedQuantity : lineItem.InStockQuantity;
			lineItem.PreorderQuantity = lineItem.PreorderQuantity == 0  ? inventory.PreorderQuantity : lineItem.PreorderQuantity;
			lineItem.InventoryStatus = ((InventoryStatus)inventory.Status).ToString();
		}
	}
}
