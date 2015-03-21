using System;
using System.Activities;
using System.Linq;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Inventories.Repositories;
using VirtoCommerce.Foundation.Customers.Services;

namespace VirtoCommerce.OrderWorkflow
{
    /// <summary>
    /// Adjusts the inventory levels in the inventory sub system based on the items ordered.
    /// </summary>
	public class AdjustInventoryActivity : OrderActivityBase
	{
        ICatalogRepository _catalogRepository;
        protected ICatalogRepository CatalogRepository
        {
            get { return _catalogRepository ?? (_catalogRepository = ServiceLocator.GetInstance<ICatalogRepository>()); }
	        set
            {
                _catalogRepository = value;
            }
        }

        ICustomerSessionService _customerSessionService;
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

        ICacheRepository _cacheRepository;
        protected ICacheRepository CacheRepository
        {
            get { return _cacheRepository ?? (_cacheRepository = ServiceLocator.GetInstance<ICacheRepository>()); }
	        set
            {
                _cacheRepository = value;
            }
        }

        IInventoryRepository _inventoryRepository;
        protected IInventoryRepository InventoryRepository
        {
            get { return _inventoryRepository ?? (_inventoryRepository = ServiceLocator.GetInstance<IInventoryRepository>()); }
	        set
            {
                _inventoryRepository = value;
            }
        }

        public AdjustInventoryActivity()
        {
        }

        public AdjustInventoryActivity(ICatalogRepository catalogRepository, IInventoryRepository inventoryRepository, ICustomerSessionService customerService, ICacheRepository cacheRepository)
        {
            _catalogRepository = catalogRepository;
            _inventoryRepository = inventoryRepository;
            _cacheRepository = cacheRepository;
            _customerSessionService = customerService;
        }

		protected override void Execute(CodeActivityContext context)
		{
			base.Execute(context);

			foreach (var lineItem in CurrentOrderGroup.OrderForms.SelectMany(f => f.LineItems))
			{
				AdjustStockItemQuantity(lineItem);
			}
		}

        private void AdjustStockItemQuantity(LineItem lineItem)
        {
            if (String.IsNullOrEmpty(lineItem.CatalogItemId) ||
                lineItem.CatalogItemId.StartsWith("@"))
            {
                return;
            }

            var catalogHelper = new CatalogClient(CatalogRepository, null, CustomerSessionService, CacheRepository, InventoryRepository);
            var item = catalogHelper.GetItem(lineItem.CatalogItemId);

			if (item != null && item.TrackInventory)
			{
                var repo = InventoryRepository;
                var inventory = catalogHelper.GetItemInventory(lineItem.CatalogItemId, lineItem.FulfillmentCenterId, false);
			    if (inventory != null)
			    {
				    if (AdjustStockInventoryQuantity(lineItem, inventory))
				    {
					    repo.UnitOfWork.Commit();
				    }
				    else
				    {
					    throw new InvalidWorkflowException(string.Format("Failed to adjust inventory for lineItem {0}", lineItem.LineItemId));
				    }
			    }
			}
        }

        private static bool AdjustStockInventoryQuantity(LineItem lineItem, Inventory inventory)
        {
            var delta = GetLineItemAdjustedQuantity(lineItem);

			var allowBackorder = inventory.AllowBackorder && 
				inventory.BackorderAvailabilityDate.HasValue && 
				inventory.BackorderAvailabilityDate.Value <= DateTime.Now;

			var allowPreorder = inventory.AllowPreorder &&
				inventory.PreorderAvailabilityDate.HasValue &&
				inventory.PreorderAvailabilityDate.Value <= DateTime.Now;

	        var inventoryAdjusted = false;

            //arrival
            if (delta > 0)
            {
                // need distribute delta between InStock, Backorder, Preorder.
                if (lineItem.InStockQuantity > 0)
                {                                 
	                var inStock = delta;

	                if (allowPreorder)
	                {
						var preorderdelta = Math.Min(delta, lineItem.PreorderQuantity - inventory.PreorderQuantity);
		                inventory.PreorderQuantity += preorderdelta;
						inStock -= preorderdelta;
	                }
	                if (allowBackorder)
	                {
						var backorderDelta = Math.Min(delta, lineItem.BackorderQuantity - inventory.BackorderQuantity);
		                inventory.BackorderQuantity += backorderDelta;
		                inStock -= backorderDelta;
	                }
					inventory.InStockQuantity += inStock;
					inventoryAdjusted = true;
                } //need distribute delta between Preorder and Backorder
                else if (lineItem.InStockQuantity == 0)
                {
                    if (lineItem.PreorderQuantity > 0 && allowPreorder)
                    {
                        inventory.PreorderQuantity += delta;
						inventoryAdjusted = true;
                    }
                    else if (lineItem.BackorderQuantity > 0 && allowBackorder)
                    {
                        inventory.BackorderQuantity += delta;
						inventoryAdjusted = true;
                    }
                }
            }//consumption
            else
            {
                delta = Math.Abs(delta);
                var instock = inventory.InStockQuantity - inventory.ReservedQuantity;
                if (instock >= delta) // Adjust the main inventory
                {
                    inventory.InStockQuantity -= delta;
					inventoryAdjusted = true;
                }
                else if (instock > 0) // there still exist items in stock
                {
	                if (allowBackorder)
	                {
		                // Calculate difference between currently available and backorder
		                var backorderDelta = delta - instock;

		                if (inventory.BackorderQuantity >= backorderDelta)
		                {
			                // Update inventory
			                inventory.InStockQuantity -= instock;
			                inventory.BackorderQuantity -= backorderDelta;
							inventoryAdjusted = true;
		                }
	                }
                }
                else if (instock == 0)
                {
					if (allowBackorder && inventory.BackorderQuantity >= delta)
					{
						inventory.BackorderQuantity -= delta;
						inventoryAdjusted = true;
					}
					else if (allowPreorder && inventory.PreorderQuantity >= delta)
					{
						inventory.PreorderQuantity -= delta;
						inventoryAdjusted = true;
					}
                }
            }

			return inventoryAdjusted;
        }

        /// <summary>
        /// Gets the line item adjusted quantity.
        /// </summary>
        /// <param name="lineItem">The line item.</param>
        /// <returns></returns>
        private static decimal GetLineItemAdjustedQuantity(LineItem lineItem)
        {
			//Need to find a way to determine if item was added removed or modified
			//Should return positive qty if item was removed, negative if added and exact qty when modified
            var retVal = -lineItem.Quantity;
            return retVal;
        }
	}
}
