using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using foundationModel = VirtoCommerce.Foundation.Inventories.Model;
using coreModel = VirtoCommerce.Domain.Inventory.Model;

namespace VirtoCommerce.InventoryModule.Data.Converters
{
	public static class InventoryConverter
	{
		/// <summary>
		/// Converting to model type
		/// </summary>
		/// <param name="catalogBase"></param>
		/// <returns></returns>
		public static coreModel.InventoryInfo ToCoreModel(this foundationModel.Inventory dbEntity)
		{
			if (dbEntity == null)
				throw new ArgumentNullException("dbEntity");

			var retVal = new coreModel.InventoryInfo();
			retVal.InjectFrom(dbEntity);
			retVal.Status = (coreModel.InventoryStatus)dbEntity.Status;
			retVal.ProductId = dbEntity.Sku;
			retVal.CreatedDate = dbEntity.Created.Value;
			retVal.ModifiedDate = dbEntity.LastModified;


			retVal.ReservedQuantity = (long)dbEntity.ReservedQuantity;
			retVal.BackorderQuantity = (long)dbEntity.BackorderQuantity;
			retVal.InStockQuantity = (long)dbEntity.InStockQuantity;
			retVal.PreorderQuantity = (long)dbEntity.PreorderQuantity;
			retVal.ReorderMinQuantity = (long)dbEntity.ReorderMinQuantity;
			

			return retVal;

		}


		public static foundationModel.Inventory ToFoundation(this coreModel.InventoryInfo inventory)
		{
			if (inventory == null)
				throw new ArgumentNullException("inventory");

			var retVal = new foundationModel.Inventory();

			retVal.InjectFrom(inventory);

			retVal.Sku = inventory.ProductId;
			retVal.Status = (int)inventory.Status;

			retVal.ReservedQuantity = inventory.ReservedQuantity;
			retVal.BackorderQuantity = inventory.BackorderQuantity;
			retVal.InStockQuantity = inventory.InStockQuantity;
			retVal.PreorderQuantity = inventory.PreorderQuantity;
			retVal.ReorderMinQuantity = inventory.ReorderMinQuantity;
			
		
			return retVal;
		}

		/// <summary>
		/// Patch changes
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void Patch(this foundationModel.Inventory source, foundationModel.Inventory target)
		{
			if (target == null)
				throw new ArgumentNullException("target");
			var patchInjection = new PatchInjection<foundationModel.Inventory>(x => x.Sku, x => x.Status,
																			   x => x.AllowBackorder, x => x.AllowPreorder,
																			   x => x.BackorderAvailabilityDate, x => x.BackorderQuantity,
																			   x => x.FulfillmentCenterId, x => x.InStockQuantity, x => x.PreorderAvailabilityDate,
																			   x => x.PreorderQuantity, x => x.ReorderMinQuantity, x => x.ReservedQuantity);
			target.InjectFrom(patchInjection, source);
		}


	}
}
