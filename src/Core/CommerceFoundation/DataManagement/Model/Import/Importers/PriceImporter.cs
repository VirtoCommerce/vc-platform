using System.Linq;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class PriceImporter : EntityImporterBase
	{
		private IPricelistRepository _repository;
		private ICatalogRepository _catalogRepository;

		public PriceImporter(ICatalogRepository catalogRepository)
		{
			_catalogRepository = catalogRepository;
			Name = ImportEntityType.Price.ToString();
			InitializeSystemProperties();			
			var catalogs = _catalogRepository.Catalogs.ToList();
			catalogs.ForEach(cat => SystemProperties.First(prop => prop.Name == "CatalogId").EnumValues.Add(cat.Name));
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var priceId = new ImportProperty { Name = "PriceId", DisplayName = "Price Id", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var priceSalePrice = new ImportProperty { Name = "Sale", DisplayName = "Sale price", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var priceListPrice = new ImportProperty { Name = "List", DisplayName = "List price", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var priceMinQuantity = new ImportProperty { Name = "MinQuantity", DisplayName = "Minimum quantity", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var pricePriceListId = new ImportProperty { Name = "PricelistId", DisplayName = "Pricelist Id", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var priceItemId = new ImportProperty { Name = "ItemId", DisplayName = "Item Id/code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var priceCatalogId = new ImportProperty { Name = "CatalogId", DisplayName = "Catalog", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name, IsEnumValuesProperty = true};

			AddSystemProperties(
				action, priceId, priceSalePrice, priceListPrice,
				priceMinQuantity, pricePriceListId, priceItemId, priceCatalogId);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_catalogRepository = (ICatalogRepository) repository;
			_repository = (IPricelistRepository) repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);
			var itemId = systemValues.First(y => y.Name == "ItemId").Value;
			var catName = systemValues.First(y => y.Name == "CatalogId").Value;
			if (!string.IsNullOrEmpty(catName))
				catalogId = _catalogRepository.Catalogs.Where(cat => cat.Name == catName).First().CatalogId;

			//get item with the provided ItemId or item Code
			var originalItems = _catalogRepository.Items.Where(x => x.ItemId == itemId || x.Code == itemId).ToArray();
			Item originalItem;

			//if more than 1 item found and catalogId is not provided - return error, otherwise ok
			if (originalItems.Count() > 1)
			{
				if (!string.IsNullOrEmpty(catalogId))
					originalItem = originalItems.FirstOrDefault(x => x.CatalogId == catalogId);
				else
				{
					var catNames = string.Empty;
					originalItems.ToList().ForEach(x => catNames = catNames + x.CatalogId + ", ");
					_error = string.Format("Item with code - {0} has matches in catalogs: {1}. Specify catalog property.", itemId, catNames);
					return _error;
				}
			}
			else
			{
				originalItem = originalItems.FirstOrDefault();
			}
			
			//if item with the code found (not null) try to execute the import action otherwise return error
			if (originalItem != null)
			{
				switch (action)
				{
					case ImportAction.Insert:
						var addItem = InitializeItem(null, systemValues);
						//set price itemId to the found ItemId
						addItem.ItemId = originalItem.ItemId;
						_repository.Add(addItem);

						break;
					case ImportAction.InsertAndReplace:
						var qtyR = systemValues.FirstOrDefault(y => y.Name == "MinQuantity");
						var listIdR = systemValues.FirstOrDefault(y => y.Name == "PricelistId");
						
						if (qtyR != null && listIdR != null)
						{
							var pricelist = _repository.Pricelists.Where(x => x.Name == listIdR.Value || x.PricelistId == listIdR.Value).FirstOrDefault();
							var qtyVal = int.Parse(qtyR.Value);
							var origItem = _repository.Prices.ToList().Where(x => x.ItemId == originalItem.ItemId && x.MinQuantity == qtyVal && x.PricelistId == pricelist.PricelistId).SingleOrDefault();
							if (origItem != null)
							{
								InitializeItem(origItem, systemValues);
								//set price itemId to the found ItemId
								origItem.ItemId = originalItem.ItemId;
								_repository.Update(origItem);
							}
							else
							{
								var newItem = InitializeItem(null, systemValues);
								//set price itemId to the found ItemId
								newItem.ItemId = originalItem.ItemId;
								_repository.Add(newItem);
							}
						}
						break;
					case ImportAction.Update:
						var qty = systemValues.FirstOrDefault(y => y.Name == "MinQuantity");
						var listId = systemValues.FirstOrDefault(y => y.Name == "PricelistId");
						if (qty != null && listId != null)
						{
							var pricelist = _repository.Pricelists.Where(x => x.Name == listId.Value || x.PricelistId == listId.Value).FirstOrDefault();
							var qtyVal = int.Parse(qty.Value);
							var origItem = _repository.Prices.ToList().Where(x => x.ItemId == originalItem.ItemId && x.MinQuantity == qtyVal && x.PricelistId == pricelist.PricelistId).SingleOrDefault();
							if (origItem != null)
							{
								InitializeItem(origItem, systemValues);
								//set price itemId to the found ItemId
								origItem.ItemId = originalItem.ItemId;
								_repository.Update(origItem);
							}
						}
						break;
					case ImportAction.Delete:
						var qtyValue = systemValues.FirstOrDefault(y => y.Name == "MinQuantity");
						var pricelistId = systemValues.FirstOrDefault(y => y.Name == "PricelistId");
						if (qtyValue != null && pricelistId != null)
						{
							var qtyVal = int.Parse(qtyValue.Value);
							var deleteItem = _repository.Prices.ToList().Where(x => x.ItemId == originalItem.ItemId && x.MinQuantity == qtyVal && x.PricelistId == pricelistId.Value).SingleOrDefault();
							if (deleteItem != null)
								_repository.Remove(deleteItem);
						}
						break;
				}
			}
			else
			{
				_error = string.Format("Item with itemId/code - {0} not found", itemId);
			}
			return _error;
		}

		private static Price InitializeItem(Price item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new CatalogEntityFactory().CreateEntity<Price>();
			var priceId = systemValues.FirstOrDefault(price => price.Name == "PriceId");
			if (priceId != null)
				systemValues.FirstOrDefault(price => price.Name == "PriceId").Value = priceId.Value ?? item.PriceId;
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
