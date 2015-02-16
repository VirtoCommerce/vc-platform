using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Inventory.Model;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.OrderModule.Data.Model;

namespace VirtoCommerce.OrderModule.Data.Interceptors
{
	/// <summary>
	/// Translates the changes in the model system order 
	/// </summary>
	public class InventoryOperationInterceptor : IInterceptor
	{
		private readonly IInventoryService _inventoryService;
		public InventoryOperationInterceptor(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		#region IInterceptor Members

		public void Before(InterceptionContext context)
		{
			var changedEntries = context.ChangeTracker.Entries().Where(entry => (entry.State == EntityState.Added) || (entry.State == EntityState.Modified) || (entry.State == EntityState.Deleted));
			foreach (var changedEntry in changedEntries)
			{
				var stockOutOperation = changedEntry.Entity as IStockOutOperation;
				if (stockOutOperation != null && stockOutOperation.IsApproved)
				{
					var inventoryInfos = _inventoryService.GetProductsInventoryInfos(stockOutOperation.Positions.Select(x => x.ProductId))
														  .ToArray();
					var changedInventoryInfos = new List<InventoryInfo>();
					foreach (var lineItem in stockOutOperation.Positions)
					{
						var inventoryInfo = inventoryInfos.FirstOrDefault(x => x.ProductId == lineItem.ProductId);
						if(inventoryInfo != null)
						{
							changedInventoryInfos.Add(inventoryInfo);
							if(changedEntry.State == EntityState.Deleted)
							{
								inventoryInfo.InStockQuantity += lineItem.Quantity; 
							}
							else if(changedEntry.State == EntityState.Added)
							{
								inventoryInfo.InStockQuantity -= lineItem.Quantity; 
							}
							else
							{
								var quantityProperty = changedEntry.Property("Quantity");
								var delta = (int)quantityProperty.CurrentValue - (int)quantityProperty.OriginalValue;
								inventoryInfo.InStockQuantity -= lineItem.Quantity; 
							}
						}
					}

					if (changedInventoryInfos.Any())
					{
						_inventoryService.UpsertInventories(changedInventoryInfos);
					}
				}
			}
		}

		public void After(InterceptionContext context)
		{
		}

		#endregion
	}
}
